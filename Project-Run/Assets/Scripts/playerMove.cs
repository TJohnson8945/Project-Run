using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public Transform camera;
    public Rigidbody playerBody;
    public GameObject body;

    public float camRotSpeed = 5f;
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

    public LayerMask whatIsWall;
    public float wallrunForce, maxWallrunTime, maxWallSpeed;
    public bool isWallRight, isWallLeft;
    public bool isWallRunning;
    public float maxWallRunCameraTilt, wallRunCameraTilt;
    public Transform orientation;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    void FixedUpdate()
    {
        ExtraGrav();
    }

    void Camera()
    {
        //Get cam and body rotation
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
            wallRunCameraTilt += Time.deltaTime * maxWallRunCameraTilt * 2;
        if (Math.Abs(wallRunCameraTilt) < maxWallRunCameraTilt && isWallRunning && isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;

        //Tilts camera back again
        if (wallRunCameraTilt > 0 && !isWallRight && !isWallLeft)
            wallRunCameraTilt -= Time.deltaTime * maxWallRunCameraTilt * 2;
        if (wallRunCameraTilt < 0 && !isWallRight && !isWallLeft)
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
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = runSpeed;
            isSprinting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = walkSpeed;
            isSprinting = false;
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
            playerBody.velocity = directIntentY * Input.GetAxis("Vertical") * speed + directIntentX * Input.GetAxis("Horizontal") * speed + Vector3.up * playerBody.velocity.y;
            playerBody.velocity = Vector3.ClampMagnitude(playerBody.velocity, maxspeed);
        }
    }

    void ExtraGrav()
    {
        if (isWallRunning)
        {
            xtraGrav = 10f;
            playerBody.AddForce(Vector3.down * xtraGrav, ForceMode.Acceleration);
        }
        else if (!isWallRunning)
        {
            playerBody.AddForce(Vector3.down * xtraGrav, ForceMode.Acceleration);
        }
    }
    

    void Jump()
    {
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
        }
    }

    //Wallrunning
   

    private void WallRunInput() //make sure to call in void Update
    {
        //Wallrun
        if (Input.GetKey(KeyCode.D) && isWallRight) StartWallrun();
        if (Input.GetKey(KeyCode.A) && isWallLeft) StartWallrun();
    }
    private void StartWallrun()
    {
        playerBody.useGravity = false;
        isWallRunning = true;

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
    }
    private void CheckForWall() //make sure to call in void Update
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);

        //leave wall run
        if (!isWallLeft && !isWallRight) StopWallRun();

    }
      
}
