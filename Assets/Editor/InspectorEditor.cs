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
        localizedString;

    void OnEnable()
    {
        targetComponent = serializedObject.FindProperty("Target");
        audioClips = serializedObject.FindProperty("AudioClips");
        sprites = serializedObject.FindProperty("Sprites");
        localizedString = serializedObject.FindProperty("LocalizedString");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(targetComponent);

        Localize.TargetComponent target = (Localize.TargetComponent)targetComponent.enumValueIndex;

        //EditorGUI.BeginDisabledGroup(target != Localize.TargetComponent.AudioSource);
        //EditorGUILayout.PropertyField(audioClips, new GUIContent("AudioClips"), true);
        //EditorGUI.EndDisabledGroup();

        //EditorGUI.BeginDisabledGroup(target != Localize.TargetComponent.Image);
        //EditorGUILayout.PropertyField(sprites, new GUIContent("Sprites"), true);
        //EditorGUI.EndDisabledGroup();

        //EditorGUI.BeginDisabledGroup(target != Localize.TargetComponent.Text);
        //EditorGUILayout.PropertyField(localizedString, new GUIContent("LocalizedString"), true);
        //EditorGUI.EndDisabledGroup();

        switch (target)
        {
            case Localize.TargetComponent.AudioSource:
                EditorGUILayout.PropertyField(audioClips, new GUIContent("AudioClips"), true);
                break;
            case Localize.TargetComponent.Image:
                EditorGUILayout.PropertyField(sprites, new GUIContent("Sprites"), true);
                break;
            case Localize.TargetComponent.Text:
                EditorGUILayout.PropertyField(localizedString, new GUIContent("Text"), true);
                break;
        }   

        serializedObject.ApplyModifiedProperties();
    }
}
