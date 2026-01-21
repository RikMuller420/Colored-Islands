#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Processes and applies using statements to scripts (NO BACKUPS)
    /// </summary>
    public static class UsingStatementProcessor
    {
        /// <summary>
        /// Process changed scripts and add missing usings ONLY between them
        /// </summary>
        public static void ProcessChangedScripts(List<ScriptInfo> changedScripts)
        {
            if (changedScripts == null || changedScripts.Count == 0)
                return;

            // Build type namespace map from ALL changed scripts
            var typeNamespaceMap = BuildTypeNamespaceMap(changedScripts);

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
                    Debug.LogError($"Failed to fix usings for {script.fileName}: {e.Message}\n{e.StackTrace}");
                }
            }

            if (totalUsingsAdded > 0)
            {
                AssetDatabase.Refresh();
            }
        }

        private static Dictionary<string, string> BuildTypeNamespaceMap(List<ScriptInfo> changedScripts)
        {
            var map = new Dictionary<string, string>();

            foreach (var script in changedScripts)
            {
                string scriptName = Path.GetFileNameWithoutExtension(script.fileName);
                string namespaceName = script.customNamespace ?? script.generatedNamespace;

                if (!string.IsNullOrEmpty(namespaceName))
                {
                    // Always add both versions for EVERY type
                    map[scriptName] = namespaceName;

                    // Add Attribute version
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

                    // Handle generic types
                    if (scriptName.Contains('`'))
                    {
                        string genericName = scriptName.Substring(0, scriptName.IndexOf('`'));
                        map[genericName] = namespaceName;
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// Apply using statements to scripts (NO BACKUPS)
        /// </summary>
        private static int ApplyUsingStatements(Dictionary<string, List<string>> scriptUsings)
        {
            int updatedCount = 0;

            foreach (var kvp in scriptUsings)
            {
                string scriptPath = kvp.Key;
                List<string> requiredUsings = kvp.Value;

                if (ApplyUsingStatementsToFile(scriptPath, requiredUsings))
                {
                    updatedCount++;
                }
            }

            return updatedCount;
        }

        /// <summary>
        /// Apply using statements to a single file (NO BACKUPS)
        /// </summary>
        private static bool ApplyUsingStatementsToFile(string filePath, List<string> requiredUsings)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                string newContent = AddUsingsToContent(content, requiredUsings);

                if (newContent != content)
                {
                    // NO BACKUP - directly write the file
                    File.WriteAllText(filePath, newContent);
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to update usings in {Path.GetFileName(filePath)}: {e.Message}");
            }

            return false;
        }

        /// <summary>
        /// Add using statements to file content
        /// </summary>
        private static string AddUsingsToContent(string content, List<string> requiredUsings)
        {
            if (requiredUsings.Count == 0)
                return content;

            var lines = content.Split('\n').ToList();
            var existingUsings = ExtractExistingUsings(lines);

            // Filter out already existing usings
            var usingsToAdd = new List<string>();
            foreach (string newUsing in requiredUsings)
            {
                if (!IsUsingAlreadyPresent(existingUsings, newUsing))
                {
                    usingsToAdd.Add(newUsing);
                }
            }

            if (usingsToAdd.Count == 0)
                return content;

            // Sort usings
            usingsToAdd = SortUsings(usingsToAdd);

            // Find insertion point
            int insertIndex = FindInsertIndex(lines);

            // Insert new usings
            for (int i = 0; i < usingsToAdd.Count; i++)
            {
                lines.Insert(insertIndex + i, usingsToAdd[i]);
            }

            return string.Join("\n", lines);
        }

        /// <summary>
        /// Sort usings with system namespaces first
        /// </summary>
        private static List<string> SortUsings(List<string> usings)
        {
            return usings.OrderBy(u =>
            {
                string ns = u.Substring(6).TrimEnd(';').Trim();
                if (ns.StartsWith("System."))
                    return "A" + ns;
                else if (ns == "System")
                    return "B" + ns;
                else if (ns.StartsWith("UnityEngine."))
                    return "C" + ns;
                else if (ns == "UnityEngine")
                    return "D" + ns;
                else if (ns.StartsWith("UnityEditor."))
                    return "E" + ns;
                else if (ns == "UnityEditor")
                    return "F" + ns;
                else
                    return "G" + ns;
            }).ToList();
        }

        /// <summary>
        /// Extract existing using statements from lines
        /// </summary>
        private static List<string> ExtractExistingUsings(List<string> lines)
        {
            var usings = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();

                // Skip empty lines and comments when looking for usings
                if (string.IsNullOrWhiteSpace(trimmed))
                    continue;

                if (trimmed.StartsWith("//") || trimmed.StartsWith("/*"))
                    continue;

                if (trimmed.StartsWith("using ") && trimmed.EndsWith(";"))
                {
                    usings.Add(trimmed);
                }
                else
                {
                    // Found non-using line, stop searching
                    break;
                }
            }

            return usings;
        }

        /// <summary>
        /// Check if a using statement is already present
        /// </summary>
        private static bool IsUsingAlreadyPresent(List<string> existingUsings, string newUsing)
        {
            string newNamespace = newUsing.Substring(6).TrimEnd(';').Trim();

            foreach (string existingUsing in existingUsings)
            {
                string existingNamespace = existingUsing.Substring(6).TrimEnd(';').Trim();

                // Exact match
                if (existingNamespace == newNamespace)
                    return true;

                // If new namespace is child of existing namespace
                if (newNamespace.StartsWith(existingNamespace + "."))
                    return true;

                // If existing namespace is child of new namespace
                // (we might still need the more specific using)
                if (existingNamespace.StartsWith(newNamespace + "."))
                {
                    // Keep both if they're different
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Find where to insert new using statements
        /// </summary>
        private static int FindInsertIndex(List<string> lines)
        {
            int lastUsingIndex = -1;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();

                // Skip empty lines and comments
                if (string.IsNullOrWhiteSpace(trimmed))
                    continue;

                if (trimmed.StartsWith("//"))
                    continue;

                if (trimmed.StartsWith("/*"))
                {
                    // Skip multi-line comments
                    while (i < lines.Count && !trimmed.Contains("*/"))
                    {
                        i++;
                        if (i < lines.Count)
                        {
                            line = lines[i];
                            trimmed = line.Trim();
                        }
                    }
                    continue;
                }

                if (trimmed.StartsWith("using ") && trimmed.EndsWith(";"))
                {
                    lastUsingIndex = i;
                }
                else
                {
                    // Found non-using line
                    break;
                }
            }

            // If no existing usings, insert after any header comments
            if (lastUsingIndex == -1)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    string trimmed = lines[i].Trim();

                    // Skip empty lines and single-line comments
                    if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith("//"))
                        continue;

                    // Skip multi-line comment start
                    if (trimmed.StartsWith("/*"))
                    {
                        while (i < lines.Count && !trimmed.Contains("*/"))
                        {
                            i++;
                            if (i < lines.Count)
                                trimmed = lines[i].Trim();
                        }
                        continue;
                    }

                    // Found first non-comment line
                    return i;
                }

                return 0; // Empty file
            }

            return lastUsingIndex + 1;
        }
    }
}
#endif