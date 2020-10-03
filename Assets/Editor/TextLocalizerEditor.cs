using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextLocalizerEditWindow : EditorWindow
{
    public static void Open(string key)
    {
        TextLocalizerEditWindow window = new TextLocalizerEditWindow();
        window.titleContent = new GUIContent("Localizer Window");
        window.ShowUtility();
        window.key = key;
    }

    public string key;
    public string[] values = new string[System.Enum.GetNames(typeof(LocalizationManager.LocalizedLanguage)).Length];

    public void OnGUI()
    {
        key = EditorGUILayout.TextField("Key: ", key);

        EditorGUILayout.BeginVertical();

        for (int i=0;i < values.Length;i++)
        {
            EditorGUILayout.LabelField(System.Enum.GetName(typeof(LocalizationManager.LocalizedLanguage), i) + "Value: ", GUILayout.MaxWidth(100));

            EditorStyles.textArea.wordWrap = true;
            values[i] = EditorGUILayout.TextArea(values[i], EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
        }

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Add"))
        {
            if (LocalizationManager.GetLocalizedValue(key) != string.Empty)
            {
                LocalizationManager.Replace(key, values);
            }
            else
            {
                LocalizationManager.Add(key, values);
            }
        }

        minSize = new Vector2(460, 500);
        maxSize = minSize;
    }
}


public class TextLocalizerSearchWindow :EditorWindow
{
    public static void Open()
    {
        TextLocalizerSearchWindow window = new TextLocalizerSearchWindow();
        window.titleContent = new GUIContent("Localization Search");

        Vector2 mouse = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Rect r = new Rect(mouse.x - 450, mouse.y + 10, 10, 10);
        window.ShowAsDropDown(r, new Vector2(500, 300));
    }

    public string value;
    public Vector2 Scroll;
    public Dictionary<string, string> dictionary;

    private void OnEnable()
    {
        dictionary = LocalizationManager.GetDictionaryForEditor();
    }

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("Box");
        EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
        value = EditorGUILayout.TextField(value);
        EditorGUILayout.EndHorizontal();

        GetSearchResults();
    }

    private void GetSearchResults()
    {
        if (value == null)
        {
            return;
        }

        EditorGUILayout.BeginVertical();
        Scroll = EditorGUILayout.BeginScrollView(Scroll);
        foreach (KeyValuePair<string ,string> element in dictionary)
        {
            if (element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
            {
                EditorGUILayout.BeginHorizontal("box");
                Texture deleteIcon = (Texture)Resources.Load("delete");
                GUIContent deleteContent = new GUIContent(deleteIcon);

                if (GUILayout.Button(deleteContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from Localization, Are you sure?", "Do it"))
                    {
                        LocalizationManager.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalizationManager.Init();
                        dictionary = LocalizationManager.GetDictionaryForEditor();
                    }
                }

                EditorGUILayout.TextField(element.Key);
                EditorGUILayout.LabelField(element.Value);
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}