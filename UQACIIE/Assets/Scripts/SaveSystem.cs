using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    public static void SaveLevel(MapManager mapManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/level.cg";

        FileStream stream = new FileStream(path, FileMode.Create);
        LevelData data = new LevelData(mapManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static LevelData LoadLevel()
    {
        string path = Application.persistentDataPath + "/level.cg";
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
