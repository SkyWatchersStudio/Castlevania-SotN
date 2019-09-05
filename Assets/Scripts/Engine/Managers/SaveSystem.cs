﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string m_Path = Application.persistentDataPath + "/save.bin";

    public static void SaveState(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(m_Path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveData LoadState()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(m_Path, FileMode.Open);

        SaveData data = formatter.Deserialize(stream) as SaveData;
        stream.Close();

        return data;
    }
}