using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Handles GUI rendering for the NamespaceGeneratorWindow
    /// </summary>
    public class NamespaceGeneratorWindowGUI
    {
        private readonly NamespaceGeneratorWindowData data;
        private readonly NamespaceGeneratorWindowStyles styles;
        private readonly FolderTreeManager folderTreeManager;
        private readonly NamespaceProcessor namespaceProcessor;
        private readonly EditorWindow window; // <-- Add this field

        private Vector2 scrollPosition;
        private Vector2 folderScrollPosition;

        public NamespaceGeneratorWindowGUI(NamespaceGeneratorWindowData data,
                                          NamespaceGeneratorWindowStyles styles,
                                          FolderTreeManager folderTreeManager,
                                          NamespaceProcessor namespaceProcessor,
                                          EditorWindow window)
        {
            this.data = data;
            this.styles = styles;
            this.folderTreeManager = folderTreeManager;
            this.namespaceProcessor = namespaceProcessor;
            this.window = window; // <-- Assign the window instance
        }

        public void OnGUI(Rect position)
        {
            styles.InitializeIfNeeded();

            float headerHeight = 100; // Reduced from 120
            float tabHeight = 35;
            float padding = 10;

            DrawHeader(position);
            DrawTabButtons();
            EditorGUILayout.Space(padding);

            float remainingHeight = position.height - headerHeight - tabHeight - padding;

            switch (data.selectedTab)
            {
                case 0:
                    DrawFoldersTab(remainingHeight);
                    break;
                case 1:
                    DrawScriptsTab(remainingHeight);
                    break;
            }
        }

        private void DrawHeader(Rect position)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("SF Namespace Generator", styles.headerStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(styles.folderBoxStyle);
            EditorGUILayout.LabelField("Configuration", styles.sectionHeaderStyle);
            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.5f));
            string newRootNamespace = EditorGUILayout.TextField("Root Namespace:", data.rootNamespace);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.5f));
            bool newIncludeFolderStructure = EditorGUILayout.Toggle("Include Folder Structure", data.includeFolderStructure);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            if (newRootNamespace != data.rootNamespace || newIncludeFolderStructure != data.includeFolderStructure)
            {
                data.rootNamespace = newRootNamespace;
                data.includeFolderStructure = newIncludeFolderStructure;
                if (data.filteredScripts != null && data.filteredScripts.Count > 0)
                {
                    data.UpdateGeneratedNamespaces(namespaceProcessor);
                }

                window.Repaint();
            }

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }

        private void DrawTabButtons()
        {
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < data.tabOptions.Length; i++)
            {
                if (i == data.selectedTab)
                {
                    if (GUILayout.Button(data.tabOptions[i], styles.activeTabButtonStyle))
                        data.selectedTab = i;
                }
                else
                {
                    if (GUILayout.Button(data.tabOptions[i], styles.tabButtonStyle))
                        data.selectedTab = i;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFoldersTab(float availableHeight)
        {
            // Start measuring from here
            Rect startRect = EditorGUILayout.GetControlRect(false, 0);

            EditorGUILayout.BeginVertical(styles.folderBoxStyle);
            EditorGUILayout.LabelField("Target Folder", styles.sectionHeaderStyle);
            EditorGUILayout.BeginHorizontal();
            string newFolderPath = EditorGUILayout.TextField("Folder:", data.selectedFolderPath, GUILayout.ExpandWidth(true));
            if (newFolderPath != data.selectedFolderPath)
            {
                data.selectedFolderPath = newFolderPath;
                data.SavePreferences();
            }
            if (GUILayout.Button("Browse", styles.buttonStyle, GUILayout.Width(80)))
                BrowseFolder();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(styles.statusBoxStyle);
            EditorGUILayout.LabelField("Scripts will be scanned from:", EditorStyles.miniBoldLabel);
            EditorGUILayout.LabelField(data.selectedFolderPath, EditorStyles.wordWrappedLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(styles.folderBoxStyle);
            EditorGUILayout.LabelField("Folder Namespace Overrides", styles.sectionHeaderStyle);

            // Get the current position after drawing the previous elements
            Rect currentRect = EditorGUILayout.GetControlRect(false, 0);
            float usedHeight = currentRect.y - startRect.y;

            // Calculate remaining space
            float remainingSpace = availableHeight - usedHeight - 140f; // Reserve 140 for buttons and status

            // Clamp to reasonable values
            remainingSpace = Mathf.Max(remainingSpace, 150f); // Minimum scroll height
            remainingSpace = Mathf.Min(remainingSpace, 600f); // Maximum scroll height

            folderScrollPosition = EditorGUILayout.BeginScrollView(folderScrollPosition,
                GUILayout.Height(remainingSpace),
                GUILayout.ExpandHeight(false)); // Important: Don't expand
            DrawFolderOverrides();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            // Now draw the buttons and status
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = !data.isLoading;
            if (GUILayout.Button("Scan Selected Folder", styles.primaryButtonStyle))
                ScanSelectedFolder();
            if (GUILayout.Button("Expand All", styles.buttonStyle))
                folderTreeManager.ForceExpandAll(data.rootFolderNode, true);
            if (GUILayout.Button("Collapse All", styles.buttonStyle))
                folderTreeManager.ForceExpandAll(data.rootFolderNode, false);
            GUI.enabled = true;

            if (data.isLoading)
            {
                EditorGUILayout.LabelField("Scanning...", EditorStyles.miniLabel);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5f);

            if (data.filteredScripts.Count > 0)
            {
                EditorGUILayout.BeginVertical(styles.successBoxStyle);
                EditorGUILayout.LabelField($"Found {data.filteredScripts.Count} scripts. Switch to the Scripts tab to view and edit them.", EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndVertical();
            }
            else if (!data.isLoading)
            {
                EditorGUILayout.BeginVertical(styles.warningBoxStyle);
                EditorGUILayout.LabelField("No scripts found. Click 'Scan Selected Folder'.", EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndVertical();
            }
        }
        private void DrawScriptsTab(float availableHeight)
        {
            EditorGUILayout.BeginVertical();

            if (data.filteredScripts.Count > 0)
            {
                int selectedCount = data.filteredScripts.Count(s => s.includeInBatch);

                EditorGUILayout.BeginVertical(styles.folderBoxStyle);
                EditorGUILayout.LabelField($"Scripts: {data.filteredScripts.Count} total, {selectedCount} selected", styles.sectionHeaderStyle);
                EditorGUILayout.EndVertical();

                DrawSelectionOptions();

                EditorGUILayout.Space();

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));
                foreach (var group in data.filteredScripts.GroupBy(s => s.folderPath).OrderBy(g => g.Key))
                    DrawFolderGroup(group.Key, group.ToList());
                EditorGUILayout.EndScrollView();

                EditorGUILayout.Space();

                GUILayout.FlexibleSpace();

                string buttonText = selectedCount > 0 ?
                    $"Generate Namespaces for {selectedCount} Scripts" :
                    "Generate Namespaces (No Scripts Selected)";
                bool buttonEnabled = selectedCount > 0;

                GUI.enabled = !data.isLoading && !data.isGenerating;
                if (GUILayout.Button(buttonText, styles.primaryButtonStyle, GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                    GenerateNamespacesForSelectedScripts();
                GUI.enabled = true;

                GUILayout.FlexibleSpace();
            }
            else
            {
                EditorGUILayout.BeginVertical(styles.warningBoxStyle);
                EditorGUILayout.LabelField("No scripts found. Switch to the Folders tab and click 'Scan Selected Folder'.", EditorStyles.wordWrappedLabel);
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }
        private void DrawFolderOverrides()
        {
            EditorGUILayout.BeginVertical(styles.statusBoxStyle);
            EditorGUILayout.LabelField("Override namespaces for folders. Cascades to children.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.EndVertical();

            if (data.rootFolderNode == null)
            {
                EditorGUILayout.LabelField("No folder data available. Please scan a folder first.");
                return;
            }

            if (data.isLoading) EditorGUILayout.LabelField("Loading...");
            else if (data.rootFolderNode.children == null || data.rootFolderNode.children.Count == 0)
                EditorGUILayout.LabelField("No subfolders.");
            else
            {
                foreach (var child in data.rootFolderNode.children.Where(c => c.hasScriptsOrScriptChildren).OrderBy(c => c.name))
                    DrawFolderNode(child, 0);
            }
        }

        private void DrawFolderNode(FolderNode node, int depth)
        {
            if (!node.hasScriptsOrScriptChildren) return;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(depth * 15);

            bool hasChildren = node.children != null && node.children.Any(c => c.hasScriptsOrScriptChildren);

            GUIStyle centeredStyle = new GUIStyle(EditorStyles.label);
            centeredStyle.alignment = TextAnchor.LowerRight;
            centeredStyle.fixedWidth = 20;

            if (hasChildren)
            {
                string buttonText = node.isExpanded ? "▼" : "▶";
                if (GUILayout.Button(buttonText, centeredStyle))
                {
                    node.isExpanded = !node.isExpanded;
                }
            }
            else
            {

                GUILayout.Label("•", centeredStyle);
            }

            EditorGUILayout.LabelField($"📁 {node.name}");

            if (!data.folderOverrides.ContainsKey(node.fullPath))
                data.folderOverrides[node.fullPath] = data.persistentFolderOverrides.ContainsKey(node.fullPath) ?
                    data.persistentFolderOverrides[node.fullPath] : CleanNamespacePart(node.name);

            string current = data.folderOverrides[node.fullPath];
            string newVal = EditorGUILayout.TextField(current, GUILayout.Width(200));

            if (newVal != current)
            {
                data.folderOverrides[node.fullPath] = newVal.Trim();
                data.persistentFolderOverrides[node.fullPath] = string.IsNullOrWhiteSpace(newVal) ? null : newVal;
                data.SavePreferences();
                data.UpdateGeneratedNamespaces(namespaceProcessor);
                window.Repaint();
            }

            if (GUILayout.Button("Reset", styles.buttonStyle, GUILayout.Width(60)))
            {
                data.folderOverrides[node.fullPath] = CleanNamespacePart(node.name);
                data.persistentFolderOverrides[node.fullPath] = data.folderOverrides[node.fullPath];
                data.SavePreferences();
                data.UpdateGeneratedNamespaces(namespaceProcessor);
                window.Repaint();
            }

            EditorGUILayout.EndHorizontal();

            if (node.isExpanded && node.children != null)
            {
                foreach (var child in node.children.Where(c => c.hasScriptsOrScriptChildren).OrderBy(c => c.name))
                    DrawFolderNode(child, depth + 1);
            }
        }

        private void DrawFolderGroup(string folderPath, List<ScriptInfo> scripts)
        {
            string display = folderPath.StartsWith("Assets/") ? folderPath.Substring(7) : folderPath;
            EditorGUILayout.BeginVertical(styles.folderBoxStyle);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"📁 {display}", styles.sectionHeaderStyle);
            bool allSelected = scripts.All(s => s.includeInBatch);
            bool newSel = EditorGUILayout.Toggle(allSelected, GUILayout.Width(20));
            if (newSel != allSelected) scripts.ForEach(s => s.includeInBatch = newSel);
            EditorGUILayout.EndHorizontal();
            foreach (var s in scripts.OrderBy(s => s.fileName)) DrawScriptItem(s);
            EditorGUILayout.EndVertical();
        }

        private void DrawScriptItem(ScriptInfo script)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical(styles.scriptBoxStyle);

            EditorGUILayout.BeginHorizontal();
            script.includeInBatch = EditorGUILayout.Toggle(script.includeInBatch, GUILayout.Width(20));
            EditorGUILayout.LabelField(script.fileName, EditorStyles.boldLabel, GUILayout.MinWidth(150));

            string status;
            Color statusColor;
            if (script.isEditorScript)
            {
                status = "Editor";
                statusColor = styles.secondaryColor;
            }
            else if (script.hasExistingNamespace)
            {
                status = "Has Namespace";
                statusColor = styles.successColor;
            }
            else
            {
                status = "No Namespace";
                statusColor = styles.warningColor;
            }

            Color originalColor = GUI.color;
            GUI.color = statusColor;
            EditorGUILayout.LabelField(status, GUILayout.Width(100));
            GUI.color = originalColor;

            if (script.hasExistingNamespace)
            {
                bool newWantsToUpdate = EditorGUILayout.ToggleLeft("Update", script.wantsToUpdate, GUILayout.Width(60));
                if (newWantsToUpdate != script.wantsToUpdate)
                {
                    script.wantsToUpdate = newWantsToUpdate;
                    if (script.wantsToUpdate)
                    {
                        script.customNamespace = "";
                        script.includeInBatch = true;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(script.customNamespace))
                        {
                            script.includeInBatch = false;
                        }
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Namespace:", GUILayout.Width(80));

            string displayNamespace = "";
            bool isEditable = !string.IsNullOrEmpty(script.customNamespace);

            if (isEditable)
            {
                displayNamespace = script.customNamespace;
            }
            else
            {
                displayNamespace = script.generatedNamespace;
            }

            string newNamespace = EditorGUILayout.TextField(displayNamespace);
            if (newNamespace != displayNamespace)
            {
                if (isEditable)
                {
                    script.customNamespace = newNamespace;
                }
                else
                {
                    script.customNamespace = newNamespace;
                    script.includeInBatch = true;
                }
            }

            if (GUILayout.Button("Reset", styles.buttonStyle, GUILayout.Width(60)))
            {
                script.customNamespace = "";
                if (!script.hasExistingNamespace)
                {
                    script.includeInBatch = false;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSelectionOptions()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All", styles.buttonStyle))
            {
                foreach (var script in data.filteredScripts)
                    script.includeInBatch = true;
            }
            if (GUILayout.Button("Deselect All", styles.buttonStyle))
            {
                foreach (var script in data.filteredScripts)
                    script.includeInBatch = false;
            }
            if (GUILayout.Button("Select Only Without Namespace", styles.buttonStyle))
            {
                foreach (var script in data.filteredScripts)
                    script.includeInBatch = !script.hasExistingNamespace;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void BrowseFolder()
        {
            string path = EditorUtility.OpenFolderPanel("Select Folder", data.selectedFolderPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                path = path.Replace('\\', '/');
                string dataPath = Application.dataPath.Replace('\\', '/');

                if (path.StartsWith(dataPath))
                {
                    data.selectedFolderPath = "Assets" + path.Substring(dataPath.Length);
                    data.SavePreferences();
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Selected folder must be within the Assets folder.", "OK");
                }
            }
        }

        private void ScanSelectedFolder()
        {
            if (string.IsNullOrEmpty(data.selectedFolderPath))
            {
                EditorUtility.DisplayDialog("Error", "Please select a folder to scan.", "OK");
                return;
            }

            data.isLoading = true;
            window.Repaint();

            // Use async operation to prevent UI freezing
            EditorApplication.delayCall += () =>
            {
                try
                {
                    EditorUtility.DisplayProgressBar("Scanning Scripts", "Finding files...", 0f);
                    data.rootFolderNode = folderTreeManager.BuildFolderTree(data.selectedFolderPath);
                    EditorUtility.DisplayProgressBar("Scanning Scripts", "Building folder tree...", 0.1f);
                    data.filteredScripts = namespaceProcessor.ScanScripts(data.selectedFolderPath);

                    var scriptFolders = new HashSet<string>(data.filteredScripts.Select(s => s.folderPath));
                    folderTreeManager.MarkFoldersWithScripts(data.rootFolderNode, scriptFolders);

                    data.InitializeOverrides();
                    data.filteredScripts = data.filteredScripts.OrderBy(s => s.path).ToList();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error scanning folder: {ex.Message}");
                    EditorUtility.DisplayDialog("Error", $"Failed to scan folder: {ex.Message}", "OK");
                }
                finally
                {
                    // Ensure loading state is always reset
                    data.isLoading = false;
                    EditorUtility.ClearProgressBar();
                    window.Repaint();
                }
            };
        }

        private void GenerateNamespacesForSelectedScripts()
        {
            var selectedScripts = data.filteredScripts.Where(s => s.includeInBatch).ToList();
            if (selectedScripts.Count == 0)
            {
                EditorUtility.DisplayDialog("No Scripts Selected",
                    "Please select at least one script to generate namespaces for.", "OK");
                return;
            }

            // Disable UI during async processing
            data.isGenerating = true;
            window.Repaint();

            int total = selectedScripts.Count;
            var changedScripts = new List<ScriptInfo>();
            int successCount = 0;
            int failCount = 0;
            var failedFiles = new List<string>();
            int currentIndex = 0;

            EditorUtility.DisplayProgressBar("Generating Namespaces", "Starting...", 0f);

            // Async processing function
            EditorApplication.CallbackFunction processNext = null;
            processNext = () =>
            {
                if (currentIndex >= total)
                {
                    // Phase 2: Add using statements
                    if (changedScripts.Count > 0)
                    {
                        bool fixUsings = changedScripts.Count > 1 ? EditorUtility.DisplayDialog("Fix Using Statements",
                            $"Add missing using statements between {changedScripts.Count} changed scripts?", "Yes", "No") : true;

                        if (fixUsings)
                        {
                            try
                            {
                                FixAllUsingStatements(changedScripts);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError($"Error fixing using statements: {e.Message}");
                            }
                        }
                    }

                    EditorUtility.ClearProgressBar();

                    // Show results
                    string message = successCount == 0 && failCount == 0 ?
                        "No changes needed. All selected scripts already have the correct namespaces." :
                        $"Namespace generation complete.\n\nUpdated: {successCount} scripts" +
                        (failCount > 0 ? $"\nFailed: {failCount}\n\nFailed files:\n" + string.Join("\n", failedFiles) : "");

                    EditorUtility.DisplayDialog("Generation Complete", message, "OK");
                    AssetDatabase.Refresh();

                    data.isGenerating = false;
                    window.Repaint();
                    return;
                }

                var script = selectedScripts[currentIndex];
                EditorUtility.DisplayProgressBar("Generating Namespaces", $"Processing {script.fileName}", (float)currentIndex / total);

                try
                {
                    string finalNamespace = !string.IsNullOrEmpty(script.customNamespace) ?
                        script.customNamespace : script.generatedNamespace;
                    string content = File.ReadAllText(script.path);

                    if (CodeUpdater.HasNamespace(content))
                    {
                        string existingNamespace = CodeUpdater.ExtractNamespace(content);
                        if (existingNamespace == finalNamespace) return;
                    }

                    string newContent = CodeUpdater.AddNamespace(content, finalNamespace);
                    if (newContent != content)
                    {
                        File.WriteAllText(script.path, newContent);
                        changedScripts.Add(script);
                        successCount++;
                    }
                }
                catch (System.Exception ex)
                {
                    failCount++;
                    failedFiles.Add($"{script.fileName}: {ex.Message}");
                }

                currentIndex++;
                EditorApplication.delayCall += processNext;  // Schedule next script
            };

            // Start processing
            EditorApplication.delayCall += processNext;
        }

        /// <summary>
        /// Fix using statements for all changed scripts
        /// </summary>
        private void FixAllUsingStatements(List<ScriptInfo> changedScripts)
        {
            if (changedScripts == null || changedScripts.Count == 0)
                return;

            // Build type namespace map from ALL changed scripts
            var typeNamespaceMap = BuildTypeNamespaceMapFromAllScripts();

            foreach (var script in changedScripts)
            {
                string scriptName = Path.GetFileNameWithoutExtension(script.fileName);
                string namespaceName = script.customNamespace ?? script.generatedNamespace;

                if (!string.IsNullOrEmpty(namespaceName))
                {
                    // Add the type name
                    typeNamespaceMap[scriptName] = namespaceName;

                    // Always add Attribute version for all scripts (not just those ending with Attribute)
                    // This catches cases where non-Attribute classes are used as attributes
                    string attributeName = scriptName + "Attribute";
                    typeNamespaceMap[attributeName] = namespaceName;

                    // Also check if it's already an Attribute class
                    if (scriptName.EndsWith("Attribute"))
                    {
                        string baseName = scriptName.Substring(0, scriptName.Length - 9);
                        typeNamespaceMap[baseName] = namespaceName;
                    }

                    // Handle generic types
                    if (scriptName.Contains('`'))
                    {
                        string genericName = scriptName.Substring(0, scriptName.IndexOf('`'));
                        typeNamespaceMap[genericName] = namespaceName;
                    }
                }
            }

            int totalUsingsAdded = 0;

            // Analyze each changed script
            foreach (var script in changedScripts)
            {
                try
                {
                    string scriptName = Path.GetFileNameWithoutExtension(script.fileName);
                    string scriptNamespace = script.customNamespace ?? script.generatedNamespace;

                    // Find all referenced types from other changed scripts
                    var requiredUsings = DependencyAnalyzer.FindReferencedTypesInScript(
                        script.path,
                        typeNamespaceMap,
                        scriptNamespace);

                    // Also check for references to ALL project scripts (for single script case)
                    if (changedScripts.Count == 1)
                    {
                        requiredUsings.AddRange(FindReferencesToOtherProjectScripts(script, typeNamespaceMap));
                    }

                    if (requiredUsings.Count > 0)
                    {
                        // Apply usings to this script
                        string content = File.ReadAllText(script.path);
                        string newContent = AddUsingsToContent(content, requiredUsings);

                        if (newContent != content)
                        {
                            File.WriteAllText(script.path, newContent);
                            totalUsingsAdded += requiredUsings.Count;

                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Failed to fix usings for {script.fileName}: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Build type-to-namespace map from ALL scripts in the project.
        /// </summary>
        private Dictionary<string, string> BuildTypeNamespaceMapFromAllScripts()
        {
            var map = new Dictionary<string, string>();
            string[] allScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            foreach (string scriptPath in allScripts)
            {
                try
                {
                    string content = File.ReadAllText(scriptPath);
                    if (CodeUpdater.HasNamespace(content))
                    {
                        string namespaceName = CodeUpdater.ExtractNamespace(content);
                        string scriptName = Path.GetFileNameWithoutExtension(scriptPath);
                        if (!string.IsNullOrEmpty(namespaceName))
                        {
                            // Add the type name
                            map[scriptName] = namespaceName;
                            // Handle attributes
                            if (scriptName.EndsWith("Attribute"))
                            {
                                string baseName = scriptName.Substring(0, scriptName.Length - 9);
                                map[baseName] = namespaceName;
                            }
                            else
                            {
                                string attributeName = scriptName + "Attribute";
                                map[attributeName] = namespaceName;
                            }

                            // Handle generics
                            if (scriptName.Contains('`'))
                            {
                                string genericName = scriptName.Substring(0, scriptName.IndexOf('`'));
                                map[genericName] = namespaceName;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Failed to process {scriptPath}: {e.Message}");
                }
            }
            return map;
        }

        /// <summary>
        /// Find references to other scripts in the project (for single script case)
        /// </summary>
        private List<string> FindReferencesToOtherProjectScripts(ScriptInfo script, Dictionary<string, string> currentTypeMap)
        {
            var requiredUsings = new List<string>();

            try
            {
                string scriptContent = File.ReadAllText(script.path);
                string scriptNamespace = script.customNamespace ?? script.generatedNamespace;

                // Get all other scripts in the project
                string[] allScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

                foreach (string otherScriptPath in allScripts)
                {
                    // Skip self
                    if (otherScriptPath == script.path)
                        continue;

                    try
                    {
                        string otherContent = File.ReadAllText(otherScriptPath);

                        // Check if other script has a namespace
                        if (CodeUpdater.HasNamespace(otherContent))
                        {
                            string otherNamespace = CodeUpdater.ExtractNamespace(otherContent);
                            string otherScriptName = Path.GetFileNameWithoutExtension(otherScriptPath);

                            // Skip if same namespace
                            if (otherNamespace == scriptNamespace)
                                continue;

                            // Skip if already in our current type map
                            if (currentTypeMap.ContainsKey(otherScriptName))
                                continue;

                            // Check if script references this other script
                            if (ScriptReferencesType(script.path, otherScriptName))
                            {
                                string usingStatement = $"using {otherNamespace};";
                                if (!scriptContent.Contains($"using {otherNamespace};"))
                                {
                                    requiredUsings.Add(usingStatement);
                                }
                            }

                            // Also check for generic version
                            if (otherScriptName.Contains('`'))
                            {
                                string genericName = otherScriptName.Substring(0, otherScriptName.IndexOf('`'));
                                if (ScriptReferencesType(script.path, genericName))
                                {
                                    string usingStatement = $"using {otherNamespace};";
                                    if (!scriptContent.Contains($"using {otherNamespace};"))
                                    {
                                        requiredUsings.Add(usingStatement);
                                    }
                                }
                            }
                        }
                    }
                    catch { /* Skip errors */ }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to find project references: {e.Message}");
            }

            return requiredUsings;
        }

        /// <summary>
        /// Helper method to check if a script references a specific type
        /// </summary>
        private bool ScriptReferencesType(string scriptPath, string typeName)
        {
            try
            {
                string content = File.ReadAllText(scriptPath);
                string cleanContent = RemoveCommentsAndStringsForCheck(content);
                return IsTypeReferencedForCheck(cleanContent, typeName);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Simplified type reference check for GUI class
        /// </summary>
        private bool IsTypeReferencedForCheck(string cleanContent, string typeName)
        {
            // Simple check for type name as whole word
            string pattern = $@"\b{Regex.Escape(typeName)}\b";

            if (!Regex.IsMatch(cleanContent, pattern, RegexOptions.IgnoreCase))
                return false;

            // Check for common type usage patterns
            string[] patterns = {
                $@"\b{Regex.Escape(typeName)}\s+\w+\s*[;=,\{{\}}]",           // TypeName variable
                $@"\bnew\s+{Regex.Escape(typeName)}\s*\(",                    // new TypeName(
                $@"typeof\s*\(\s*{Regex.Escape(typeName)}\s*\)",              // typeof(TypeName)
                $@"\[\s*{Regex.Escape(typeName)}\s*",                         // [TypeName
                $@"<\s*{Regex.Escape(typeName)}\s*[>,]",                      // <TypeName>
                $@"\(\s*{Regex.Escape(typeName)}\s*\)",                       // (TypeName)
                $@"\b(?:as|is)\s+{Regex.Escape(typeName)}\b",                 // as/is TypeName
                $@"case\s+{Regex.Escape(typeName)}\b",                        // case TypeName
                $@":\s*{Regex.Escape(typeName)}\b",                           // : TypeName
                $@"\b{Regex.Escape(typeName)}\.",                             // TypeName.
                $@"\.{Regex.Escape(typeName)}\b",                             // .TypeName
                $@"Custom(?:Editor|PropertyDrawer).*{Regex.Escape(typeName)}" // CustomEditor(typeof(TypeName))
            };

            foreach (string p in patterns)
            {
                if (Regex.IsMatch(cleanContent, p, RegexOptions.IgnoreCase))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Simple comment/string removal for GUI class
        /// </summary>
        private string RemoveCommentsAndStringsForCheck(string content)
        {
            // Simple implementation
            var lines = content.Split('\n');
            var cleanLines = new List<string>();

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (!trimmed.StartsWith("//"))
                {
                    cleanLines.Add(line);
                }
            }

            return string.Join("\n", cleanLines);
        }
        /// <summary>
        /// Helper method to add usings to content
        /// </summary>
        private string AddUsingsToContent(string content, List<string> requiredUsings)
        {
            if (requiredUsings.Count == 0)
                return content;

            var lines = content.Split('\n').ToList();
            var existingUsings = new List<string>();

            // Extract existing usings
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();

                if (string.IsNullOrWhiteSpace(trimmed))
                    continue;

                if (trimmed.StartsWith("//") || trimmed.StartsWith("/*"))
                    continue;

                if (trimmed.StartsWith("using ") && trimmed.EndsWith(";"))
                {
                    existingUsings.Add(trimmed);
                }
                else
                {
                    break;
                }
            }

            // Filter out already existing usings
            var usingsToAdd = new List<string>();
            foreach (string newUsing in requiredUsings)
            {
                string newNamespace = newUsing.Substring(6).TrimEnd(';').Trim();
                bool exists = false;

                foreach (string existingUsing in existingUsings)
                {
                    string existingNamespace = existingUsing.Substring(6).TrimEnd(';').Trim();

                    if (existingNamespace == newNamespace ||
                        newNamespace.StartsWith(existingNamespace + "."))
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    usingsToAdd.Add(newUsing);
                    existingUsings.Add(newUsing);
                }
            }

            if (usingsToAdd.Count == 0)
                return content;

            // Sort usings
            usingsToAdd.Sort();

            // Find insertion point
            int insertIndex = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                string trimmed = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("//"))
                    continue;

                if (trimmed.StartsWith("using ") && trimmed.EndsWith(";"))
                {
                    insertIndex = i + 1;
                }
                else
                {
                    break;
                }
            }

            // Insert new usings
            for (int i = 0; i < usingsToAdd.Count; i++)
            {
                lines.Insert(insertIndex + i, usingsToAdd[i]);
            }

            return string.Join("\n", lines);
        }

        private string CleanNamespacePart(string name)
        {
            if (string.IsNullOrEmpty(name)) return "Default";
            string clean = name.Trim('_', ' ', '-', '.');

            if (new[] { "Assets", "Scripts", "Editor", "Resources", "Plugins" }
                .Contains(clean, StringComparer.OrdinalIgnoreCase))
                return "";

            clean = Regex.Replace(clean, @"[^a-zA-Z0-9_]", " ");
            clean = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(clean).Replace(" ", "");

            if (string.IsNullOrEmpty(clean))
                clean = Regex.Replace(name, @"[^a-zA-Z0-9]", "") ?? "Folder";

            return clean.Length > 0 && !char.IsLetter(clean[0]) ? "N" + clean : clean;
        }
    }
}