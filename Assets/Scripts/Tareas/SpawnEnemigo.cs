using UnityEngine;

public class SpawnEnemigo : MonoBehaviour
{
    [SerializeField] private GameObject enemigo;

    [SerializeField] private Transform spawn1, spawn2, spawn3;

    private float timer1 = 2f;
    private float timer2 = 2f;
    private float timer3 = 2f;

    private void Update()
    {
        timer1 -= Time.deltaTime;
        timer2 -= Time.deltaTime;
        timer3 -= Time.deltaTime;

        if (timer1 <= 0f)
        {
            Instantiate(enemigo, spawn1.position, enemigo.transform.rotation);
            timer1 = 2f;
        }

        if (timer2 <= 0f)
        {
            Instantiate(enemigo, spawn2.position, enemigo.transform.rotation);
            timer2 = 2f;
        }

        if (timer3 <= 0f)
        {
            Instantiate(enemigo, spawn3.position, enemigo.transform.rotation);
            timer3 = 2f;
        }
    }
}
