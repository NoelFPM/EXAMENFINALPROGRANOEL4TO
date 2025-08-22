using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController controller;

    private float movX;
    private float movZ;

    [SerializeField]
    [Range(0f, 100)]
    private float movSpeed;

    [SerializeField]
    [Range(0f, 100)]
    private float jumpSpeed;

    [SerializeField]
    private Vector3 VelocitY;

    [SerializeField]
    private float gravedad = -9.8f;

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float radio;
    [SerializeField]
    private LayerMask whatIsGround;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(2);
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, radio, whatIsGround);

        VelocitY.y += gravedad * Time.deltaTime;

        if(isGrounded && VelocitY.y <= 0)
        {
            VelocitY.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            AudioManager.Instance.Play("Jump");
            VelocitY.y = Mathf.Sqrt(jumpSpeed * gravedad * -2);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) )
        {
            VelocitY.y = -1 * (Mathf.Sqrt(jumpSpeed * gravedad * -2));
        }

            controller.Move(VelocitY * Time.deltaTime);


        movX = Input.GetAxis("Horizontal") * movSpeed * Time.deltaTime;
        movZ = Input.GetAxis("Vertical") * movSpeed * Time.deltaTime;

        // Es un vector que toma en cuenta la posicion local de el objeto
        Vector3 movimiento = transform.right * movX + transform.forward * movZ;

        // Es un vector que toma en cuenta la posicion global de el objeto
        //Vector3 movimiento = new(movX,0,movZ);

        controller.Move(movimiento);
    }
}
