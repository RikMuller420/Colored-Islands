using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Main window for the namespace generator
    /// </summary>
    public class NamespaceGeneratorWindow : EditorWindow
    {
        private NamespaceGeneratorWindowData data;
        private NamespaceGeneratorWindowStyles styles;
        private NamespaceGeneratorWindowGUI gui;
        private FolderTreeManager folderTreeManager;
        private NamespaceProcessor namespaceProcessor;

        [MenuItem("Tools/Generate Namespaces")]
        public static void ShowWindow() => GetWindow<NamespaceGeneratorWindow>("Namespace Generator");

        private void OnEnable()
        {
            InitializeComponents();
            data.LoadPreferences();

            // Auto-scan the default folder if it exists
            if (System.IO.Directory.Exists(data.selectedFolderPath))
            {
                // Use a delay to ensure the window is fully initialized
                AutoScanInitialFolder();
            }
        }

        private void OnDisable()
        {
            data.SavePreferences();
        }

        private void InitializeComponents()
        {
            data = new NamespaceGeneratorWindowData();
            styles = new NamespaceGeneratorWindowStyles();
            folderTreeManager = new FolderTreeManager();
            namespaceProcessor = new NamespaceProcessor();

            gui = new NamespaceGeneratorWindowGUI(data, styles, folderTreeManager, namespaceProcessor, this);
        }

        private void OnGUI()
        {
            gui.OnGUI(position);
        }

        /// <summary>
        /// Automatically scans the initial folder when the window is first opened
        /// </summary>
        private void AutoScanInitialFolder()
        {
            // Only scan if we haven't scanned yet (no scripts loaded)
            if (data.filteredScripts == null || data.filteredScripts.Count == 0)
            {
                // Initialize the processor with current settings
                namespaceProcessor.Initialize(data.rootNamespace, data.includeFolderStructure,
                    data.selectedFolderPath, data.folderOverrides);

                // Perform the scan
                data.rootFolderNode = folderTreeManager.BuildFolderTree(data.selectedFolderPath);
                data.filteredScripts = namespaceProcessor.ScanScripts(data.selectedFolderPath);

                // Mark folders with scripts
                var scriptFolders = new HashSet<string>(data.filteredScripts.Select(s => s.folderPath));
                folderTreeManager.MarkFoldersWithScripts(data.rootFolderNode, scriptFolders);

                // Initialize overrides
                data.InitializeOverrides();
                data.filteredScripts = data.filteredScripts.OrderBy(s => s.path).ToList();

                // This Repaint() call is correct because it's inside the EditorWindow class
                Repaint();
            }
        }
    }
}