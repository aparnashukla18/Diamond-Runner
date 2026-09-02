using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultipler;
    public float sprintSpeed;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Slope")]
    public float maxSlopeAngle;
    public RaycastHit slopeHit;

    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundMask;
    bool isGrounded;

    public Transform Orientation;

    public float horizontalInput;
    public float verticalInput;

    Vector3 movementDirection;

    public Rigidbody playerrb;

    public MovementState state; // always store current state of player

    public enum MovementState
    {
        walking,
        crouching,
        sprinting,
        air
    }

    void Awake()
    {
        
    }
    void Start()
    {
        playerrb = GetComponent<Rigidbody>();
        playerrb.freezeRotation = true;
        readyToJump = true;

        startYScale = transform.localScale.y;
    }
    void Update()
    {
        // Ground Check

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1f);

        MyInput();
        SpeedControl();
        stateHandler();

        // Handle drag
        if (isGrounded)
        {
            playerrb.linearDamping = groundDrag;
        }
        else
        {
            playerrb.linearDamping = 0;
        }
    }
    void FixedUpdate()
    {
        playerMovement();
    }
    void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // When Jump
        if(Input.GetKeyDown(jumpKey) && readyToJump &&isGrounded )
        {
            Debug.Log("SPACE PRESSED");

            readyToJump = false;
            Debug.Log("Ready To Jump: " + readyToJump);

            Jump();
           
            Debug.Log("Is Grounded: " + isGrounded);


            Invoke(nameof(ResetJump), jumpCooldown);
        }
        // When Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            playerrb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }
    void stateHandler()
    {
        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }
    void playerMovement()
    {
        movementDirection = Orientation.forward * verticalInput + Orientation.right * horizontalInput;

        //On slope
        if(OnSlope() )
        {
            playerrb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);
            if(playerrb.linearVelocity.y > 0)
            {
                playerrb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        playerrb.useGravity = !OnSlope();

        //playerrb.AddForce(movementDirection.normalized * moveSpeed * 10f , ForceMode.Force);

        // On ground
        if (isGrounded)
        {
            playerrb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // In air
        else if(!isGrounded)
        {
            playerrb.AddForce(movementDirection.normalized * moveSpeed * 10f * airMultipler, ForceMode.Force);
        }
            

        
    }
    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerrb.linearVelocity.x, 0, playerrb.linearVelocity.z);

        // Limit Velocity if needed
        if(flatVel.magnitude > moveSpeed)
        {
           Vector3 limitedVel =  flatVel.normalized * moveSpeed;
            playerrb.linearVelocity = new Vector3(limitedVel.x, playerrb.linearVelocity.y,limitedVel.z);
        }
    }
    void Jump()
    {
        // Reaset y velocity 
        playerrb.linearVelocity = new Vector3(playerrb.linearVelocity.x, 0, playerrb.linearVelocity.z);

        playerrb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
       
    }
    void ResetJump()
    {
        readyToJump = true;
    }
    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(movementDirection, slopeHit.normal).normalized;
    }
}
