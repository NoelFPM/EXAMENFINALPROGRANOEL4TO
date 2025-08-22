using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField]
    private GameObject prefob;

    private bool check = false;
    private GameObject colObj;

    private void Update()
    {
        if(check)
        {
            colObj.GetComponent<VidaEnemigo>().DanioEnemigo(1);
            check = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    Instantiate(prefob, transform.position, transform.rotation);
        //}

        if(collision.gameObject.CompareTag("Enemy"))
        {
            colObj = collision.gameObject;
            check = true;  
        }
    }
}
