using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Управление очками игрока
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    
    [Header("UI")]
    public TextMeshProUGUI scoreText; // TextMeshPro текст для отображения очков
    
    private int currentScore = 0;
    private int highScore = 0;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Загружаем сохраненный прогресс
        LoadProgress();
        UpdateScoreUI();
    }
    
    /// <summary>
    /// Добавить очки
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        
        // Обновляем лучший счет, если текущий больше
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
        
        UpdateScoreUI();
        SaveProgress(); // Автосохранение при изменении очков
        Debug.Log($"Очки добавлены! Текущий счет: {currentScore}");
    }
    
    /// <summary>
    /// Получить текущий счет
    /// </summary>
    public int GetScore()
    {
        return currentScore;
    }
    
    /// <summary>
    /// Получить лучший счет
    /// </summary>
    public int GetHighScore()
    {
        return highScore;
    }
    
    /// <summary>
    /// Установить счет (для загрузки из сохранения)
    /// </summary>
    public void SetScore(int score)
    {
        currentScore = score;
        UpdateScoreUI();
    }
    
    /// <summary>
    /// Установить лучший счет (для загрузки из сохранения)
    /// </summary>
    public void SetHighScore(int score)
    {
        highScore = score;
    }
    
    /// <summary>
    /// Сбросить текущий счет (для новой игры)
    /// </summary>
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
        SaveProgress();
    }
    
    /// <summary>
    /// Обновить UI
    /// </summary>
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Очки: {currentScore}";
        }
    }
    
    /// <summary>
    /// Сохранить прогресс
    /// </summary>
    private void SaveProgress()
    {
        SaveData saveData = SaveSystem.Load();
        saveData.currentScore = currentScore;
        saveData.highScore = highScore;
        SaveSystem.Save(saveData);
    }
    
    /// <summary>
    /// Загрузить прогресс
    /// </summary>
    private void LoadProgress()
    {
        SaveData saveData = SaveSystem.Load();
        highScore = saveData.highScore;
        // Загружаем текущий счет из сохранения
        currentScore = saveData.currentScore;
    }
}
