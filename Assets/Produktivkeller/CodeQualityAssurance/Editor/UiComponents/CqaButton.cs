using Produktivkeller.CodeQualityAssurance.Editor.UiStyles;
using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.UiComponents
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

        internal static bool SmallButton(string text)
        {
            GUILayout.Space(5);
            bool pressed = GUILayout.Button(text, GUILayout.Width(100));
            GUILayout.Space(0);
            return pressed;
        }

        internal static bool EditButton()
        {
            bool pressed = GUILayout.Button(Resources.Load("pen") as Texture2D, ContextButtonStyleProvider.Provide());
            return pressed;
        }
    }
}