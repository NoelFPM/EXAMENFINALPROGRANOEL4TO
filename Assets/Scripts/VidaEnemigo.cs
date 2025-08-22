using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    [SerializeField]
    int vida;

    private bool estaMuerto = false;

    public void DanioEnemigo(int danio)
    {
        if (estaMuerto) return;

        vida -= danio;

        if (vida <= 0)
        {
            estaMuerto = true;
            // Llamar al EnemyCounter de forma segura
            if (EnemyCounter.Instance != null)
            {
                EnemyCounter.Instance.EnemyKilled();
            }
            Destroy(this.gameObject);
        }
    }
}