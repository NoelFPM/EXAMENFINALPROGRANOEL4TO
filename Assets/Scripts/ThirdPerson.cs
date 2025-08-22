using System.Collections;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    private float movX;
    private float movY;

    [SerializeField]
    [Range(0f, 100)]
    private float jumpSpeed;

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

    private Animator anim;

    private bool golpeando;

    [SerializeField]
    private Transform cam;

    [SerializeField]
    private float velocidadActual;

    [SerializeField]
    private float velocidadMax;

    [SerializeField]
    private float velocidadNor;

    private CharacterController charCtrl;

    [SerializeField]
    private float smoothing = 0.1f;
    private float smoothVel;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        velocidadActual = velocidadNor;
        anim = transform.GetChild(0).GetComponent<Animator>();
        charCtrl = GetComponent<CharacterController>();
    }
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, radio, whatIsGround);
        anim.SetBool("IsGrounded", isGrounded);

        VelocitY.y += gravedad * Time.deltaTime;

        if (isGrounded && VelocitY.y <= 0)
        {
            VelocitY.y = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !golpeando && GameManager.Instance.active)
        {
            VelocitY.y = Mathf.Sqrt(jumpSpeed * gravedad * -2);
        }

        charCtrl.Move(VelocitY * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, radio, whatIsGround);
        movX = Input.GetAxis("Horizontal");
        movY = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movX, 0, movY).normalized;

        if (movimiento.magnitude > .1 && !golpeando && GameManager.Instance.active)
        {
            anim.SetBool("IsMoving", true);
            float targetAngle = Mathf.Atan2(movimiento.x, movimiento.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVel, smoothing);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 dirMov = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            charCtrl.Move(dirMov.normalized * velocidadActual * Time.deltaTime);



        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            velocidadActual = velocidadMax;
            anim.SetBool("IsRun", true);
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            velocidadActual = velocidadNor;
            anim.SetBool("IsRun", false);
        }

        if (Input.GetMouseButtonDown(0) && !golpeando && isGrounded && GameManager.Instance.active)
        {
            anim.SetTrigger("Attack");
            StartCoroutine(Esperar());
        }
    }
    IEnumerator Esperar()
    {
        golpeando = true;
        yield return new WaitForSeconds(.8f);
        golpeando = false;
    }
}

