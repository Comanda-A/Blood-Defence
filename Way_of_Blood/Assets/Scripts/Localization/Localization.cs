using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Localization", menuName = "Localization/LocalizationData", order = 1)]
public class Localization : ScriptableObject
{
    public string Language; // �������� ����� (��������, "English", "Russian")
    public string LanguageCode; // ��� ����� (��������, "en", "ru")
    public string LanguagePath; // ���� � ������ �������� (��������, "Localization/en")

    public List<LocalizationEntry> Translations = new List<LocalizationEntry>();

    // ����������� ������ ���� �����������
    private static List<Localization> _allLocalizations = new List<Localization>();

    // �������������� ����������� ��� ��������
    private void OnEnable()
    {
        if (!_allLocalizations.Contains(this))
        {
            _allLocalizations.Add(this);
        }
    }

    // �������������� ������ ����������� ��� ��������
    private void OnDisable()
    {
        if (_allLocalizations.Contains(this))
        {
            _allLocalizations.Remove(this);
        }
    }

    // ����� ��� ��������� �������� �� �����
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
        return key; // ���������� ����, ���� ������� �� ������
    }

    // ����������� ����� ��� ��������� ���� �����������
    public static List<Localization> GetAllLocalizations()
    {
        return _allLocalizations;
    }
}

[System.Serializable]
public class LocalizationEntry
{
    public string Key; // ���������� ����
    public string Value; // �������
}