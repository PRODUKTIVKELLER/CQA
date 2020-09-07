using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.UiStyles
{
    public abstract class ContextButtonStyleProvider
    {
        public static GUIStyle Provide()
        {
            GUIStyle guiStyle = new GUIStyle(GUI.skin.button)
            {
                fixedWidth = 20,
                fixedHeight = 20
            };

            return guiStyle;
        }
    }
}