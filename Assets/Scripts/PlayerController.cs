using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera = null;

    [SerializeField] AnimationCurve jumpFallOff;

    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float gravity = -15f;
    [SerializeField] float jumpMultiplier = 10f;

    [SerializeField] [Range(0f, 0.5f)] float mouseSmoothTime = 0.04f;
    [SerializeField] [Range(0f, 0.5f)] float moveSmoothTime = 0.2f;

    [SerializeField] bool lockCursor = true;
    [SerializeField] bool isJumping = false;

    CharacterController controller = null;



    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    Vector2 currentDirection = Vector2.zero;
    Vector2 currentDirectionVelocity = Vector2.zero;


    float velovityY = 0f;
    float cameraPitch = 0f;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
    }

    void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

    }


    void UpdateMovement()
    {
        Vector2 targetDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDirection.Normalize();

        currentDirection = Vector2.SmoothDamp(currentDirection, targetDirection, ref currentDirectionVelocity, moveSmoothTime);

        if (controller.isGrounded)
        {
            velovityY = 0f;
        }

        velovityY += gravity * Time.deltaTime;

        Vector3 velocity = (transform.forward * currentDirection.y + transform.right * currentDirection.x) * walkSpeed + Vector3.up * velovityY;

        //Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }

        controller.Move(velocity * Time.deltaTime);

    }




    private IEnumerator JumpEvent()
    {
        float slopeLimitBackup = controller.slopeLimit;
        controller.slopeLimit = 90f;
        float timeInAir = 0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

        controller.slopeLimit = slopeLimitBackup;
        isJumping = false;
    }

}
