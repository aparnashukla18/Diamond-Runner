using UnityEngine;

public class Sliding : MonoBehaviour
{
   public Transform orientation;
   public Transform playerObj;
   private Rigidbody rb;
   private PlayerController playerController;

   public float slideForce;
   public float maxslideSpeed;
   public float slideTime;
    public float maxSlideTime;

    public float slideYscale;
    private float startYscale;

    KeyCode slideKey = KeyCode.LeftShift;
    private float horizontalInput;
    private float verticalInput;

    private bool sliding;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();

        startYscale = playerObj.localScale.y;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            startSliding();
        }
        if (Input.GetKeyUp(slideKey) && sliding)
        {
            stopSliding();
        }
    }
    void FixedUpdate()
    {
        if (sliding)
        {
            slidingMovement();
        }
    }
    public void startSliding()
    {
        sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYscale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTime = maxSlideTime;
    }
    void slidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        if(slideTime <= 0 )
        {
            stopSliding();
            
        }

    }
    void stopSliding()
    {
        sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYscale, playerObj.localScale.z);

    }
}
