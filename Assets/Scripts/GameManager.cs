using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int dinero;
    public bool active = true;
    public int collect;

    public float posX;
    public float posY;
    public float posZ;

    private Transform player;

    public string nivel;
    public string nombreGuardado;

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
    }

    public void GuardadDatos()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        posX = player.position.x;
        posY = player.position.y;
        posZ = player.position.z;
         
        nivel = SceneManager.GetActiveScene().name;
    }

    public IEnumerator MoverJugador()
    {
        yield return new WaitForSeconds(0.1f); // espera breve para que cargue la escena
        player = GameObject.FindGameObjectWithTag("Player").transform;
        CharacterController ctrl = player.GetComponent<CharacterController>();
        ctrl.enabled = false;
        player.transform.position = new Vector3(posX, posY, posZ);
        ctrl.enabled = true;
    }
}
