using UnityEngine;
using System.IO;

public static class SavingSystem
{
    public static void SaveProgress(ScoreData data, string path)
    {
        string jsonString = JsonUtility.ToJson(data);
        Debug.Log(jsonString);
        using (StreamWriter streamWriter = File.CreateText(path))
        {
            streamWriter.Write(jsonString);
        }
    }

    public static ScoreData LoadProgress(ScoreData data,string path)
    {
        using (StreamReader streamReader = File.OpenText(path))
        {
            string jsonString = streamReader.ReadToEnd();
            return JsonUtility.FromJson<ScoreData>(jsonString);
        } 
    }
}