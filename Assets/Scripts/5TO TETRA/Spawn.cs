using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private int maxObjectsInScene = 10;
    [SerializeField] private int initialActiveObjects = 3;

    private Queue<GameObject> pool;
    private int activeObjects = 0;
    private Coroutine spawnCoroutine;

    private void Start()
    {
        InitializePool();

        // activa enemigos iniciales
        for (int i = 0; i < initialActiveObjects && pool.Count > 0; i++)
        {
            SpawnEnemy();
        }

        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void InitializePool()
    {
        pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject instance = Instantiate(objectToSpawn);
            instance.SetActive(false);

            
            Shuerk enemyScript = instance.GetComponent<Shuerk>();
            if (enemyScript != null)
            {
              
            }

            pool.Enqueue(instance);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            if (activeObjects < maxObjectsInScene && pool.Count > 0)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        if (pool.Count == 0) return;

        GameObject enemy = pool.Dequeue();
        enemy.transform.position = GetRandomSpawn().position;
        enemy.SetActive(true);
        activeObjects++;
    }

    public void ReturnToPool(GameObject enemy)
    {
        if (enemy != null)
        {
            enemy.SetActive(false);
            pool.Enqueue(enemy);
            activeObjects--;
        }
    }

    private Transform GetRandomSpawn()
    {
        if (spawnPoints.Length == 0)
            return transform;

        int randomSpawn = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomSpawn];
    }

    private void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }

    // obtener la cantidad de objetos activos 
    public int GetActiveObjectsCount()
    {
        return activeObjects;
    }
}