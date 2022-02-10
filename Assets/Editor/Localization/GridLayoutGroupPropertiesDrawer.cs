using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Localization
{
    [CustomPropertyDrawer(typeof(GridLayoutGroupProperties))]
    public class GridLayoutGroupPropertiesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var startCorner = property.FindPropertyRelative("StartCorner");
            EditorGUILayout.PropertyField(startCorner, new GUIContent("Start Corner"), true);

            var childAlignment = property.FindPropertyRelative("ChildAlignment");
            EditorGUILayout.PropertyField(childAlignment, new GUIContent("Child Alignment"), true);

            EditorGUI.EndProperty();
        }

        //To fix extra spaces above the input field problem.
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        { 
            return 0; 
        }
    }
}
