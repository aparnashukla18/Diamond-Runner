using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultipler;
    bool readyToJump;

    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;

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

    void Awake()
    {
        
    }
    void Start()
    {
        playerrb = GetComponent<Rigidbody>();
        playerrb.freezeRotation = true;
    }
    void Update()
    {
        // Ground Check

        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        MyInput();
        SpeedControl();

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
        if(Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    void playerMovement()
    {
        movementDirection = Orientation.forward * verticalInput + Orientation.right * horizontalInput;

        //playerrb.AddForce(movementDirection.normalized * moveSpeed * 10f , ForceMode.Force);

        // On ground
        if(isGrounded)
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
}
