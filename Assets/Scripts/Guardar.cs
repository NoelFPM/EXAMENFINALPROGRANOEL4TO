using UnityEngine;

public class Guardar : MonoBehaviour
{
    public string nombreArchivo = "slot1"; // Puedes cambiar esto por slot2, slot3, etc.

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GuardarAhora();
        }
    }

    public void GuardarAhora()
    {
        SistemaGuardado.GuardarPartida(nombreArchivo);
        Debug.Log("Guardado: " + nombreArchivo);
    }

    // Para usar desde botones del UI
    public void GuardarEnSlot(string slotNombre)
    {
        nombreArchivo = slotNombre;
        GuardarAhora();
    }
}