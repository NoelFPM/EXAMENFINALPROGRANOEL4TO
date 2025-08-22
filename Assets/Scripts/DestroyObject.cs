using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField]
    private int vida;

    [SerializeField]
    private GameObject obj;

    [SerializeField]
    private int[] dropLimit;

    public void Danio(int danio)
    {
        int rand=
        Random.Range(1, 4);
        Instantiate(obj, transform.position, Quaternion.identity);
        vida -= danio;

        if(vida <=0)
        {
            Instantiate(obj,transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
