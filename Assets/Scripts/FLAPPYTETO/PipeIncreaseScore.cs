using UnityEngine;

public class PipeIncreaseScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (Score.instance != null)
            {
                Score.instance.UpdateScore();
            }
            else
            {
                Debug.LogError("Null.");
            }
        }
    }
}