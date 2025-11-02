using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuerk : MonoBehaviour
{
    // Variables Serializadas
    [SerializeField] private float health;
    [SerializeField] private float shield;
    [SerializeField] private bool isDead;

    // Referencia al Coroutine
    Coroutine patroll;

    // Método Start
    private void Start()
    {
        patroll = StartCoroutine(Patroll());
    }

    // Método OnEnable (se llama al habilitar el objeto/componente)
    private void OnEnable()
    {
        if (patroll != null)
        {
            StopCoroutine(patroll);
            patroll = StartCoroutine(Patroll());
        }
        else
        {
            patroll = StartCoroutine(Patroll());
        }
    }

    // Método OnDisable (se llama al deshabilitar/destruir el objeto/componente)
    private void OnDisable()
    {
        // Reseteo de variables
        health = 100;
        shield = 200;
        isDead = false;

        // Detener el coroutine
        StopCoroutine(patroll);
        patroll = null;
    }

    // El Coroutine de Patrullaje
    private IEnumerator Patroll()
    {
        yield return null;
        Debug.Log("Patrullando");
    }
}