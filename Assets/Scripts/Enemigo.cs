using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform prota;
    private bool detect;

    [Header("Detección")]
    [SerializeField] private float radio = 5f;
    [SerializeField] private LayerMask mask;

    [Header("Patrulla por Waypoints")]
    [SerializeField] private Transform[] posiZion;
    private int cordenada = 0;

    [Header("Patrulla Aleatoria")]
    [SerializeField] private bool usarMerodeo = false; // si es true, patrulla aleatoria
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTimer = 5f;
    private float timer;

    private bool musicStart;

    private void Start()
    {
        prota = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    private void Update()
    {
        // Detecta si el jugador está cerca
        detect = Physics.CheckSphere(transform.position, radio, mask);

        if (detect) //  Si ve al jugador → lo persigue
        {
            if (!musicStart)
            {
                AudioManager.Instance.Play("Candy");
                AudioManager.Instance.Stop("Happy");
                musicStart = true;
            }

            agent.SetDestination(prota.position);
            agent.stoppingDistance = 3;
        }
        else //  Si NO ve al jugador → patrulla
        {
            if (musicStart)
            {
                AudioManager.Instance.Stop("Candy");
                AudioManager.Instance.Play("Happy");
                musicStart = false;
            }

            if (usarMerodeo)
            {
                // --- Modo Aleatorio ---
                timer += Time.deltaTime;
                if (timer >= wanderTimer)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                    agent.SetDestination(newPos);
                    agent.stoppingDistance = 0;
                    timer = 0;
                }
            }
            else
            {
                // --- Modo Waypoints ---
                if (Vector3.Distance(transform.position, posiZion[cordenada].position) < 1)
                {
                    cordenada++;
                    if (cordenada >= posiZion.Length) cordenada = 0;
                }
                agent.SetDestination(posiZion[cordenada].position);
                agent.stoppingDistance = 0;
            }
        }
    }

    // Para generar posiciones aleatorias en el NavMesh
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMesh.SamplePosition(randDirection, out NavMeshHit navHit, dist, layermask);
        return navHit.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}
