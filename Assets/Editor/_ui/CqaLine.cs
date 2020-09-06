using UnityEditor;
using UnityEngine;

namespace Editor._ui
{
    public abstract class CqaLine
    {
        public static void DrawHorizontalLine()
        {
            const float padding = 0;
            const float thickness = 1;

            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2f;
            r.x -= 22;
            r.width += 26;
            EditorGUI.DrawRect(r, Color.grey);
        }

        public static void DrawNavigationLine(float height)
        {
            EditorGUI.DrawRect(
                new Rect(
                    200,
                    0,
                    1, 
                    height
                ), new Color(15 / 255f, 25 / 255f, 37 / 255f));
        }

        public static void DrawVerticalLine(float height, float x)
        {
            EditorGUI.DrawRect(
                new Rect(
                    x,
                    0,
                    1, 
                    height
                ), new Color(15 / 255f, 25 / 255f, 37 / 255f));
        }
    }
}