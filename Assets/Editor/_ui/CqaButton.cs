using UnityEditor;
using UnityEngine;

namespace Editor._ui
{
    public abstract class CqaButton
    {
        internal static bool NormalButton(string text)
        {
            GUILayout.Space(5);
            bool pressed = GUILayout.Button(text, GUILayout.Width(150));
            GUILayout.Space(0);
            return pressed;
        }
        
    }
}