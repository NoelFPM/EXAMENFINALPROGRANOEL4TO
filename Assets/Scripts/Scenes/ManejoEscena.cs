using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManejoEscena : MonoBehaviour
{
    public static ManejoEscena Instance;

    private string areaVoy;
    private string nivelVoy;
    private bool AreaEspecifica;

    private Animator anim;

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
        anim = transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
    }

    public void CambioEscena(string nivel, bool especifica, string area)
    {
        nivelVoy = nivel;
        areaVoy = area;
        AreaEspecifica = especifica;

        StartCoroutine(Waiter());
    }


        IEnumerator Waiter()
        {
        GameManager.Instance.active = false;
        anim.SetBool("Active", true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nivelVoy);
        yield return new WaitForSeconds(1);
        if (AreaEspecifica)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            player.GetComponent<CharacterController>().enabled = false;
            Transform spawnPoint = GameObject.Find(areaVoy).transform;
            player.position = spawnPoint.position;
            player.rotation = spawnPoint.rotation;
            player.GetComponent<CharacterController>().enabled = true;
        }
        anim.SetBool("Active", false);
        GameManager.Instance.active = true;


    }
}

