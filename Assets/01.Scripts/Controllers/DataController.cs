using System;
using System.IO;
using UnityEngine;

public class DataController : MonoBehaviour
{
    private SaveData _saveData;
    private string _filePath;
    private string _jsonData;
    private void Awake()
    {
        _filePath = Application.persistentDataPath + "/SaveData.json";
    }

    public string Save(SaveData saveData)
    {
        _saveData = saveData;

        string jsonData = JsonUtility.ToJson(_saveData, true);
        _jsonData = jsonData;
        FileStream fs = File.Create(_filePath);
        StreamWriter sw = new StreamWriter(fs);
        sw.Write(jsonData);
        sw.Close();
        Debug.Log(_filePath);
        return jsonData;
        
    }

    public SaveData Load()
    {
        FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);
        SaveData saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
        sr.Close();

        _saveData = saveData;
        return saveData;
    }
}
