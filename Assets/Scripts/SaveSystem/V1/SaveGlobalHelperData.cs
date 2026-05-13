using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveGlobalHelperData
{
    public static void SaveHelperData(GlobalHelper GH)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gHelper.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        GlobalHelperData data = new GlobalHelperData(GH);

        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log(path);
    }

    public static GlobalHelperData LoadHelper()
    {
        string path = Application.persistentDataPath + "/gHelper.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GlobalHelperData data = formatter.Deserialize(stream) as GlobalHelperData;
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
