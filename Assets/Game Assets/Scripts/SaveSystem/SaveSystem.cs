using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveData(PlayerDataSO playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.dataPath + "/SAVE.DAT";
        FileStream stream = new FileStream(path, FileMode.Create);

        FileData data = new FileData();

        data.Copy(playerData);

        Debug.Log("NANI");

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void LoadData(PlayerDataSO playerData)
    {
        string path = Application.dataPath + "/SAVE.DAT";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            FileData data = formatter.Deserialize(stream) as FileData;
            stream.Close();

            // when the static method is called, we will load the necessary data as well as store the data to reference if need be.
            playerData.Copy(data);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }
}