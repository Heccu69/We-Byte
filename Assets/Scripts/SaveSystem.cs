using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveSystem
{
    private const string SAVE_FOLDER = "Saves";
    private const int MAX_SAVE_SLOTS = 3; // Максимум 3 слота сохранений

    /// <summary>
    /// Получить путь к папке сохранений
    /// </summary>
    private static string GetSaveFolderPath()
    {
        string path = Path.Combine(Application.persistentDataPath, SAVE_FOLDER);
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    /// <summary>
    /// Получить путь к файлу сохранения по индексу слота
    /// </summary>
    private static string GetSaveFilePath(int slotIndex)
    {
        return Path.Combine(GetSaveFolderPath(), $"save_slot_{slotIndex}.json");
    }

    /// <summary>
    /// Сохранить данные в указанный слот
    /// </summary>
    public static void SaveToSlot(SaveData data, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS)
        {
            Debug.LogError($"Неверный индекс слота: {slotIndex}. Допустимо: 0-{MAX_SAVE_SLOTS - 1}");
            return;
        }

        data.slotIndex = slotIndex;
        data.UpdateSaveTime();

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        string path = GetSaveFilePath(slotIndex);
        File.WriteAllText(path, json);
        Debug.Log($"Сохранено в слот {slotIndex}: {path}");
    }

    /// <summary>
    /// Загрузить данные из указанного слота
    /// </summary>
    public static SaveData LoadFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS)
        {
            Debug.LogError($"Неверный индекс слота: {slotIndex}");
            return null;
        }

        string path = GetSaveFilePath(slotIndex);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
            Debug.Log($"Загружено из слота {slotIndex}");
            return data;
        }
        else
        {
            Debug.LogWarning($"Слот {slotIndex} пуст");
            return null;
        }
    }

    /// <summary>
    /// Проверить, существует ли сохранение в слоте
    /// </summary>
    public static bool SlotExists(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS)
            return false;

        return File.Exists(GetSaveFilePath(slotIndex));
    }

    /// <summary>
    /// Удалить сохранение из слота
    /// </summary>
    public static void DeleteSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= MAX_SAVE_SLOTS)
        {
            Debug.LogError($"Неверный индекс слота: {slotIndex}");
            return;
        }

        string path = GetSaveFilePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Слот {slotIndex} удален");
        }
    }

    /// <summary>
    /// Получить информацию о всех слотах
    /// </summary>
    public static SaveData[] GetAllSlots()
    {
        SaveData[] slots = new SaveData[MAX_SAVE_SLOTS];
        for (int i = 0; i < MAX_SAVE_SLOTS; i++)
        {
            slots[i] = LoadFromSlot(i);
        }
        return slots;
    }

    /// <summary>
    /// Создать новое сохранение в первом свободном слоте
    /// </summary>
    public static int CreateNewSave(string saveName = "Новое сохранение")
    {
        for (int i = 0; i < MAX_SAVE_SLOTS; i++)
        {
            if (!SlotExists(i))
            {
                SaveData newSave = new SaveData();
                newSave.slotName = saveName;
                newSave.slotIndex = i;
                SaveToSlot(newSave, i);
                return i;
            }
        }
        Debug.LogWarning("Все слоты заняты!");
        return -1;
    }

    // ===== СТАРЫЕ МЕТОДЫ ДЛЯ СОВМЕСТИМОСТИ =====

    /// <summary>
    /// Сохранение (старый метод, использует слот 0)
    /// </summary>
    public static void Save(SaveData data)
    {
        SaveToSlot(data, 0);
    }

    /// <summary>
    /// Загрузка (старый метод, использует слот 0)
    /// </summary>
    public static SaveData Load()
    {
        SaveData data = LoadFromSlot(0);
        if (data == null)
        {
            data = new SaveData();
            SaveToSlot(data, 0);
        }
        return data;
    }
}
