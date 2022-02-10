using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Localization
{
    [CustomPropertyDrawer(typeof(HorizontalOrVerticalLayoutGroupProperties))]
    public class HorizontalOrVerticalLayoutGroupPropertiesDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var childAlignment = property.FindPropertyRelative("ChildAlignment");
            EditorGUILayout.PropertyField(childAlignment, new GUIContent("Child Alignment:"), true);

            var reverseArrangment = property.FindPropertyRelative("ReverseArrangment");
            EditorGUILayout.PropertyField(reverseArrangment, new GUIContent("Reverse Arrangment:"), true);

            EditorGUI.EndProperty();
        }

        //To fix extra spaces above the input field problem.
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) 
        { 
            return 0; 
        }
    }
}
