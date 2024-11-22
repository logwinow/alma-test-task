using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// custom
namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(RangeSliderAttribute))]
    public class RangeSliderPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return (property.propertyType == SerializedPropertyType.Float || property.propertyType == SerializedPropertyType.Integer)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            RangeSliderAttribute rangeSliderAttirbute = (RangeSliderAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Float || property.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.BeginProperty(rect, label, property);

                float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
                float labelWidth = EditorGUIUtility.labelWidth + NaughtyEditorGUI.HorizontalSpacing;
                float sliderWidth = rect.width - labelWidth;
                float sliderPadding = 5.0f;

                Rect labelRect = new Rect(
                    rect.x,
                    rect.y,
                    labelWidth,
                    rect.height);

                Rect sliderRect = new Rect(
                    rect.x + labelWidth + sliderPadding - indentLength,
                    rect.y,
                    sliderWidth - 2.0f * sliderPadding + indentLength,
                    rect.height);

                // Draw the label
                EditorGUI.LabelField(labelRect, label.text);

                // Draw the slider
                EditorGUI.BeginChangeCheck();

                if (property.propertyType == SerializedPropertyType.Float)
                {
                    float sliderValue = property.floatValue;
                    sliderValue = EditorGUI.Slider(sliderRect, sliderValue, rangeSliderAttirbute.MinValue, rangeSliderAttirbute.MaxValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.floatValue = sliderValue;
                    }
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    int sliderValue = property.intValue;

                    sliderValue = EditorGUI.IntSlider(sliderRect, sliderValue, (int)rangeSliderAttirbute.MinValue, (int)rangeSliderAttirbute.MaxValue);

                    if (EditorGUI.EndChangeCheck())
                    {
                        property.floatValue = sliderValue;
                    }
                }

                EditorGUI.EndProperty();
            }
            else
            {
                string message = rangeSliderAttirbute.GetType().Name + " can be used only on Float or Int fields";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }
    }
}