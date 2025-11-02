using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

// Object Pooling
public class Spawn : MonoBehaviour
{
    // --- Variables de Configuración y Pool ---

    // Puntos donde pueden spawnear objetos // Estos si ya son parte de la escena
    [SerializeField] Transform[] spawnPoints;

    // El objeto a spawnear
    [SerializeField] private GameObject objectToSpawn;

    // La tasa de tiempo entre cada spawn
    [SerializeField] private float spawnRate;

    // El tamaño de mi piscina. La cantidad de objetos de los que puedo disponer
    [SerializeField] private int poolSize;

    // La cantidad maxima de objetos que pueden estar activos en la escena
    [SerializeField] private int maxObjectsInScene;

    // Cuantos objetos ya estan activos // 0
    [SerializeField] private int activeObjects = 0;

    // Esta variable va a almacenar todos nuestros objetos para disponer, es decir, va a ser nuestra piscina/pool
    private Queue<GameObject> pool;

    // --- Método Start ---

    private void Start()
    {
        // Inicialización de la cola
        pool = new Queue<GameObject>();

        // Este for instancia todos los objetos que tendremos en la piscina, y los desactiva
        for (int i = 0; i < poolSize; i++)
        {
            // Instancia el objeto sin decirle dónde (para agregarlo a la piscina)
            GameObject instance = Instantiate(objectToSpawn);

            // Desactiva el objeto
            instance.SetActive(false);

            // Agrega a la cola/pool // En espera
            pool.Enqueue(instance);
        }

        // Inicia el ciclo de aparición de objetos
        StartCoroutine(SpawnObjects());
    }

    // --- Métodos de Control ---

    // Coroutine para aparecer objetos periódicamente
    private IEnumerator SpawnObjects()
    {
        // Bucle que spawnea objetos hasta alcanzar el límite
        // for(int i = activeObjects; activeObjects < maxObjectsInScene; i++) // Spawneo ID
        while (activeObjects < maxObjectsInScene)
        {
            // Espera el tiempo definido por spawnRate
            yield return new WaitForSeconds(spawnRate);

            // Saca el objeto de la piscina
            GameObject objeto = pool.Dequeue(); // Aquí guardo temporalmente el objeto que saque de mi piscina

            // Asigna una posición de aparición aleatoria
            objeto.transform.position = GetRandomSpawn().position;

            // Activa el objeto
            objeto.SetActive(true);

            // Incrementa el contador de objetos activos
            activeObjects++;

            // Inicia una coroutine para devolver el objeto a la cola después de un tiempo
            StartCoroutine(BackToQueue(objeto));
        }
    }

    // Coroutine para devolver un objeto a la cola
    private IEnumerator BackToQueue(GameObject objeto)
    {
        // Espera 2 segundos antes de devolverlo (simulando vida útil)
        yield return new WaitForSeconds(2f);

        // Desactiva el objeto
        objeto.SetActive(false);

        // Devuelve el objeto a la piscina
        pool.Enqueue(objeto);

        // Decrementa el contador de objetos activos
        activeObjects--;
    }

    // Obtiene una posición de spawn aleatoria
    private Transform GetRandomSpawn()
    {
        // Me consigue un indice de spawn random
        int randomSpawn = Random.Range(0, spawnPoints.Length);

        // Regresa un spawn usando el índice random
        return spawnPoints[randomSpawn];
    }
}