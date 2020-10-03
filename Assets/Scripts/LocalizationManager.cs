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

    public static string GetLocalizedValue(string key)
    {
        if (!IsInit)
        {
            Init();
        }

        string value = "Not Found!";
        switch (CurrentLanguage)
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
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].Contains("\""))
            {
                values[i].Replace('"', '\"');
            }
        }

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
        for (int i=0;i < values.Length;i++)
        {
            if (values[i].Contains("\""))
            {
                values[i].Replace('"', '\"');
            }
        }

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

    public void ChangeLanguage()
    {
        CurrentLanguage = (CurrentLanguage == LocalizedLanguage.Farsi) ? LocalizedLanguage.English : LocalizedLanguage.Farsi;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
