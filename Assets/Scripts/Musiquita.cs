using UnityEngine;

public class Musiquita : MonoBehaviour
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
