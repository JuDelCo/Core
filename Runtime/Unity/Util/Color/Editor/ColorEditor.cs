// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Ju.Time
{
	using Ju.Color;

	[CustomPropertyDrawer(typeof(Color))]
	public class ColorDrawer : PropertyDrawer
	{
		private const float BottomSpacing = 2;

		public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
		{
			pos.height -= BottomSpacing;
			label = EditorGUI.BeginProperty(pos, label, prop);

			var contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
			var indent = EditorGUI.indentLevel;
			var labelWidth = EditorGUIUtility.labelWidth;

			EditorGUI.indentLevel = 0;
			EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(new GUIContent("")).x;
			var color = EditorGUI.ColorField(contentRect, new GUIContent(""), new UnityEngine.Color(prop.FindPropertyRelative("rValue").floatValue, prop.FindPropertyRelative("gValue").floatValue, prop.FindPropertyRelative("bValue").floatValue, prop.FindPropertyRelative("aValue").floatValue), true, true, true);
			prop.FindPropertyRelative("rValue").floatValue = color.r;
			prop.FindPropertyRelative("gValue").floatValue = color.g;
			prop.FindPropertyRelative("bValue").floatValue = color.b;
			prop.FindPropertyRelative("aValue").floatValue = color.a;

			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUI.indentLevel = indent;

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return base.GetPropertyHeight(property, label) + BottomSpacing;
		}
	}
}

#endif
#endif
