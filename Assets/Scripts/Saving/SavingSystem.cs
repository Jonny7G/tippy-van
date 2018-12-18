using UnityEngine;
using System.IO;

public static class SavingSystem
{
    public static void SaveProgress<T>(T t, string path)
    {
        string jsonString = JsonUtility.ToJson(t);
        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
    }

    public static void LoadProgress<T>(out T t,string path)
    {
        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            t = JsonUtility.FromJson<T>(jsonString);
        } 
    }
}