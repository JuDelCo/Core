// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Ju.Time
{
	using Ju.Color;

	[CustomPropertyDrawer(typeof(Color32))]
	public class Color32Drawer : PropertyDrawer
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
			Color32 color = EditorGUI.ColorField(contentRect, new GUIContent(""), new UnityEngine.Color32((byte)prop.FindPropertyRelative("rValue").intValue, (byte)prop.FindPropertyRelative("gValue").intValue, (byte)prop.FindPropertyRelative("bValue").intValue, (byte)prop.FindPropertyRelative("aValue").intValue), true, true, false);
			prop.FindPropertyRelative("rValue").intValue = color.r;
			prop.FindPropertyRelative("gValue").intValue = color.g;
			prop.FindPropertyRelative("bValue").intValue = color.b;
			prop.FindPropertyRelative("aValue").intValue = color.a;

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
