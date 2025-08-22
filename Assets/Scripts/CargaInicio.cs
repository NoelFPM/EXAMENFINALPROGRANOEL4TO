using UnityEngine;
using UnityEngine.SceneManagement;

public class CargaInicio : MonoBehaviour
{
    private PerfilJugador perfil;

    public void InicioJuego(string nombreGuardado)
    {
        // SOLUCI�N: Usar solo el nombre base, sin extensi�n
        GameManager.Instance.nombreGuardado = nombreGuardado.Replace(".fun", "");
        perfil = SistemaGuardado.CargarPartida(nombreGuardado);

        if (perfil != null)
        {
            // Cargar TODOS los datos en el GameManager
            GameManager.Instance.collect = perfil.colect;
            GameManager.Instance.dinero = perfil.dinero;
            GameManager.Instance.posX = perfil.posX;
            GameManager.Instance.posY = perfil.posY;
            GameManager.Instance.posZ = perfil.posZ;
            GameManager.Instance.nivel = perfil.nivel;

            // Cargar tambi�n los enemigos en el EnemyCounter
            if (EnemyCounter.Instance != null)
            {
                EnemyCounter.Instance.totalEnemies = perfil.enemigosRestantes;
            }

            // Cargar la escena
            SceneManager.LoadScene(GameManager.Instance.nivel);
        }
        else
        {
            // Si no hay guardado, cargar escena inicial
            SceneManager.LoadScene("MedioTermino");
        }
    }
}