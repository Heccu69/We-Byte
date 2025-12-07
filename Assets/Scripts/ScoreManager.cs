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
        UpdateScoreUI();
    }
    
    /// <summary>
    /// Добавить очки
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();
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
    /// Обновить UI
    /// </summary>
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Очки: {currentScore}";
        }
    }
}
