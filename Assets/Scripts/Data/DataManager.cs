using UnityEngine;
using System.IO;
using Sirenix.Serialization;

public class DataManager : MonoBehaviour
{
    /// <summary>
    /// Creates a file with saved data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">name of the save file</param>
    /// <param name="data">Data to store, this could be anything from simple types to entire classes</param>
    public static void Save<T>(string fileName, T data) where T : class
    {
        byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
        File.WriteAllBytes($"{Application.persistentDataPath}/{fileName}", bytes);
    }

    /// <summary>
    /// Loads a save file
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName">name of the save file to load</param>
    /// <param name="dataObject">If savefile does not exist create one with the object passed in this argument</param>
    /// <returns></returns>
    public static T Load<T>(string fileName, T dataObject) where T : class
    {
        //If file doesn't exist, create a new one and then load it
        if (!File.Exists($"{Application.persistentDataPath}/{fileName}"))
        {
            Save<T>(fileName, dataObject);
            byte[] bytes = File.ReadAllBytes($"{Application.persistentDataPath}/{fileName}");
            T data = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
            return data;
        }
        else //otherwise just load
        {
            byte[] bytes = File.ReadAllBytes($"{Application.persistentDataPath}/{fileName}");
            T data = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
            return data;
        }
    }
}