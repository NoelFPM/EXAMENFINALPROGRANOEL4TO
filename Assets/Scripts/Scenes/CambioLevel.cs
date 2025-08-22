using UnityEngine;

public class CambioLevel : MonoBehaviour
{
    [SerializeField] private string nivel;
    [SerializeField] private bool AreaEspecifica;
    [SerializeField] private string nombreArea;
        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ManejoEscena.Instance.CambioEscena(nivel, AreaEspecifica, nombreArea);
        }
    }
}
