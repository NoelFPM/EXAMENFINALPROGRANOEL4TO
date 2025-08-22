using UnityEngine;

public class TitleSong : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.Play("Title");
    }
}
