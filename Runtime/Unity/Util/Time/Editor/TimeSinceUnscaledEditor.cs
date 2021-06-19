// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using Ju.Services.Internal;

namespace Ju.Time
{
	[CustomPropertyDrawer(typeof(TimeSinceUnscaled))]
	public class TimeSinceUnscaledDrawer : PropertyDrawer
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
			EditorGUI.BeginDisabledGroup(true);
			if (EditorApplication.isPlaying)
			{
				EditorGUI.TextField(contentRect, (ServiceCache.Time.UnscaledTime - prop.FindPropertyRelative("time").floatValue).ToString());
			}
			else
			{
				EditorGUI.TextField(contentRect, prop.FindPropertyRelative("time").floatValue.ToString());
			}
			EditorGUI.EndDisabledGroup();

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
