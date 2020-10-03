using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Components)), CanEditMultipleObjects]
public class InspectorEditor : Editor
{
    public SerializedProperty
        targetComponent,
        audioClips,
        images,
        text;
    void OnEnable()
    {
        targetComponent = serializedObject.FindProperty("Target");
        audioClips = serializedObject.FindProperty("AudioClips");
        images = serializedObject.FindProperty("Images");
        text = serializedObject.FindProperty("Text");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(targetComponent);

        Localize.TargetComponent target = (Localize.TargetComponent)targetComponent.enumValueIndex;
        switch (target)
        {
            case Localize.TargetComponent.AudioSource:
                EditorGUILayout.PropertyField(audioClips, new GUIContent("AudioClips"));
                break;
            case Localize.TargetComponent.Image:
                EditorGUILayout.PropertyField(images, new GUIContent("Images"));
                break;
            case Localize.TargetComponent.Text:
                EditorGUILayout.PropertyField(text, new GUIContent("Text"));
                break;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
