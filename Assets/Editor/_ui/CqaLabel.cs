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
        
        internal static void Heading2(string text)
        {
            GUIStyle guiStyle = new GUIStyle {fontSize = 15, fontStyle = FontStyle.Bold};

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
        
        internal static void Slim(string text)
        {
            EditorGUILayout.LabelField(text);
        }
        
        internal static void FormLabel(string text)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontStyle = FontStyle.Bold;
            
            EditorGUILayout.LabelField(text, guiStyle);
        }
        
        internal static void FormDescription(string text)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.wordWrap = true;
            guiStyle.padding = new RectOffset(3, 0, 5, 10);
            
            EditorGUILayout.LabelField(text, guiStyle);
        }

        internal static void FoldoutEntry(string text)
        {
            GUIStyle guiStyle = new GUIStyle {alignment = TextAnchor.MiddleLeft};
            EditorGUILayout.LabelField(text, guiStyle);
        }

        internal static void FormGroupSpacing()
        {
            GUILayout.Space(15);
        }

        public static void FormError(string text)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.normal.textColor = new Color(0.7f, 0, 0);
            EditorGUILayout.LabelField(text, guiStyle);
        }
    }
}