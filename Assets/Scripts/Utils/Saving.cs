using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using Unity.VisualScripting;

public static class Saving
{
    static Saving()
    {
        _savesPath = Path.Combine(Application.persistentDataPath, "saves.dat");
    }

    private static Dictionary<string, byte[]> _savesData;
    private static string _savesPath;

    private static void LoadAll()
    {
        if (!File.Exists(_savesPath))
        {
            _savesData = new();
            return;
        }

        using var fs = new FileStream(_savesPath, FileMode.Open);

        BinaryFormatter formatter = new BinaryFormatter();

        _savesData = formatter.Deserialize(fs) as Dictionary<string, byte[]>;
    }

    public static void Set<T>(string key, T data)
    {
        if (ReferenceEquals(data, null)) return;

        if (_savesData is null)
            LoadAll();

        using var stream = new MemoryStream();
        IFormatter formatter = new BinaryFormatter();

        formatter.Serialize(stream, data);

        _savesData[key] = stream.ToArray();
    }

    public static void Remove(string key)
    {
        _savesData.Remove(key);
    }

    public static bool TryLoad<T>(string key, out T data)
    {
        if (_savesData is null)
            LoadAll();

        if (!_savesData.TryGetValue(key, out var bytes))
        {
            data = default;
            return false;
        }

        using var stream = new MemoryStream(bytes);
        IFormatter formatter = new BinaryFormatter();

        data = (T)formatter.Deserialize(stream);
        return true;
    }

    public static T Load<T>(string key, T defaultValue = default)
    {
        if (TryLoad(key, out T data))
        {
            return data;
        }

        return defaultValue;
    }

    public static void Save()
    {
        using FileStream fs = new FileStream(_savesPath, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();

        formatter.Serialize(fs, _savesData);
    }
}
