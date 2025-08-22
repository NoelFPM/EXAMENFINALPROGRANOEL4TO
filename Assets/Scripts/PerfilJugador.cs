using UnityEngine;

[System.Serializable]
public class PerfilJugador
{
    public int colect;
    public int dinero;
    public int enemigosRestantes;
    public float posX;
    public float posY;
    public float posZ;
    public string nivel;

    public PerfilJugador()
    {
        // SOLUCIÓN: Verificar que las instancias existan antes de usarlas
        if (GameManager.Instance != null)
        {
            colect = GameManager.Instance.collect;
            dinero = GameManager.Instance.dinero;
            nivel = GameManager.Instance.nivel;
            posX = GameManager.Instance.posX;
            posY = GameManager.Instance.posY;
            posZ = GameManager.Instance.posZ;
        }

        // SOLUCIÓN: Verificar si EnemyCounter existe
        if (EnemyCounter.Instance != null)
        {
            enemigosRestantes = EnemyCounter.Instance.totalEnemies;
        }
        else
        {
            enemigosRestantes = 20; // Valor por defecto
        }
    }
}