using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static void SaveLevel(LevelDesignManager levelDesignManager, string saveName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + saveName + ".uqac";

        FileStream stream = new FileStream(path, FileMode.Create);
        LevelData data = new LevelData(levelDesignManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static LevelData LoadLevel(string saveName)
    {
        string path = Application.persistentDataPath + "/" + saveName + ".uqac";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Save file non trouve dans le path : " + path);
            return null;
        }
    }

}
