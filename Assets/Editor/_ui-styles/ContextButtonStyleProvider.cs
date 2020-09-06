using UnityEngine;

namespace Editor
{
    public abstract class ContextButtonStyleProvider
    {
        public static GUIStyle Provide()
        {
            GUIStyle guiStyle = new GUIStyle (GUI.skin.button);

            guiStyle.fixedWidth = 20;
            guiStyle.fixedHeight = 20;

            return guiStyle;
        }
    }
}