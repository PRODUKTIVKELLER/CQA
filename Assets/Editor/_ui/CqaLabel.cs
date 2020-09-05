using System;
using UnityEditor;
using UnityEngine;

namespace Editor._ui
{
    public abstract class CqaLabel
    {
        internal static void Heading1(string text)
        {
            GUIStyle guiStyle = new GUIStyle {fontSize = 20, fontStyle = FontStyle.Bold};
            
            GUILayout.Space(20);
            EditorGUILayout.LabelField(text, guiStyle);
            GUILayout.Space(20);
        }

        internal static void Error(string text)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = new Color(0.7f, 0, 0);
            guiStyle.fontStyle = FontStyle.Bold;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(text, guiStyle);
            EditorGUILayout.Space();
        }

        internal static void Success(string text)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = new Color(0, 0.7f, 0);
            guiStyle.fontStyle = FontStyle.Bold;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(text, guiStyle);
            EditorGUILayout.Space();
        }

        internal static void Bold(string text)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontStyle = FontStyle.Bold;
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(text, guiStyle);
            EditorGUILayout.Space();
        }

        internal static void Normal(string text)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(text);
            EditorGUILayout.Space();
        }
    }
}