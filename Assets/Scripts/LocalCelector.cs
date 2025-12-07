using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LocalCelector : MonoBehaviour
{
    private bool active = false;
   
    private string[] languageCodes = { "en", "ru" };
    
    public void ChangeLocale(int localeID)
    {
        if (active == true)
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        active = false;
        
        // Сохраняем выбранный язык
        if (_localeID >= 0 && _localeID < languageCodes.Length)
        {
            string langCode = languageCodes[_localeID];
            SaveLanguage(langCode);
        }
    }
    
    /// <summary>
    /// Сохраняет выбранный язык через GameManager
    /// </summary>
    private void SaveLanguage(string langCode)
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.ChangeLanguage(langCode);
        }
        else
        {
            // Если GameManager не найден, сохраняем напрямую
            SaveData saveData = SaveSystem.Load();
            saveData.selectedLanguage = langCode;
            SaveSystem.Save(saveData);
            Debug.Log("Язык сохранен: " + langCode);
        }
    }
}
