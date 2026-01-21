using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Parity.NamespaceGenerator.Editor
{
    public class ScriptInfo
    {
        public string path;
        public string fileName => Path.GetFileName(path);
        public string folderPath => Path.GetDirectoryName(path).Replace('\\', '/');
        public string generatedNamespace;
        public string customNamespace;
        public bool hasExistingNamespace;
        public bool isEditorScript;
        public bool includeInBatch;
        public bool wantsToUpdate;

        public List<string> requiredUsings = new List<string>();
        public bool usingsProcessed = false;

        public string GetFinalNamespace()
        {
            return !string.IsNullOrEmpty(customNamespace) ? customNamespace : generatedNamespace;
        }
    }

    public class NamespaceProcessor
    {
        private NamespaceGenerator namespaceGenerator;
        private string rootNamespace;
        private bool includeFolderStructure;
        private string selectedFolderPath;
        private Dictionary<string, string> folderOverrides;

        public void Initialize(string rootNamespace, bool includeFolderStructure, string selectedFolderPath, Dictionary<string, string> folderOverrides)
        {
            this.rootNamespace = rootNamespace;
            this.includeFolderStructure = includeFolderStructure;
            this.selectedFolderPath = selectedFolderPath;
            this.folderOverrides = folderOverrides ?? new Dictionary<string, string>();
            this.namespaceGenerator = new NamespaceGenerator(this.rootNamespace, this.includeFolderStructure, this.selectedFolderPath, this.folderOverrides);
        }

        public List<ScriptInfo> ScanScripts(string rootPath)
        {
            var scripts = new List<ScriptInfo>();
            string[] files = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories);
            int total = files.Length;
            for (int i = 0; i < total; i++)
            {
                string file = files[i];
                // Update progress every 10 files or at the end
                if (i % 10 == 0 || i == total - 1)
                {
                    EditorUtility.DisplayProgressBar("Scanning Scripts", $"Processing {Path.GetFileName(file)}", 0.1f + 0.9f * (float)i / total);
                }
                // REMOVE the editor file filtering - we want to include all files
                // if (file.Contains("\\Editor\\")) // Remove this check
                var scriptInfo = new ScriptInfo
                {
                    path = file.Replace('\\', '/'),
                    hasExistingNamespace = CodeUpdater.HasNamespace(File.ReadAllText(file)),
                    isEditorScript = file.Contains("/Editor/") || file.Contains("\\Editor\\")
                };
                // Generate the initial namespace
                scriptInfo.generatedNamespace = namespaceGenerator.GenerateNamespaceName(scriptInfo.path);
                scriptInfo.includeInBatch = !scriptInfo.hasExistingNamespace; // Default selection
                scriptInfo.wantsToUpdate = scriptInfo.hasExistingNamespace; // Default to update existing
                scripts.Add(scriptInfo);
            }
            return scripts.OrderBy(s => s.path).ToList();
        }

        // This method might be redundant if Initialize is always called.
        public void RegenerateNamespaces(string rootNamespace, bool includeFolderStructure, Dictionary<string, string> folderOverrides)
        {
            // This is where the bug likely was. It might not have been updating its internal state correctly.
            // Replacing this with a call to Initialize is safer.
            Initialize(rootNamespace, includeFolderStructure, this.selectedFolderPath, folderOverrides);
        }

        public string GenerateNamespaceForScript(string scriptPath)
        {
            // Ensure the generator is not null before calling
            if (namespaceGenerator == null)
            {
                // This could happen if Initialize was never called.
                // As a fallback, we could try to initialize with default values, but that's probably not needed.
                // Or just return an empty string or throw an exception.
                return "";
            }
            return namespaceGenerator.GenerateNamespaceName(scriptPath);
        }
    }
}