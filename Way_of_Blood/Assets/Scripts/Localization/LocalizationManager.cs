using System.Collections.Generic;
using UnityEngine;

public static class LocalizationManager
{
    private static Localization _currentLocalization;

    // ������������� ������� ����������� �� ���� �����
    public static void SetLanguage(string languageCode)
    {
        var allLocalizations = Localization.GetAllLocalizations();
        foreach (var localization in allLocalizations)
        {
            if (localization.LanguageCode == languageCode)
            {
                _currentLocalization = localization;
                Debug.Log($"Language set to: {localization.Language}");
                return;
            }
        }
        Debug.LogError($"Localization not found for language code: {languageCode}");
    }

    // �������� ������� �� �����
    public static string GetTranslation(string key)
    {
        if (_currentLocalization != null)
        {
            return _currentLocalization.GetTranslation(key);
        }
        else
        {
            Debug.LogError("Localization is not set!");
            return key; // ���������� ����, ���� ����������� �� �����������
        }
    }

    // �������� ������ ���� ��������� ������
    public static List<Localization> GetAvailableLanguages()
    {
        return Localization.GetAllLocalizations();
    }
}