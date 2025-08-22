using UnityEngine;

[System.Serializable]
public class Dialogos
{
    public string nombre;
    [TextArea(3,5)]
    public string texto;

    public Sprite personaje1;
    public Sprite personaje2;
    public Sprite box;
}
