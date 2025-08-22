using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DangerPP : MonoBehaviour
{
    [Header("Referencias Post Processing")]
    public PostProcessVolume volume;
    private Vignette vignette;

    [Header("Configuración Viñeta")]
    public float distanciaPeligro = 5f;
    public Transform player;
    public string tagEnemigo = "Enemy"; // etiqueta de tus enemigos

    void Start()
    {
        volume.profile.TryGetSettings(out vignette);
    }

    void Update()
    {
        float distanciaMin = float.MaxValue;

        // Buscamos todos los enemigos activos por tag
        GameObject[] enemigos = GameObject.FindGameObjectsWithTag(tagEnemigo);

        foreach (GameObject enemigo in enemigos)
        {
            float dist = Vector3.Distance(player.position, enemigo.transform.position);
            if (dist < distanciaMin) distanciaMin = dist;
        }

        // Viñeta roja según proximidad
        if (distanciaMin <= distanciaPeligro)
        {
            vignette.color.value = Color.red;
            vignette.intensity.value = Mathf.Lerp(0, 0.5f, 1 - (distanciaMin / distanciaPeligro));
        }
        else
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, Time.deltaTime * 2);
        }
    }
}
