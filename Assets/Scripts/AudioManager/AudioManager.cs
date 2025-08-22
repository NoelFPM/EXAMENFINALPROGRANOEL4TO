using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sonido[] sonidos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach (Sonido s in sonidos)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public void Play(string nombre)
    {
        foreach (Sonido s in sonidos)
        {
            if (s.nombre == nombre)
            {
                s.source.Play();
                return;
            }
            Debug.Log("esa no la manejamos pa " + nombre);
        }
    }

    public void Stop(string nombre)
    {
        foreach (Sonido s in sonidos)
        {
            if (s.nombre == nombre)
            {
                s.source.Stop();
                return;
            }
            Debug.Log("esa no la manejamos pa " + nombre);
        }
    }
}
