using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Configuracin de Disparo")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 100f;
    [SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

    private void Start()
    {
  
        if (playerCamera == null)
            playerCamera = Camera.main;

       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

     
        if (Input.GetKeyDown(shootKey) && playerCamera != null)
        {
            Shoot();

           
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            // Verifica si golpeamos un enemigo
            Shuerk enemy = hit.transform.GetComponent<Shuerk>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}