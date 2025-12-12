using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Shuerk : MonoBehaviour
{
    [Header("Estadísticas")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float shield = 200f;
    [SerializeField] private bool isDead = false;

    [Header("Movimiento")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float patrolWaitTime = 3f;
    [SerializeField] private Transform[] patrolPoints;

    private NavMeshAgent agent;
    private Coroutine patrolCoroutine;
    private int currentPatrolIndex = 0;
    private Transform player;
    private Spawn spawnManager;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        spawnManager = FindAnyObjectByType<Spawn>();
    }

    private void Start()
    {
        patrolCoroutine = StartCoroutine(Patrol());
    }

    private void OnEnable()
    {
        isDead = false;
        health = 100f;
        shield = 200f;

        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
        }
        patrolCoroutine = StartCoroutine(Patrol());
    }

    private void OnDisable()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }
    }

    private void Update()
    {
        if (isDead || player == null) return;

        // detecta al jugador
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 10f)
        {
            ChasePlayer();
        }
        else if (distanceToPlayer > 15f)
        {
            if (patrolCoroutine == null)
                patrolCoroutine = StartCoroutine(Patrol());
        }
    }

    private IEnumerator Patrol()
    {
        if (patrolPoints.Length == 0) yield break;

        agent.speed = patrolSpeed;

        while (!isDead && patrolPoints.Length > 0)
        {
            //ir al punto de patrulla actual
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);

            //esperar hasta llegar
            while (!isDead && (agent.pathPending || agent.remainingDistance > agent.stoppingDistance))
            {
                yield return null;
            }

            if (!isDead)
            {
                yield return new WaitForSeconds(patrolWaitTime);

                // siguiente punto de patrulla
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    private void ChasePlayer()
    {
        if (patrolCoroutine != null)
        {
            StopCoroutine(patrolCoroutine);
            patrolCoroutine = null;
        }

        agent.speed = chaseSpeed;
        if (player != null)
            agent.SetDestination(player.position);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        // escudo
        if (shield > 0)
        {
            shield -= damage;
            if (shield < 0)
            {
                // si el escudo se rompe, el exceso de daño se resta a la salud
                health += shield; 
                shield = 0;
            }
        }
        else
        {
            health -= damage;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        agent.isStopped = true;

        //notifica al gamenanager que un enemigo fue derrotado
       
        //GameManager gameManager = FindFirstObjectByType<GameManager>();
        //if (gameManager != null)
        //{
        //    gameManager.EnemyDefeated();
        //}

        // devolver al pool usando el Spawn manager
        if (spawnManager != null)
        {
            spawnManager.ReturnToPool(gameObject);
        }
        else
        {
            //desactivar el objeto
            gameObject.SetActive(false);
        }
    }
}