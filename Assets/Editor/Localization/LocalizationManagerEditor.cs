using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LocalizationManager)), CanEditMultipleObjects]
public class LocalizationManagerEditor : Editor
{
    public SerializedProperty
        englishMaterialPresets,
        farsiMaterialPresets,
        englishFontAsset,
        farsiFontAsset;

    void OnEnable()
    {
        LocalizationManager.SetInstance();

        englishMaterialPresets = serializedObject.FindProperty("EnglishMaterialPresets");
        farsiMaterialPresets = serializedObject.FindProperty("FarsiMaterialPresets");
        englishFontAsset = serializedObject.FindProperty("EnglishFontAsset");
        farsiFontAsset = serializedObject.FindProperty("FarsiFontAsset");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(englishFontAsset, new GUIContent("EnglishFontAsset:"), true);

        EditorGUILayout.PropertyField(englishMaterialPresets, new GUIContent("EnglishMaterialPresets:"), true);
        ShowArrayProperty(englishMaterialPresets);

        EditorGUILayout.PropertyField(farsiFontAsset, new GUIContent("FarsiFontAsset:"), true);

        EditorGUILayout.PropertyField(farsiMaterialPresets, new GUIContent("FarsiMaterialPresets:"), true);
        ShowArrayProperty(farsiMaterialPresets);

        serializedObject.ApplyModifiedProperties();
    }

    public void ShowArrayProperty(SerializedProperty list)
    {
        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent(System.Enum.GetName(typeof(LocalizationManager.Outline), i)));
        }
        EditorGUI.indentLevel -= 1;
    }
}