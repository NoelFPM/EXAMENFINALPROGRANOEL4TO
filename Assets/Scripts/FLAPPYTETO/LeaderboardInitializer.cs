using UnityEngine;

public class LeaderboardInitializer : MonoBehaviour
{
    [SerializeField] private Transform contentParent; // Asigna el Content aquí

    private void OnEnable()
    {
        // 1. Reasignar la referencia al PlayfabManager persistente
        if (PlayfabManager.instance != null && contentParent != null)
        {
            PlayfabManager.instance.SetLeaderboardParent(contentParent);
            // 2. Opcional: Llamar a GetLeaderboard aquí si el panel se activa
            PlayfabManager.instance.GetLeaderboard();
        }
    }
}