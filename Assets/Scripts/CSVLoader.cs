using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CSVLoader
{
    private TextAsset CSVFile;
    private readonly char LineSeparator = '\n';
    private readonly string[] FieldSeperator = { "\",\"" };

    public void LoadCSV()
    {
        CSVFile = Resources.Load<TextAsset>("Localization");
    }

    public Dictionary<string, string> GetDictionaryValues(string attributeID)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        string[] lines = CSVFile.text.Split(LineSeparator);

        int attributeIndex = -1;

        string[] headers = lines[0].Split(FieldSeperator, StringSplitOptions.None);

        for (int i=0;i < headers.Length;i++)
        {
            if (headers[i].Contains(attributeID))
            {
                attributeIndex = i;
                break;
            }
        }

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int i=1;i < lines.Length;i++)
        {
            string line = lines[i].Trim(' ');

            string[] fields = CSVParser.Split(line);

            for (int j=0;j < fields.Length;j++)
            {
                fields[j] = fields[j].Replace("\"", "");
            }

            if (fields.Length > attributeIndex)
            {
                var key = fields[0];

                if (dictionary.ContainsKey(key))
                {
                    continue;
                }

                var value = fields[attributeIndex];

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
        for (int i=0;i < values.Length;i++)
        {
            newElements.Add(values[i]);
        }

        string appended = string.Join("\",\"", newElements);
        File.AppendAllText("Assets/Resources/Localization.csv", "\n\"" + appended + "\"");

        UnityEditor.AssetDatabase.Refresh();
    }

    public void Remove(string key)
    {
        string[] lines = CSVFile.text.Split(LineSeparator);

        string[] keys = new string[lines.Length];

        for (int i=0;i < lines.Length;i++)
        {
            string line = lines[i];

            keys[i] = line.Split(FieldSeperator, StringSplitOptions.None)[0];
        }

        int index = -1;

        for(int i = 0; i < keys.Length; i++)
        {
            if (keys[i].Contains(key))
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
            File.WriteAllText("Assets/Resources/Localization.csv", replaced);
        }
    }

    public void Edit(string key, string[] values)
    {
        Remove(key);
        Add(key, values);
    }
#endif

}
