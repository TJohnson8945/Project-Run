﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMove : MonoBehaviour
{
    public Transform camera;
    public Rigidbody playerBody;
    public GameObject body;
    [Range(1.0f, 10.0f)]
    public float camRotSpeed;
    [Range(1.0f, 10.0f)]
    public float deadZone;
    public float rotSmoothSpeed = 10f;

    public float walkSpeed = 12f;
    public float crouchSpeed = 7f;
    public float runSpeed = 18f;
    public float maxspeed = 50f;
    public float jumpForce = 20f;
    public float xtraGrav = 45;

    float xBodyRot;
    float camRotY;
    Vector3 directIntentX;
    Vector3 directIntentY;
    public float speed;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public bool isGrounded;
    public bool isSprinting;
    public bool isCrouching;
    public bool extraJump;

    public LayerMask whatIsWall;
    public float wallrunForce, maxWallrunTime, maxWallSpeed;
    public bool isWallRight, isWallLeft, isWallForward;
    public bool isWallRunning, isWallClimbing;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    public Transform orientation;
    public float xAxis = 0;
    public float yAxis = 0;
    public float camX = 0;
    public float camY = 0;
    private string os;

    // Start is called before the first frame update
    void Start()
    {
        os = SystemInfo.operatingSystem;
        Cursor.lockState = CursorLockMode.Locked;
        speed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Camera();
        Move();
        Jump();
        CheckForWall();
        WallRunInput();
        ExtraJump();
        xAxis = Input.GetAxis("move 1");
        yAxis = Input.GetAxis("move 2");
        camX = Input.GetAxis("turn 1");
        camY = Input.GetAxis("turn 2");

        if (isWallForward && Input.GetKey(KeyCode.W))
        {
            WallClimb();
        }
            
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        ExtraGrav();
    }

    void Camera()
    {
        
        //Get cam and body rotation
        xBodyRot += Input.GetAxis("turn 1") * camRotSpeed;
        if(os.Contains("Mac")){
            camRotY += Input.GetAxis("turn 2 mac") * camRotSpeed;
        }
        else{
            camRotY += Input.GetAxis("turn 2") * camRotSpeed;
        }
        xBodyRot += Input.GetAxis("Mouse X") * camRotSpeed;
        camRotY += Input.GetAxis("Mouse Y") * camRotSpeed;

        //stop camera from rotate 360 on y axis
        camRotY = Mathf.Clamp(camRotY, -75f, 75f);

        //Create quaternions for rotations
        Quaternion camTargetRot = Quaternion.Euler(-camRotY, 0, 0); 
        Quaternion playerTargetRot = Quaternion.Euler(0, xBodyRot, 0);

        //doing rotations
        transform.rotation = Quaternion.Lerp(transform.rotation, playerTargetRot, Time.deltaTime * rotSmoothSpeed); 
        camera.localRotation = Quaternion.Lerp(camera.localRotation, camTargetRot, Time.deltaTime * rotSmoothSpeed); 

        //While Wallrunning
        //Tilts camera in .5 second
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallRight)
        {
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
            camera.transform.localRotation = Quaternion.Euler(0, 0, wallRunCameraTilt);
        }
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
        {
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
            camera.transform.localRotation = Quaternion.Euler(0, 0, wallRunCameraTilt);

        }


        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft && !isWallRunning)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft && !isWallRunning)
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
    }

    void Move()
    {
        //make player direction match camera
        directIntentX = camera.right;
        directIntentX.y = 0;
        directIntentX.Normalize();

        directIntentY = camera.forward;
        directIntentY.y = 0;
        directIntentY.Normalize();


        //decide speed and if sprinting/crouching
        if(Input.GetButtonDown("joystick button 2")){
            if(isCrouching){
                speed = walkSpeed;
                body.transform.localScale = new Vector3(1f, 1.25f,1f);
            }else{
                speed = crouchSpeed;
                body.transform.localScale = new Vector3(1f,.625f,1f);
            }
            isCrouching = !isCrouching;
        }
        if(Input.GetButtonDown("joystick button 10")){
            if(isSprinting){
                speed = walkSpeed;
                isSprinting = false;
            }
            else if(isCrouching){ // where we slide
                isCrouching = false;
                speed = walkSpeed;
                isSprinting = false;
                body.transform.localScale = new Vector3(1f, 1.25f, 1f);
            }
            else{
                isSprinting = true;
                speed = runSpeed;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(isSprinting){
                speed = walkSpeed;
            }else{
                speed = runSpeed;
                isSprinting = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            body.transform.localScale = new Vector3(1f,.625f,1f);
            isCrouching = true;
            speed = crouchSpeed;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            body.transform.localScale = new Vector3(1f, 1.25f,1f);
            isCrouching = false;
            speed = walkSpeed;
        }

        //change velocity
        if(isGrounded)
        {   
            if(Input.GetAxis("Vertical") < .1 && Input.GetAxis("turn 1") < .1){
                isSprinting = false;
                speed = walkSpeed;
            }
            playerBody.velocity = directIntentY * Input.GetAxis("Vertical") * speed + directIntentX * Input.GetAxis("Horizontal") * speed + Vector3.up * playerBody.velocity.y;
            playerBody.velocity = Vector3.ClampMagnitude(playerBody.velocity, maxspeed);
        }
    }

    void ExtraGrav()
    {
        //decides extra gravity basedon if youre wall running
        if (isWallRunning)
        {
            xtraGrav = 10f;
            playerBody.AddForce(Vector3.down * xtraGrav, ForceMode.Acceleration);
        }
        else if (!isWallRunning)
        {
            xtraGrav = 45f;
            playerBody.AddForce(Vector3.down * xtraGrav, ForceMode.Acceleration);
        }
    }
    

    void Jump()
    {
        //applies jump force
        if(isGrounded )
        {
            if((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("joystick button 1"))){
                playerBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
                isGrounded = false;
            }
        }
    }

    //Wallrunning
   

    private void WallRunInput() //make sure to call in void Update
    {
        //Wallrun
        if ((Input.GetKey(KeyCode.D) || Input.GetAxis("move 1") > .5) && isWallRight) StartWallrun();
        
        if ((Input.GetKey(KeyCode.A) || Input.GetAxis("move 1") < -.5) && isWallLeft) StartWallrun();
    }

    private void StartWallrun()
    {
        playerBody.useGravity = false;
        isWallRunning = true;
        extraJump = true;

        if (playerBody.velocity.magnitude <= maxWallSpeed)
        {
            playerBody.AddForce(orientation.forward * wallrunForce * Time.deltaTime);

            //Make sure char sticks to wall
            if (isWallRight)
                playerBody.AddForce(orientation.right * wallrunForce / 5 * Time.deltaTime);
            else
                playerBody.AddForce(-orientation.right * wallrunForce / 5 * Time.deltaTime);
        }
    }

    private void StopWallRun()
    {
        isWallRunning = false;
        playerBody.useGravity = true;
        extraJump = false;
    }
    
    private void CheckForWall() //make sure to call in void Update
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1.5f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1.5f, whatIsWall);
        isWallForward = Physics.Raycast(transform.position, orientation.forward, 1f, whatIsWall);

        //leave wall run
        if (!isWallLeft && !isWallRight) StopWallRun();

    }

    private void ExtraJump()
    {
        /*
        if (extraJump && isWallRunning && Input.GetKeyDown(KeyCode.Space))
        {
            playerBody.AddForce(new Vector3(0, jumpForce / 2, -jumpForce / 2), ForceMode.VelocityChange);
        }
        */

        if (isWallRight &&  (Input.GetKey(KeyCode.A) || Input.GetAxis("move 1") < -.2))
        {
            playerBody.AddForce(-orientation.right * jumpForce * 3.2f);
            playerBody.AddForce(orientation.up * jumpForce);
            isWallRunning = false;
        }
        if (isWallLeft &&  (Input.GetKey(KeyCode.D) || Input.GetAxis("move 1") > .2))
        {
            playerBody.AddForce(orientation.right * jumpForce * 3.2f);
            playerBody.AddForce(orientation.up * jumpForce);
            isWallRunning = false;
        }
    }

    private void WallClimb()
    { 
        playerBody.AddForce(orientation.up * jumpForce / 2f);
        playerBody.AddForce(orientation.forward * wallrunForce / 5 * Time.deltaTime);
        playerBody.useGravity = false;
        
    }
}
