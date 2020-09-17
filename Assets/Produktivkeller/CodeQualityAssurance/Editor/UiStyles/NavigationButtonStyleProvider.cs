using UnityEngine;

namespace Produktivkeller.CodeQualityAssurance.Editor.UiStyles
{
    public abstract class NavigationButtonStyleProvider
    {
        public static GUIStyle Provide()
        {
            Texture2D normalTexture = (Texture2D) Resources.Load("1x1-00000000");
            Texture2D highlightTexture = (Texture2D) Resources.Load("1x1-0f1925ff");

            GUIStyle guiStyle = new GUIStyle(GUI.skin.label)
            {
                margin = new RectOffset(0, 20, 10, 0),
                padding = new RectOffset(15, 0, 15, 15),
                fixedWidth = 200,
                normal =
                {
                    textColor = Color.black,
                    background = normalTexture
                },
                onNormal =
                {
                    background = highlightTexture,
                    textColor = Color.white
                }
            };

            return guiStyle;
        }
    }
}