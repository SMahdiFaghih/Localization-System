using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Localize)), CanEditMultipleObjects]
public class LocalizeEditor : Editor
{
    private Localize TargetLocalize;

    public SerializedProperty
        targetComponent,
        audioClips,
        sprites,
        localizedString,
        fonts,
        fontAssets,
        startCorner,
        positions,
        outline,
        fixedFontAsset,
        isContainsAtSign;

    void OnEnable()
    {
        LocalizationManager.SetInstance();
        TargetLocalize = (Localize)target;

        targetComponent = serializedObject.FindProperty("Target");
        audioClips = serializedObject.FindProperty("AudioClips");
        sprites = serializedObject.FindProperty("Sprites");
        localizedString = serializedObject.FindProperty("LocalizedString");
        fonts = serializedObject.FindProperty("Fonts");
        fontAssets = serializedObject.FindProperty("FontAssets");
        startCorner = serializedObject.FindProperty("StartCorner");
        outline = serializedObject.FindProperty("Outline");
        positions = serializedObject.FindProperty("Positions");
        fixedFontAsset = serializedObject.FindProperty("FixedFontAsset");
        isContainsAtSign = serializedObject.FindProperty("IsContainsAtSign");
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
                EditorGUILayout.PropertyField(outline, new GUIContent("Outline"), true);
                EditorGUILayout.PropertyField(fixedFontAsset, new GUIContent("FixedFontAsset?"), true);
                EditorGUILayout.PropertyField(isContainsAtSign, new GUIContent("ContainsAtSign? (@)"), true);
                break;
            case Localize.TargetComponent.Font:
                EditorGUILayout.PropertyField(fonts, new GUIContent("Fonts:"), true);
                ShowArrayProperty(fonts);
                break;
            case Localize.TargetComponent.FontAsset:
                EditorGUILayout.PropertyField(fontAssets, new GUIContent("FontAssets:"), true);
                ShowArrayProperty(fontAssets);
                break;
            case Localize.TargetComponent.GridLayoutGroup:
                EditorGUILayout.PropertyField(startCorner, new GUIContent("StartCorner:"), true);
                break;
            case Localize.TargetComponent.Position2D:
                EditorGUILayout.PropertyField(positions, new GUIContent("2D Positions:"), true);
                ShowArrayProperty(positions);
                break;
        }

        foreach (LocalizationManager.LocalizedLanguage language in System.Enum.GetValues(typeof(LocalizationManager.LocalizedLanguage)))
        {
            if (GUILayout.Button(language.ToString()))
            {
                TargetLocalize.ApplyLocalization((int)language, true);
            }
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