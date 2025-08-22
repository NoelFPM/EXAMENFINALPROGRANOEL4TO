using UnityEngine;

public class Espada : MonoBehaviour
{
    [SerializeField]
    private int danio;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.GetComponent<DestroyObject>() != null)
        {
            other.GetComponent<DestroyObject>().Danio(danio);
        }
    }
}
