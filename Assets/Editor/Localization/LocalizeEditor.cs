﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Localization
{
    [CustomEditor(typeof(Localize)), CanEditMultipleObjects]
    public class LocalizeEditor : Editor
    {
        private Localize TargetLocalize;

        public SerializedProperty
            targetComponent,
            audioClips,
            sprites,
            localizedString,
            gridLayoutGroupProperties,
            horizontalOrVerticalLayoutGroupProperties,
            rectTransformProperties,
            outline,
            fixedFontAsset,
            fixedFontAssetDetails,          
            isContainsAtSign;

        void OnEnable()
        {
            LocalizationManager.SetInstance();
            TargetLocalize = (Localize)target;

            targetComponent = serializedObject.FindProperty("Target");
            audioClips = serializedObject.FindProperty("AudioClips");
            sprites = serializedObject.FindProperty("Sprites");
            localizedString = serializedObject.FindProperty("LocalizedString");
            gridLayoutGroupProperties = serializedObject.FindProperty("GridLayoutGroupProperties");
            horizontalOrVerticalLayoutGroupProperties = serializedObject.FindProperty("HorizontalOrVerticalLayoutGroupProperties");
            outline = serializedObject.FindProperty("Outline");
            rectTransformProperties = serializedObject.FindProperty("RectTransformProperties");
            fixedFontAsset = serializedObject.FindProperty("FixedFontAsset");
            fixedFontAssetDetails = serializedObject.FindProperty("FixedFontAssetDetails");
            isContainsAtSign = serializedObject.FindProperty("IsContainsAtSign");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(targetComponent);

            Localize.TargetComponent target = (Localize.TargetComponent)targetComponent.enumValueIndex;

            switch (target)
            {
                case Localize.TargetComponent.RTLTextMeshPro:
                    EditorGUILayout.PropertyField(localizedString, new GUIContent("Key"), true);
                    EditorGUILayout.PropertyField(outline, new GUIContent("Outline"), true);

                    EditorGUILayout.PropertyField(fixedFontAsset, new GUIContent("Fixed FontAsset"), true);
                    if (TargetLocalize.FixedFontAsset)
                    {
                        EditorGUILayout.PropertyField(fixedFontAssetDetails, new GUIContent("Fixed FontAsset Details"), true);
                    }

                    EditorGUILayout.PropertyField(isContainsAtSign, new GUIContent("Contains AtSign(@)"), true);
                    break;
                case Localize.TargetComponent.AudioSource:
                    EditorGUILayout.PropertyField(audioClips, new GUIContent("AudioClips"), false);
                    ShowArrayProperty(audioClips);
                    break;
                case Localize.TargetComponent.Image:
                    EditorGUILayout.PropertyField(sprites, new GUIContent("Sprites"), false);
                    ShowArrayProperty(sprites);
                    break;
                case Localize.TargetComponent.GridLayoutGroup:
                    EditorGUILayout.PropertyField(gridLayoutGroupProperties, new GUIContent("GridLayoutGroup Properties"), false);
                    ShowArrayPropertyForCustomClass(gridLayoutGroupProperties);
                    break;
                case Localize.TargetComponent.HorizontalOrVerticalLayoutGroup:
                    EditorGUILayout.PropertyField(horizontalOrVerticalLayoutGroupProperties, new GUIContent("HorizontalOrVerticalLayoutGroup Properties"), false);
                    ShowArrayPropertyForCustomClass(horizontalOrVerticalLayoutGroupProperties);
                    break;
                case Localize.TargetComponent.RectTransform:
                    EditorGUILayout.PropertyField(rectTransformProperties, new GUIContent("RectTransform Properties"), false);
                    ShowButtonsForArrayProperty(rectTransformProperties);
                    break;
            }

            foreach (LocalizedLanguage language in System.Enum.GetValues(typeof(LocalizedLanguage)))
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
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent(System.Enum.GetName(typeof(LocalizedLanguage), i)));
            }
            EditorGUI.indentLevel -= 1;
        }

        public void ShowArrayPropertyForCustomClass(SerializedProperty list)
        {
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.LabelField(new GUIContent(System.Enum.GetName(typeof(LocalizedLanguage), i)));
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent(System.Enum.GetName(typeof(LocalizedLanguage), i)));
            }
            EditorGUI.indentLevel -= 1;
        }

        public void ShowButtonsForArrayProperty(SerializedProperty list)
        {
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUIUtility.labelWidth = 25;
                EditorGUILayout.LabelField(new GUIContent(System.Enum.GetName(typeof(LocalizedLanguage), i)));

                if (GUILayout.Button("Apply Current Properties"))
                {
                    TargetLocalize.SetRectTransformProperties(i);
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel -= 1;
        }
    }
}