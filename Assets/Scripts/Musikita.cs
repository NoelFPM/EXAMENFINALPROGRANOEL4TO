using UnityEngine;

public class Musikita : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.Play("Happy");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
