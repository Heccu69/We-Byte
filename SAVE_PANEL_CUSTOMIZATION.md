# 🎨 Кастомизация Панели Сохранений

## 📝 Примеры Кода для Расширения Функционала

### 1. Добавление Анимации Открытия Панели

Создайте новый скрипт `SavePanelAnimator.cs`:

```csharp
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SavePanelAnimator : MonoBehaviour
{
    [Header("Настройки Анимации")]
    public float fadeInDuration = 0.3f;
    public float scaleInDuration = 0.4f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    
    void Awake()
    {
        // Добавляем CanvasGroup если его нет
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        rectTransform = GetComponent<RectTransform>();
    }
    
    void OnEnable()
    {
        StartCoroutine(AnimateIn());
    }
    
    IEnumerator AnimateIn()
    {
        // Начальное состояние
        canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector3.zero;
        
        float elapsed = 0f;
        float maxDuration = Mathf.Max(fadeInDuration, scaleInDuration);
        
        while (elapsed < maxDuration)
        {
            elapsed += Time.deltaTime;
            
            // Fade In
            if (elapsed < fadeInDuration)
                canvasGroup.alpha = elapsed / fadeInDuration;
            else
                canvasGroup.alpha = 1f;
            
            // Scale In
            if (elapsed < scaleInDuration)
            {
                float t = elapsed / scaleInDuration;
                float scale = scaleCurve.Evaluate(t);
                rectTransform.localScale = Vector3.one * scale;
            }
            else
            {
                rectTransform.localScale = Vector3.one;
            }
            
            yield return null;
        }
        
        // Финальное состояние
        canvasGroup.alpha = 1f;
        rectTransform.localScale = Vector3.one;
    }
}
```

**Как использовать:**
1. Добавьте скрипт на `QuickSavePanel`
2. Настройте параметры в Inspector
3. Панель будет анимированно появляться при открытии

---

### 2. Форматирование Времени Игры

Добавьте в `SavePanelManager.cs` метод для красивого отображения времени:

```csharp
/// <summary>
/// Форматировать время игры в читаемый вид
/// </summary>
string FormatPlayTime(float seconds)
{
    int hours = Mathf.FloorToInt(seconds / 3600f);
    int minutes = Mathf.FloorToInt((seconds % 3600f) / 60f);
    int secs = Mathf.FloorToInt(seconds % 60f);
    
    if (hours > 0)
        return $"{hours}ч {minutes}м";
    else if (minutes > 0)
        return $"{minutes}м {secs}с";
    else
        return $"{secs}с";
}

// Используйте в UpdateUI():
void UpdateUI()
{
    if (lastSaveData != null)
    {
        // ... существующий код ...
        
        // Добавьте время игры
        if (lastSaveInfoText != null)
        {
            string info = $"<b>{lastSaveData.slotName}</b>\n";
            info += $"Уровень: {lastSaveData.currentLevel}\n";
            info += $"Счет: {lastSaveData.currentScore}\n";
            info += $"Лучший счет: {lastSaveData.highScore}\n";
            info += $"Время игры: {FormatPlayTime(lastSaveData.playTime)}";
            lastSaveInfoText.text = info;
        }
    }
}
```

---

### 3. Подтверждение Загрузки

Добавьте диалог подтверждения перед загрузкой:

```csharp
[Header("Диалог Подтверждения")]
public GameObject confirmationDialog;
public TextMeshProUGUI confirmationText;
public Button confirmYesButton;
public Button confirmNoButton;

void Start()
{
    // ... существующий код ...
    
    if (confirmYesButton != null)
        confirmYesButton.onClick.AddListener(ConfirmLoadLastSave);
    
    if (confirmNoButton != null)
        confirmNoButton.onClick.AddListener(CancelLoadLastSave);
    
    if (confirmationDialog != null)
        confirmationDialog.SetActive(false);
}

public void LoadLastSave()
{
    if (lastSaveData != null && lastSaveSlotIndex >= 0)
    {
        // Показываем диалог подтверждения
        if (confirmationDialog != null)
        {
            confirmationDialog.SetActive(true);
            if (confirmationText != null)
            {
                confirmationText.text = $"Загрузить сохранение\n\"{lastSaveData.slotName}\"?";
            }
        }
        else
        {
            // Если диалога нет, загружаем сразу
            ConfirmLoadLastSave();
        }
    }
}

void ConfirmLoadLastSave()
{
    if (confirmationDialog != null)
        confirmationDialog.SetActive(false);
    
    PlayerPrefs.SetInt("CurrentSaveSlot", lastSaveSlotIndex);
    PlayerPrefs.Save();
    
    Debug.Log($"Загружаем последнее сохранение из слота {lastSaveSlotIndex}");
    SceneManager.LoadScene(gameSceneName);
}

void CancelLoadLastSave()
{
    if (confirmationDialog != null)
        confirmationDialog.SetActive(false);
}
```

---

### 4. Отображение Статистики

Добавьте дополнительную статистику в панель:

```csharp
[Header("Дополнительная Статистика")]
public TextMeshProUGUI statsText;

void UpdateUI()
{
    if (lastSaveData != null)
    {
        // ... существующий код ...
        
        // Добавляем статистику
        if (statsText != null)
        {
            float accuracy = lastSaveData.totalCompletedOrders > 0 
                ? (float)lastSaveData.totalCorrectOrders / lastSaveData.totalCompletedOrders * 100f 
                : 0f;
            
            string stats = $"📊 Статистика:\n";
            stats += $"Заказов выполнено: {lastSaveData.totalCompletedOrders}\n";
            stats += $"Правильных: {lastSaveData.totalCorrectOrders}\n";
            stats += $"Точность: {accuracy:F1}%";
            
            statsText.text = stats;
        }
    }
}
```

---

### 5. Автоматическая Загрузка Последнего Сохранения

Добавьте опцию автозагрузки при старте:

```csharp
[Header("Автозагрузка")]
public bool autoLoadLastSave = false;
public float autoLoadDelay = 2f;

void Start()
{
    // ... существующий код ...
    
    if (autoLoadLastSave)
    {
        StartCoroutine(AutoLoadLastSaveCoroutine());
    }
}

IEnumerator AutoLoadLastSaveCoroutine()
{
    yield return new WaitForSeconds(autoLoadDelay);
    
    if (lastSaveData != null)
    {
        Debug.Log("Автозагрузка последнего сохранения...");
        LoadLastSave();
    }
}
```

---

### 6. Звуковые Эффекты

Добавьте звуки для кнопок:

```csharp
[Header("Звуки")]
public AudioClip buttonClickSound;
public AudioClip loadSaveSound;
public AudioClip errorSound;
private AudioSource audioSource;

void Awake()
{
    audioSource = gameObject.AddComponent<AudioSource>();
}

void PlaySound(AudioClip clip)
{
    if (clip != null && audioSource != null)
    {
        audioSource.PlayOneShot(clip);
    }
}

public void LoadLastSave()
{
    if (lastSaveData != null && lastSaveSlotIndex >= 0)
    {
        PlaySound(loadSaveSound);
        
        // ... остальной код загрузки ...
    }
    else
    {
        PlaySound(errorSound);
        Debug.LogWarning("Нет доступных сохранений");
    }
}
```

---

### 7. Индикатор Загрузки

Добавьте индикатор загрузки при переходе в игру:

```csharp
[Header("Индикатор Загрузки")]
public GameObject loadingIndicator;
public TextMeshProUGUI loadingText;

public void LoadLastSave()
{
    if (lastSaveData != null && lastSaveSlotIndex >= 0)
    {
        StartCoroutine(LoadWithIndicator());
    }
}

IEnumerator LoadWithIndicator()
{
    // Показываем индикатор
    if (loadingIndicator != null)
        loadingIndicator.SetActive(true);
    
    if (loadingText != null)
        loadingText.text = "Загрузка...";
    
    // Сохраняем индекс слота
    PlayerPrefs.SetInt("CurrentSaveSlot", lastSaveSlotIndex);
    PlayerPrefs.Save();
    
    // Небольшая задержка для показа индикатора
    yield return new WaitForSeconds(0.5f);
    
    // Загружаем сцену
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName);
    
    while (!asyncLoad.isDone)
    {
        if (loadingText != null)
        {
            float progress = asyncLoad.progress * 100f;
            loadingText.text = $"Загрузка... {progress:F0}%";
        }
        yield return null;
    }
}
```

---

### 8. Сравнение Сохранений

Добавьте возможность сравнить сохранения:

```csharp
public void CompareSaves()
{
    SaveData[] allSaves = SaveSystem.GetAllSlots();
    
    Debug.Log("=== Сравнение Сохранений ===");
    
    for (int i = 0; i < allSaves.Length; i++)
    {
        if (allSaves[i] != null)
        {
            Debug.Log($"Слот {i}: {allSaves[i].slotName}");
            Debug.Log($"  Уровень: {allSaves[i].currentLevel}");
            Debug.Log($"  Счет: {allSaves[i].currentScore}");
            Debug.Log($"  Время: {allSaves[i].saveDateTime}");
        }
    }
}
```

---

### 9. Резервное Копирование

Добавьте функцию создания резервной копии:

```csharp
public void CreateBackup()
{
    if (lastSaveData != null && lastSaveSlotIndex >= 0)
    {
        // Создаем копию в следующем свободном слоте
        SaveData[] allSaves = SaveSystem.GetAllSlots();
        
        for (int i = 0; i < allSaves.Length; i++)
        {
            if (allSaves[i] == null)
            {
                SaveData backup = new SaveData();
                // Копируем все данные
                backup.slotName = lastSaveData.slotName + " (Копия)";
                backup.currentLevel = lastSaveData.currentLevel;
                backup.currentScore = lastSaveData.currentScore;
                backup.highScore = lastSaveData.highScore;
                backup.totalCompletedOrders = lastSaveData.totalCompletedOrders;
                backup.totalCorrectOrders = lastSaveData.totalCorrectOrders;
                backup.playTime = lastSaveData.playTime;
                
                SaveSystem.SaveToSlot(backup, i);
                Debug.Log($"Резервная копия создана в слоте {i}");
                return;
            }
        }
        
        Debug.LogWarning("Нет свободных слотов для резервной копии");
    }
}
```

---

### 10. Фильтрация и Сортировка

Добавьте возможность сортировки сохранений:

```csharp
public enum SortType
{
    ByDate,
    ByLevel,
    ByScore
}

public SaveData[] GetSortedSaves(SortType sortType)
{
    SaveData[] allSaves = SaveSystem.GetAllSlots();
    List<SaveData> validSaves = new List<SaveData>();
    
    // Собираем только существующие сохранения
    foreach (var save in allSaves)
    {
        if (save != null)
            validSaves.Add(save);
    }
    
    // Сортируем
    switch (sortType)
    {
        case SortType.ByDate:
            validSaves.Sort((a, b) => string.Compare(b.saveDateTime, a.saveDateTime));
            break;
        case SortType.ByLevel:
            validSaves.Sort((a, b) => b.currentLevel.CompareTo(a.currentLevel));
            break;
        case SortType.ByScore:
            validSaves.Sort((a, b) => b.highScore.CompareTo(a.highScore));
            break;
    }
    
    return validSaves.ToArray();
}
```

---

## 🎨 Примеры Стилизации

### Градиентный Фон для Кнопок

```csharp
using UnityEngine.UI;

public class GradientButton : MonoBehaviour
{
    public Color topColor = new Color(0.3f, 0.8f, 0.3f);
    public Color bottomColor = new Color(0.2f, 0.6f, 0.2f);
    
    void Start()
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            // Создаем градиентную текстуру
            Texture2D gradientTexture = new Texture2D(1, 2);
            gradientTexture.SetPixel(0, 0, bottomColor);
            gradientTexture.SetPixel(0, 1, topColor);
            gradientTexture.Apply();
            
            Sprite gradientSprite = Sprite.Create(
                gradientTexture,
                new Rect(0, 0, 1, 2),
                new Vector2(0.5f, 0.5f)
            );
            
            image.sprite = gradientSprite;
        }
    }
}
```

---

## 🔧 Полезные Утилиты

### Форматирование Чисел

```csharp
public static class NumberFormatter
{
    public static string FormatScore(int score)
    {
        if (score >= 1000000)
            return $"{score / 1000000f:F1}M";
        else if (score >= 1000)
            return $"{score / 1000f:F1}K";
        else
            return score.ToString();
    }
}

// Использование:
string formattedScore = NumberFormatter.FormatScore(1250000); // "1.3M"
```

### Относительное Время

```csharp
public static string GetRelativeTime(string dateTimeString)
{
    try
    {
        DateTime saveTime = DateTime.ParseExact(dateTimeString, "dd.MM.yyyy HH:mm", null);
        TimeSpan difference = DateTime.Now - saveTime;
        
        if (difference.TotalMinutes < 1)
            return "только что";
        else if (difference.TotalMinutes < 60)
            return $"{(int)difference.TotalMinutes} мин. назад";
        else if (difference.TotalHours < 24)
            return $"{(int)difference.TotalHours} ч. назад";
        else if (difference.TotalDays < 7)
            return $"{(int)difference.TotalDays} дн. назад";
        else
            return dateTimeString;
    }
    catch
    {
        return dateTimeString;
    }
}
```

---

## 📝 Чек-лист Кастомизации

- [ ] Добавлена анимация открытия панели
- [ ] Настроено форматирование времени
- [ ] Добавлен диалог подтверждения
- [ ] Отображается дополнительная статистика
- [ ] Настроены звуковые эффекты
- [ ] Добавлен индикатор загрузки
- [ ] Реализована сортировка сохранений
- [ ] Добавлена функция резервного копирования

Выберите нужные функции и интегрируйте их в свой проект!
