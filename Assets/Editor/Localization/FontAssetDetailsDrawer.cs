using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Localization
{
    [CustomPropertyDrawer(typeof(FontAssetDetails))]
    public class FontAssetDetailsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            FontAssetDetails fontAssetDetails = fieldInfo.GetValue(property.serializedObject.targetObject) as FontAssetDetails;

            var fontAsset = property.FindPropertyRelative("FontAsset");

            EditorGUI.indentLevel += 1;
            EditorGUILayout.PropertyField(fontAsset, new GUIContent("Font Asset"), true);
            EditorGUI.indentLevel -= 1;

            var materialPresets = property.FindPropertyRelative("MaterialPresets");
            ShowArrayProperty(materialPresets);

            EditorGUI.EndProperty();
        }

        public void ShowArrayProperty(SerializedProperty list)
        {
            EditorGUI.indentLevel += 1;
            for (int i = 0; i < list.arraySize; i++)
            {
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), new GUIContent(System.Enum.GetName(typeof(Outline), i)));
            }
            EditorGUI.indentLevel -= 1;
        }

        //To fix extra spaces above the input field problem.
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        { 
            return 0; 
        }
    }
}
