using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Controll")]
    public float Speed;
    public float jumpForce;
    public float Gravity;


    [Header("Ground Setting")]
    public Transform GroundCheck;
    public float groundDistace;
    public LayerMask groundMask;


    [Header("Boolen")]
    bool isGrounded;


    [Header("Refrences")]
    public Rigidbody playerrb;


    void Awake()
    {
        playerrb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        playerrb = GetComponent<Rigidbody>(); 
    }
    void Update()
    {
       
        isGrounded = Physics.CheckSphere(
            GroundCheck.position,
            groundDistace,
            groundMask
        );

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerrb.linearVelocity = new Vector3(
                playerrb.linearVelocity.x,
                jumpForce,
                playerrb.linearVelocity.z
            );
        }
    }

    void FixedUpdate()
    {
        PlayerControll();
    }

   void PlayerControll()
   {
        

        float moveH = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        playerrb.linearVelocity = new Vector3 (moveH * Speed,0, moveZ*Speed) ;


        


       
   } 
}
