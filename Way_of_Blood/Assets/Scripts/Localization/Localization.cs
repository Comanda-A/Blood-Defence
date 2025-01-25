using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Localization", menuName = "Localization/LocalizationData", order = 1)]
public class Localization : ScriptableObject
{
    public string Language; // Название языка (например, "English", "Russian")
    public string LanguageCode; // Код языка (например, "en", "ru")
    public string LanguagePath; // Путь к файлам перевода (например, "Localization/en")

    public List<LocalizationEntry> Translations = new List<LocalizationEntry>();

    // Статический список всех локализаций
    private static List<Localization> _allLocalizations = new List<Localization>();

    // Автоматическая регистрация при создании
    private void OnEnable()
    {
        if (!_allLocalizations.Contains(this))
        {
            _allLocalizations.Add(this);
        }
    }

    // Автоматическая отмена регистрации при удалении
    private void OnDisable()
    {
        if (_allLocalizations.Contains(this))
        {
            _allLocalizations.Remove(this);
        }
    }

    // Метод для получения перевода по ключу
    public string GetTranslation(string key)
    {
        foreach (var entry in Translations)
        {
            if (entry.Key == key)
            {
                return entry.Value;
            }
        }
        Debug.LogWarning($"Translation not found for key: {key}");
        return key; // Возвращаем ключ, если перевод не найден
    }

    // Статический метод для получения всех локализаций
    public static List<Localization> GetAllLocalizations()
    {
        return _allLocalizations;
    }
}

[System.Serializable]
public class LocalizationEntry
{
    public string Key; // Уникальный ключ
    public string Value; // Перевод
}