using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    public CharacterController controller;

    private float x;
    private float z;
    public float speed = 12f;
    public float grav = -9.81f;
    public float jumpHeight = 3f;

    public bool isSprinting;
    public bool isCrouching;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(isGrounded)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 19f;
            isSprinting = true;
        }
        
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 12f;
            isSprinting = false;
        }

        if(Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = 1.5f;
            isCrouching = true;
            speed = 8f;
        }
        
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            controller.height = 3f;
            isCrouching = false;
            speed = 12f;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);


        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * grav);
        }

        velocity.y += grav * Time.deltaTime;

        if(velocity.y < -200f)
        {
            velocity.y = -200f;
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
