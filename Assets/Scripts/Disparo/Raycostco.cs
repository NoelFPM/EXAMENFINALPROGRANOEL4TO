using UnityEngine;
using System.Collections.Generic;

public class Raycostco : MonoBehaviour
{
    [SerializeField]
    private int danio;
    [SerializeField]
    private int fuerza;
    [SerializeField]
    private int cantidad;

    [SerializeField]
    private GameObject bala;
    [SerializeField]
    private Transform shooter;
    [SerializeField]
    private float fuerzaBakla;

    private GameObject objeto;
    public List<GameObject> chofer = new();

    private Transform sootPoint;
    private RaycastHit hit;

    [SerializeField]
    private LayerMask enemyMask;

    private void Awake()
    {
        sootPoint = transform.parent;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Physics.Raycast(sootPoint.position, sootPoint.forward, out hit, 100, enemyMask);

            if (hit.transform != null)
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.CompareTag("Objeto"))
                {
                    objeto = hit.transform.gameObject;
                    Destroy(objeto);
                    for (int i = 0; i < cantidad; i++)
                    {
                        Instantiate(objeto, objeto.transform.position, objeto.transform.rotation);
                    }
                }

                if (hit.transform.CompareTag("Chofer"))
                {
                    chofer.Add(hit.transform.gameObject);
                    chofer[^1].SetActive(false);
                }

                if (hit.transform.CompareTag("Enemy"))
                {
                    hit.rigidbody.AddForceAtPosition(sootPoint.forward * fuerza, hit.point);

                    hit.transform.GetComponent<VidaEnemigo>().DanioEnemigo(danio);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            
            if (chofer.Count > 0)
            {
                Physics.Raycast(sootPoint.position, sootPoint.forward, out hit, 100, enemyMask);
                if (hit.transform != null)
                {
                    GameObject clone = Instantiate(chofer[^1], hit.point, chofer[^1].transform.rotation);
                    clone.SetActive(true);
                    Destroy(chofer[^1]);
                    chofer.Remove(chofer[^1]);
                }
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            AudioManager.Instance.Play("Shoot");
            Transform clone = Instantiate(bala, shooter.position, shooter.rotation).transform;
            Transform clone1 = Instantiate(bala, shooter.position, shooter.rotation).transform;
            Transform clone2 = Instantiate(bala, shooter.position, shooter.rotation).transform;
            Transform clone3 = Instantiate(bala, shooter.position, shooter.rotation).transform;
            clone.GetComponent<Rigidbody>().AddForce(transform.forward * fuerzaBakla);
            clone1.GetComponent<Rigidbody>().AddForce(transform.forward * fuerzaBakla);
            clone2.GetComponent<Rigidbody>().AddForce(transform.forward * fuerzaBakla);
            clone3.GetComponent<Rigidbody>().AddForce(transform.forward * fuerzaBakla);
            Destroy(clone.gameObject, 10);
            Destroy(clone1.gameObject, 10);
            Destroy(clone2.gameObject, 10);
            Destroy(clone3.gameObject, 10);
        }

    }
}
