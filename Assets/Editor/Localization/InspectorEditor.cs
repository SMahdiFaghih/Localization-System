using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Localize)), CanEditMultipleObjects]
public class InspectorEditor : Editor
{
    public SerializedProperty
        targetComponent,
        audioClips,
        sprites,
        localizedString,
        fonts,
        fontAssets;

    void OnEnable()
    {
        targetComponent = serializedObject.FindProperty("Target");
        audioClips = serializedObject.FindProperty("AudioClips");
        sprites = serializedObject.FindProperty("Sprites");
        localizedString = serializedObject.FindProperty("LocalizedString");
        fonts = serializedObject.FindProperty("Fonts");
        fontAssets = serializedObject.FindProperty("FontAssets");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(targetComponent);

        Localize.TargetComponent target = (Localize.TargetComponent)targetComponent.enumValueIndex;

        switch (target)
        {
            case Localize.TargetComponent.AudioSource:
                EditorGUILayout.PropertyField(audioClips, new GUIContent("AudioClips:"), true);
                ShowArrayProperty(audioClips);
                break;
            case Localize.TargetComponent.Image:
                EditorGUILayout.PropertyField(sprites, new GUIContent("Sprites:"), true);
                ShowArrayProperty(sprites);
                break;
            case Localize.TargetComponent.RTLText:
                EditorGUILayout.PropertyField(localizedString, new GUIContent("Key"), true);
                break;
            case Localize.TargetComponent.Font:
                EditorGUILayout.PropertyField(fonts, new GUIContent("Fonts:"), true);
                ShowArrayProperty(fonts);
                break;
            case Localize.TargetComponent.FontAsset:
                EditorGUILayout.PropertyField(fontAssets, new GUIContent("FontAssets:"), true);
                ShowArrayProperty(fontAssets);
                break;
        }   

        serializedObject.ApplyModifiedProperties();
    }

    public void ShowArrayProperty(SerializedProperty list)
    {
        EditorGUI.indentLevel += 1;
        for (int i = 0; i < list.arraySize; i++)
        {
            EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent(System.Enum.GetName(typeof(LocalizationManager.LocalizedLanguage), i)));
        }
        EditorGUI.indentLevel -= 1;
    }
}
