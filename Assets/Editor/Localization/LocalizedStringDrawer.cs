using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LocalizedString))]
public class LocalizedStringDrawer : PropertyDrawer
{
    bool dropdown;
    float height;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (dropdown)
        {
            return height + 20 * System.Enum.GetNames(typeof(LocalizationManager.LocalizedLanguage)).Length;
        }

        return 20;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.width -= 34;
        position.height = 18;

        Rect valueRect = new Rect(position);
        valueRect.x += 15;
        valueRect.y += 20;
        valueRect.width -= 15;

        Rect foldButtonRectRect = new Rect(position);
        foldButtonRectRect.width = 15;

        dropdown = EditorGUI.Foldout(foldButtonRectRect, dropdown, "");

        position.x += 15;
        position.width -= 15;

        SerializedProperty key = property.FindPropertyRelative("key");
        key.stringValue = EditorGUI.TextField(position, key.stringValue);

        position.x += position.width + 2;
        position.width = 17;
        position.height = 17;

        Texture searchIcon = (Texture)Resources.Load("Localization/search");
        GUIContent searchContent = new GUIContent(searchIcon);

        if (GUI.Button(position, searchContent))
        {
            if (!TextLocalizerSearchWindow.IsOpen)
            {
                TextLocalizerSearchWindow.Open();
            }
        }

        position.x += position.width + 2;

        Texture addIcon = (Texture)Resources.Load("Localization/add");
        GUIContent addContent = new GUIContent(addIcon);
        if (GUI.Button(position, addContent))
        {
            if (!TextLocalizerEditWindow.IsOpen)
            {
                TextLocalizerEditWindow.Open(key.stringValue);
            }
        }

        if (dropdown)
        {
            for (int i = 0; i < System.Enum.GetNames(typeof(LocalizationManager.LocalizedLanguage)).Length; i++)
            {
                var value = LocalizationManager.GetLocalizedValue(key.stringValue, (LocalizationManager.LocalizedLanguage)System.Enum.GetValues(typeof(LocalizationManager.LocalizedLanguage)).GetValue(i));
                GUIStyle style = GUI.skin.box;
                height = style.CalcHeight(new GUIContent(value), valueRect.width);

                valueRect.height = height;
                valueRect.width += 21;
                valueRect.y = 20 * (i + 2) + 5;
                EditorGUI.LabelField(valueRect, value, EditorStyles.wordWrappedLabel);
            }
        }

        EditorGUI.EndProperty();
    }
}
