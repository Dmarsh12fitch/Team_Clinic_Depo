using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController playerController;

    private Vector3 velocity;

    private float gravity = -16f;
    public float currentSpeed = 8f;
    public float normalSpeed = 8f;
    public float topSpeed = 14f;
    public float jumpHeight = 1.5f;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    bool isOnGround;
    bool isSprinting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float AorD = Input.GetAxis("Horizontal");
        float WorS = Input.GetAxis("Vertical");

        Vector3 move = transform.right * AorD + transform.forward * WorS;

        playerController.Move(move * currentSpeed * Time.deltaTime);

        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isOnGround && velocity.y < 0)
        {
            velocity.y = -3f;
        }

        if (Input.GetButton("Jump") && isOnGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            Debug.Log("JumpGoddamn!");
        }

        if(Input.GetKey("left shift") && isOnGround && !isSprinting)
        {
            currentSpeed = topSpeed;
        } else {
            currentSpeed = normalSpeed;
        }

        velocity.y += gravity * Time.deltaTime;

        playerController.Move(velocity * Time.deltaTime);
    }
}
