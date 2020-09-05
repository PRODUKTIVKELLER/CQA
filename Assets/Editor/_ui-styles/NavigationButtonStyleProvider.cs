using UnityEngine;

namespace Editor
{
    public abstract class NavigationButtonStyleProvider
    {
        public static GUIStyle Provide()
        {
            Texture2D normalTexture = (Texture2D) Resources.Load("1x1-00000000");
            Texture2D highlightTexture = (Texture2D) Resources.Load("1x1-0f1925ff");
            
            GUIStyle guiStyle = new GUIStyle (GUI.skin.label);
            guiStyle.margin=new RectOffset(0,20,10,0);
            guiStyle.padding=new RectOffset(15,0,5,5);
            guiStyle.fixedWidth = 200;

            guiStyle.normal.textColor = Color.black;
            guiStyle.normal.background = normalTexture;

            guiStyle.onNormal.background = highlightTexture;
            guiStyle.onNormal.textColor = Color.white;

            return guiStyle;
        }
    }
}