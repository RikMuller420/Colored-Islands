using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Manages data and preferences for the NamespaceGeneratorWindow
    /// </summary>
    public class NamespaceGeneratorWindowData
    {
        private const string FOLDER_OVERRIDES_PREF_KEY = "NamespaceGenerator_FolderOverrides";
        private const string LAST_FOLDER_PREF_KEY = "NamespaceGenerator_LastFolder";

        public string rootNamespace = "YourCompany.YourGame";
        public bool includeFolderStructure = true;
        public string selectedFolderPath = "Assets";
        public Dictionary<string, string> folderOverrides = new Dictionary<string, string>();
        public Dictionary<string, string> persistentFolderOverrides = new Dictionary<string, string>();
        public List<ScriptInfo> filteredScripts = new List<ScriptInfo>();
        public FolderNode rootFolderNode;
        public bool isLoading = false;
        public bool isGenerating;
        public int selectedTab = 0;
        public readonly string[] tabOptions = { "Folders", "Scripts" };

        public void LoadPreferences()
        {
            selectedFolderPath = EditorPrefs.GetString(LAST_FOLDER_PREF_KEY, "Assets");
            LoadFolderOverrides();
        }

        public void SavePreferences()
        {
            SaveFolderOverrides();
            EditorPrefs.SetString(LAST_FOLDER_PREF_KEY, selectedFolderPath);
        }

        private void LoadFolderOverrides()
        {
            persistentFolderOverrides.Clear();
            string savedData = EditorPrefs.GetString(FOLDER_OVERRIDES_PREF_KEY, "");
            if (!string.IsNullOrEmpty(savedData))
            {
                foreach (string entry in savedData.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] parts = entry.Split('|', 2);
                    if (parts.Length == 2) persistentFolderOverrides[parts[0]] = parts[1].Trim();
                }
            }
        }

        private void SaveFolderOverrides()
        {
            var sb = new System.Text.StringBuilder();
            foreach (var kvp in persistentFolderOverrides.Where(kvp => !string.IsNullOrEmpty(kvp.Value)))
                sb.AppendLine($"{kvp.Key}|{kvp.Value}");
            EditorPrefs.SetString(FOLDER_OVERRIDES_PREF_KEY, sb.ToString());
        }

        public void InitializeOverrides()
        {
            folderOverrides.Clear();
            foreach (var kvp in persistentFolderOverrides)
                folderOverrides[kvp.Key] = kvp.Value;
        }

        public void UpdateGeneratedNamespaces(NamespaceProcessor namespaceProcessor)
        {
            if (namespaceProcessor != null)
            {
                namespaceProcessor.RegenerateNamespaces(rootNamespace, includeFolderStructure, folderOverrides);

                if (filteredScripts != null)
                {
                    foreach (var script in filteredScripts)
                    {
                        if (string.IsNullOrEmpty(script.customNamespace))
                        {
                            script.generatedNamespace = namespaceProcessor.GenerateNamespaceForScript(script.path);
                        }
                    }
                }
            }
        }
    }
}