using UnityEngine;

namespace Editor._ui
{
    public abstract class CqaButton
    {
        internal static bool NormalButton(string text)
        {
            return GUILayout.Button(text, GUILayout.Width(150));
        }
    }
}