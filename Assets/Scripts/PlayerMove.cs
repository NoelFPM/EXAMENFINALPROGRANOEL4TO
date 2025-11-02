using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;

    [Header("Configuración de Velocidad")]
    [SerializeField]
    [Range(1f, 20f)]
    private float moveSpeed = 5f; 

    [SerializeField]
    [Range(1f, 20f)]
    private float sprintSpeed = 10f; 

    [SerializeField]
    [Range(1f, 10f)]
    private float jumpHeight = 2.5f;

    [SerializeField]
    private float gravity = -9.81f;

    [Header("Chequeo de Suelo")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;

    private bool isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
       
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;


        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }


        
        float currentSpeed = moveSpeed;

        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

     
        Vector3 moveDirection = transform.right * x + transform.forward * z;

      
        moveDirection *= currentSpeed;


       
        Vector3 finalMovement = moveDirection;
        finalMovement.y = velocity.y;

       
        controller.Move(finalMovement * Time.deltaTime);
    }
}