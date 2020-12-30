using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextLocalizerEditWindow : EditorWindow
{
    public static string Key;
    public static string[] Values = new string[System.Enum.GetNames(typeof(LocalizationManager.LocalizedLanguage)).Length];
    public static bool IsOpen = false;

    public static void Open(string key)
    {
        TextLocalizerEditWindow window = CreateInstance<TextLocalizerEditWindow>();
        window.titleContent = new GUIContent("Localizer Window");
        window.ShowUtility();
        IsOpen = true;
        Key = key;

        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = LocalizationManager.GetLocalizedValue(key, (LocalizationManager.LocalizedLanguage)System.Enum.GetValues(typeof(LocalizationManager.LocalizedLanguage)).GetValue(i));
        }
    }

    public void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        Key = EditorGUILayout.TextField("Key: ", Key);
        if (EditorGUI.EndChangeCheck())
        {
            for (int i = 0; i < Values.Length; i++)
            {
                Values[i] = LocalizationManager.GetLocalizedValue(Key, (LocalizationManager.LocalizedLanguage)System.Enum.GetValues(typeof(LocalizationManager.LocalizedLanguage)).GetValue(i));
            }
        }

        EditorGUILayout.BeginVertical();

        for (int i = 0; i < Values.Length; i++)
        {
            EditorGUILayout.LabelField(System.Enum.GetName(typeof(LocalizationManager.LocalizedLanguage), i) + " Value: ", GUILayout.MaxWidth(100));

            EditorStyles.textArea.wordWrap = true;
            Values[i] = EditorGUILayout.TextArea(Values[i], EditorStyles.textArea, GUILayout.Height(100), GUILayout.Width(400));
        }

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Add / Edit"))
        {
            if (IsKeyDefined(Key))
            {
                LocalizationManager.Replace(Key, Values);
            }
            else
            {
                LocalizationManager.Add(Key, Values);
            }
            Selection.activeGameObject.GetComponent<Localize>().SetKey(Key);
        }

        minSize = new Vector2(460, 500);
        maxSize = minSize;
    }

    private bool IsKeyDefined(string key)
    {
        for (int i = 0; i < Values.Length; i++)
        {
            if (LocalizationManager.GetLocalizedValue(key, (LocalizationManager.LocalizedLanguage)System.Enum.GetValues(typeof(LocalizationManager.LocalizedLanguage)).GetValue(i)) != null)
            {
                return true;
            }
        }
        return false;
    }

    void OnDestroy()
    {
        IsOpen = false;
    }
}


public class TextLocalizerSearchWindow : EditorWindow
{
    public static bool IsOpen = false;

    public static void Open()
    {
        TextLocalizerSearchWindow window = CreateInstance<TextLocalizerSearchWindow>();
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
        EditorGUILayout.BeginVertical();
        Scroll = EditorGUILayout.BeginScrollView(Scroll);
        foreach (KeyValuePair<string, string> element in dictionary)
        {

            if (value == null || element.Key.ToLower().Contains(value.ToLower()) || element.Value.ToLower().Contains(value.ToLower()))
            {
                EditorGUILayout.BeginHorizontal("box");
                Texture deleteIcon = (Texture)Resources.Load("Localization/delete");
                GUIContent deleteContent = new GUIContent(deleteIcon);

                if (GUILayout.Button(deleteContent, GUILayout.MaxWidth(20), GUILayout.MaxHeight(20)))
                {
                    if (EditorUtility.DisplayDialog("Remove Key " + element.Key + "?", "This will remove the element from Localization, Are you sure?", "Do it"))
                    {
                        LocalizationManager.Remove(element.Key);
                        AssetDatabase.Refresh();
                        LocalizationManager.Init();
                        dictionary = LocalizationManager.GetDictionaryForEditor();
                        Selection.activeGameObject.GetComponent<Localize>().SetKey(string.Empty);
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

    void OnDestroy()
    {
        IsOpen = false;
    }
}