using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;

    private static Dictionary<string, string> LocalizedFarsiTexts;
    private static Dictionary<string, string> LocalizedEnglishTexts;

    private static LocalizedLanguage CurrentLanguage = LocalizedLanguage.Farsi;
    //private readonly LocalizedLanguage DefaultLanguage = LocalizedLanguage.Farsi;

    public static bool IsInit;

    public static CSVLoader csvLoader;

    public enum LocalizedLanguage
    {
        English,
        Farsi
    };

    public static void Init()
    {
        csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        UpdateDictionaries();

        IsInit = true;
    }

    public static Dictionary<string, string> GetDictionaryForEditor()
    {
        if (!IsInit)
        {
            Init();
        }

        return LocalizedEnglishTexts;
    }

    public static void UpdateDictionaries()
    {
        LocalizedFarsiTexts = csvLoader.GetDictionaryValues("farsi");
        LocalizedEnglishTexts = csvLoader.GetDictionaryValues("english");
    }

    public static string GetLocalizedValue(string key, LocalizedLanguage language)
    {
        if (!IsInit)
        {
            Init();
        }

        string value = "";
        switch (language)
        {
            case LocalizedLanguage.Farsi:
                LocalizedFarsiTexts.TryGetValue(key, out value);
                break;
            case LocalizedLanguage.English:
                LocalizedEnglishTexts.TryGetValue(key, out value);
                break;
        }
        return value;
    }

    public static void Add(string key, string[] values)
    {
        CheckValues(ref values);

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Add(key, values);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Replace(string key, string[] values)
    {
        CheckValues(ref values);

        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Edit(key, values);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    public static void Remove(string key)
    {
        if (csvLoader == null)
        {
            csvLoader = new CSVLoader();
        }

        csvLoader.LoadCSV();
        csvLoader.Remove(key);
        csvLoader.LoadCSV();

        UpdateDictionaries();
    }

    private static void CheckValues(ref string[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] == null)
            {
                continue;
            }
            if (values[i].Contains("\""))
            {
                values[i].Replace('"', '\"');
            }
            values[i] = values[i].TrimStart();
            values[i] = values[i].TrimEnd();
        }
    }

    public void ChangeLanguage()
    {
        CurrentLanguage = (CurrentLanguage == LocalizedLanguage.Farsi) ? LocalizedLanguage.English : LocalizedLanguage.Farsi;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static LocalizedLanguage GetCurrentLanguage()
    {
        return CurrentLanguage;
    }
}
