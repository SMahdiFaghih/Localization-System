using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CSVLoader
{
    private TextAsset CSVFile;
    private readonly char LineSeparator = '\n';
    private readonly string[] FieldSeperator = { "\",\"" };

    private string[] Lines;
    private string[][] LineFields;

    public void LoadCSV()
    {
        CSVFile = Resources.Load<TextAsset>("Localization/Localization");
        SeparateFields();
    }

    private void SeparateFields()
    {
        Lines = CSVFile.text.Split(LineSeparator);

        LineFields = new string[Lines.Length][];

        for (int i = 0; i < Lines.Length; i++)
        {
            string line = Lines[i].Trim().Trim('"');

            LineFields[i] = line.Split(FieldSeperator, StringSplitOptions.None);
        }
    }

    public Dictionary<string, string> GetDictionaryValues(string languageName)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        //Find language index in CSV file
        int languageIndex = -1;
        string[] headers = LineFields[0];
        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Contains(languageName))
            {
                languageIndex = i;
                break;
            }

        }

        foreach (string[] fields in LineFields)
        {
            if (fields.Length > languageIndex)
            {
                var key = fields[0];

                if (dictionary.ContainsKey(key))
                {
                    continue;
                }

                var value = fields[languageIndex];

                dictionary.Add(key, value);
            }
        }

        return dictionary;
    }

#if UNITY_EDITOR
    public void Add(string key, string[] values)
    {
        List<string> newElements = new List<string>();
        newElements.Add(key);
        for (int i = 0; i < values.Length; i++)
        {
            newElements.Add(values[i]);
        }

        string appended = string.Join("\",\"", newElements);
        File.AppendAllText("Assets/Resources/Localization/Localization.csv", "\n\"" + appended + "\"");

        UnityEditor.AssetDatabase.Refresh();
    }

    public void Remove(string key)
    {
        string[] lines = CSVFile.text.Split(LineSeparator);

        string[] keys = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            keys[i] = line.Split(FieldSeperator, StringSplitOptions.None)[0].Replace("\"", "");
        }

        int index = -1;

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == key)
            {
                index = i;
                break;
            }
        }

        if (index > -1)
        {
            string[] newLines;
            newLines = lines.Where(w => w != lines[index]).ToArray();

            string replaced = string.Join(LineSeparator.ToString(), newLines);
            File.WriteAllText("Assets/Resources/Localization/Localization.csv", replaced);
        }
    }

    public void Edit(string key, string[] values)
    {
        Remove(key);
        Add(key, values);
    }

#endif
}
