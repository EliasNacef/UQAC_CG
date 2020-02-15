using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Classe permttant de gérer le systeme de sauvegarde.
/// </summary>
public static class SaveSystem
{

    /// <summary>
    /// Permet de creer un fichier de sauvegarde de la map de jeu
    /// </summary>
    /// <param name="map"></param>
    /// <param name="saveName"></param>
    public static void SaveLevel(Map map, string saveName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveName + ".uqac";

        FileStream stream = new FileStream(path, FileMode.Create);
        LevelData data = new LevelData(map, map.idealNumberOfTraps);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    /// <summary>
    /// Permet de charger un niveau.
    /// </summary>
    /// <param name="saveName"> Nom de la sauvegarde </param>
    /// <returns> Renvoie la sauvegarde qui a ete chargee </returns>
    public static LevelData LoadLevel(string saveName)
    {
        string path1 = Application.persistentDataPath + "/" + saveName;
        string path2 = Application.streamingAssetsPath + "/Levels/" + saveName;
        if (File.Exists(path2))
        {
            Debug.Log("Loading of : " + path2);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path2, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        }
        else if (File.Exists(path1))
        {
            Debug.Log("Loading of : " + path1);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path1, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Fichier non trouve dans le chemin : " + path1 + "ou" + path2);
            return null;
        }
    }

}
