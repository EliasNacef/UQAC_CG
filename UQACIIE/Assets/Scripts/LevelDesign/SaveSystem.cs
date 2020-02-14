using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static void SaveLevel(Map map, string saveName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveName + ".uqac";

        FileStream stream = new FileStream(path, FileMode.Create);
        LevelData data = new LevelData(map, map.idealNumberOfTraps);

        formatter.Serialize(stream, data);
        stream.Close();
    }


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
            Debug.Log("Save file non trouve dans le path : " + path1 + "ou" + path2);
            return null;
        }
    }

}
