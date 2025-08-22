using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SistemaGuardado
{
    private static string savePath = Application.persistentDataPath + "/saves/";

    public static void GuardarPartida(string nombreArchivo)
    {
        // Crear directorio si no existe
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // SOLUCIÓN: Quitar .fun si ya viene incluido
        string nombreSinExtension = nombreArchivo.Replace(".fun", "");
        string path = savePath + nombreSinExtension + ".fun";

        // Primero asegurarnos de que los datos están actualizados
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GuardadDatos();
        }

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            PerfilJugador perfil = new PerfilJugador();
            formatter.Serialize(stream, perfil);
        }

        Debug.Log("Partida guardada: " + path);
    }

    public static PerfilJugador CargarPartida(string nombreArchivo)
    {
        // SOLUCIÓN: Quitar .fun si ya viene incluido
        string nombreSinExtension = nombreArchivo.Replace(".fun", "");
        string path = savePath + nombreSinExtension + ".fun";

        if (File.Exists(path))
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                PerfilJugador perfil = formatter.Deserialize(stream) as PerfilJugador;
                return perfil;
            }
        }
        else
        {
            Debug.LogWarning("No se encontró archivo: " + path);
            return null;
        }
    }
}