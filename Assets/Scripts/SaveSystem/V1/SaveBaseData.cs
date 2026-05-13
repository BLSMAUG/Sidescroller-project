using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveBaseData
{
    public static void SavePlayerBaseData(Unit player)
    {
        Debug.Log("b");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.basestats";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerBaseData data = new PlayerBaseData(player);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log(path);
        Debug.Log("c");
    }

    public static PlayerBaseData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.basestats";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerBaseData data = formatter.Deserialize(stream) as PlayerBaseData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in" + path);
            return null;
        }
    }

}