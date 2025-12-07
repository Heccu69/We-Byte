using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveSystem
{
    private const string SAVE_FILE = "game_save.json";

    // Сохранение данных
    public static void Save(SaveData data)
    {
        string json = JsonConvert.SerializeObject(data);
        string path = Application.persistentDataPath + "/" + SAVE_FILE;
        File.WriteAllText(path, json);
        Debug.Log("Сохранено: " + path);
    }

    // Загрузка данных
    public static SaveData Load()
    {
        string path = Application.persistentDataPath + "/" + SAVE_FILE;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден. Создан новый.");
            return new SaveData();
        }
    }
}
