using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Localization
{
    [CustomEditor(typeof(LocalizationManager)), CanEditMultipleObjects]
    public class LocalizationManagerEditor : Editor
    {
        public SerializedProperty
            englishFontAssetDetails,
            farsiFontAssetDetails;

        void OnEnable()
        {
            LocalizationManager.SetInstance();

            englishFontAssetDetails = serializedObject.FindProperty("EnglishFontAssetDetails");
            farsiFontAssetDetails = serializedObject.FindProperty("FarsiFontAssetDetails");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(englishFontAssetDetails, new GUIContent("EnglishFontAsset:"), true);
            EditorGUILayout.PropertyField(farsiFontAssetDetails, new GUIContent("FarsiFontAsset:"), true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}