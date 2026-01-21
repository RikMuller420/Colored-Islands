using UnityEditor;
using UnityEngine;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Manages GUI styles for the NamespaceGeneratorWindow
    /// </summary>
    public class NamespaceGeneratorWindowStyles
    {
        public GUIStyle headerStyle;
        public GUIStyle sectionHeaderStyle;
        public GUIStyle tabButtonStyle;
        public GUIStyle activeTabButtonStyle;
        public GUIStyle statusBoxStyle;
        public GUIStyle successBoxStyle;
        public GUIStyle warningBoxStyle;
        public GUIStyle buttonStyle;
        public GUIStyle primaryButtonStyle;
        public GUIStyle folderBoxStyle;
        public GUIStyle scriptBoxStyle;

        public Color primaryColor = new Color(0f, 0.44f, 0.87f, 1f);
        public Color secondaryColor = new Color(0.44f, 0.5f, 0.56f, 1f);
        public Color successColor = new Color(0f, 0.55f, 0.27f, 1f);
        public Color warningColor = new Color(0.91f, 0.44f, 0.09f, 1f);
        public Color infoColor = new Color(0.2f, 0.6f, 0.8f, 1f);

        private bool stylesInitialized = false;

        public void InitializeIfNeeded()
        {
            if (stylesInitialized) return;

            headerStyle = new GUIStyle(EditorStyles.largeLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 24,
                fontStyle = FontStyle.Bold,
                normal = { textColor = primaryColor },
                padding = new RectOffset(0, 0, 5, 10)
            };

            sectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 14,
                normal = { textColor = primaryColor },
                padding = new RectOffset(0, 0, 5, 5),
                clipping = TextClipping.Overflow
            };

            tabButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { background = MakeTexture(1, 1, new Color(0.8f, 0.8f, 0.8f, 0.5f)) },
                fixedHeight = 25
            };

            activeTabButtonStyle = new GUIStyle(GUI.skin.button)
            {
                normal = { background = MakeTexture(1, 1, primaryColor), textColor = Color.white },
                fixedHeight = 25
            };

            statusBoxStyle = new GUIStyle(EditorStyles.helpBox)
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(0, 0, 5, 5)
            };

            successBoxStyle = new GUIStyle(statusBoxStyle)
            {
                normal = { background = MakeTexture(1, 1, new Color(0.2f, 0.7f, 0.2f, 0.1f)), textColor = successColor }
            };

            warningBoxStyle = new GUIStyle(statusBoxStyle)
            {
                normal = { background = MakeTexture(1, 1, new Color(0.8f, 0.6f, 0.2f, 0.1f)), textColor = warningColor }
            };

            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                padding = new RectOffset(10, 10, 5, 5),
                margin = new RectOffset(2, 2, 2, 2)
            };

            primaryButtonStyle = new GUIStyle(buttonStyle)
            {
                normal = { background = MakeTexture(1, 1, primaryColor), textColor = Color.white },
                hover = { background = MakeTexture(1, 1, new Color(0.3f, 0.5f, 0.9f, 1f)) }
            };

            folderBoxStyle = new GUIStyle("box")
            {
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(0, 0, 5, 5)
            };

            scriptBoxStyle = new GUIStyle("box")
            {
                padding = new RectOffset(5, 5, 5, 5),
                margin = new RectOffset(0, 0, 2, 2)
            };

            stylesInitialized = true;
        }

        private Texture2D MakeTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = color;
            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }
    }
}