#if UNITY_EDITOR
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
    /// Analyzes dependencies only between scripts that were namespaced
    /// </summary>
    public class DependencyAnalyzer
    {
        /// <summary>
        /// Quick method to check if a script references a specific type
        /// </summary>
        public static bool ScriptReferencesType(string scriptPath, string typeName)
        {
            try
            {
                string content = File.ReadAllText(scriptPath);
                string cleanContent = RemoveCommentsAndStrings(content);
                return IsTypeReferenced(cleanContent, typeName);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Analyze ONLY the changed scripts for dependencies between them
        /// </summary>
        public static Dictionary<string, List<string>> AnalyzeChangedScriptDependencies(
            List<ScriptInfo> changedScripts)
        {
            var result = new Dictionary<string, List<string>>();

            if (changedScripts == null || changedScripts.Count == 0)
                return result;

            // Build type-to-namespace map ONLY from changed scripts
            var typeNamespaceMap = BuildTypeNamespaceMap(changedScripts);

            // Analyze each changed script for dependencies on OTHER changed scripts
            foreach (var script in changedScripts)
            {
                try
                {
                    string scriptName = Path.GetFileNameWithoutExtension(script.fileName);
                    string scriptNamespace = script.customNamespace ?? script.generatedNamespace;

                    // Find dependencies this script has on other changed scripts
                    var dependencies = FindDependenciesOnOtherScripts(
                        script.path,
                        scriptName,
                        scriptNamespace,
                        typeNamespaceMap);

                    if (dependencies.Count > 0)
                    {
                        result[script.path] = dependencies;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Failed to analyze {script.fileName}: {e.Message}");
                }
            }

            return result;
        }

        /// <summary>
        /// Build type-to-namespace map ONLY from changed scripts
        /// </summary>
        private static Dictionary<string, string> BuildTypeNamespaceMap(List<ScriptInfo> changedScripts)
        {
            var map = new Dictionary<string, string>();

            foreach (var script in changedScripts)
            {
                string scriptName = Path.GetFileNameWithoutExtension(script.fileName);
                string namespaceName = script.customNamespace ?? script.generatedNamespace;

                if (!string.IsNullOrEmpty(namespaceName))
                {
                    // Add the type name
                    map[scriptName] = namespaceName;

                    // Handle attributes (both with and without "Attribute" suffix)
                    if (scriptName.EndsWith("Attribute"))
                    {
                        string baseName = scriptName.Substring(0, scriptName.Length - 9);
                        if (!map.ContainsKey(baseName))
                            map[baseName] = namespaceName;
                    }
                    else
                    {
                        string attributeName = scriptName + "Attribute";
                        if (!map.ContainsKey(attributeName))
                            map[attributeName] = namespaceName;
                    }

                    // Handle generic types (without the `1, `2 suffix)
                    if (scriptName.Contains('`'))
                    {
                        string genericName = scriptName.Substring(0, scriptName.IndexOf('`'));
                        if (!map.ContainsKey(genericName))
                        {
                            map[genericName] = namespaceName;
                        }
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// Find dependencies this script has on OTHER changed scripts
        /// </summary>
        private static List<string> FindDependenciesOnOtherScripts(
            string scriptPath,
            string currentScriptName,
            string currentScriptNamespace,
            Dictionary<string, string> typeNamespaceMap)
        {
            var requiredUsings = new List<string>();

            try
            {
                string content = File.ReadAllText(scriptPath);
                string cleanContent = RemoveCommentsAndStrings(content);

                // First, check if the script already has the necessary using statements
                var existingUsings = ExtractExistingUsings(content);

                foreach (var kvp in typeNamespaceMap)
                {
                    string typeName = kvp.Key;
                    string namespaceName = kvp.Value;

                    // Skip if this is the script's own type
                    if (typeName == currentScriptName ||
                        (typeName + "Attribute") == currentScriptName ||
                        typeName == (currentScriptName + "Attribute"))
                        continue;

                    // Skip if namespace is the same (no using needed)
                    if (namespaceName == currentScriptNamespace)
                        continue;

                    // Check if using already exists
                    if (HasUsingForNamespace(existingUsings, namespaceName))
                        continue;

                    // Check if this type is referenced in the script
                    if (IsTypeReferenced(cleanContent, typeName))
                    {
                        string usingStatement = $"using {namespaceName};";
                        if (!requiredUsings.Contains(usingStatement))
                        {
                            requiredUsings.Add(usingStatement);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Error analyzing {Path.GetFileName(scriptPath)}: {e.Message}");
            }

            return requiredUsings;
        }

        /// <summary>
        /// Check if a type is referenced in the content
        /// </summary>
        private static bool IsTypeReferenced(string cleanContent, string typeName)
        {
            // Escape regex special characters
            string escapedTypeName = Regex.Escape(typeName);

            // Check if type is already in a using statement
            string usingPattern = $@"using\s+[^;]*\.{escapedTypeName}\s*;";
            if (Regex.IsMatch(cleanContent, usingPattern, RegexOptions.IgnoreCase))
                return false;

            // Build ALL patterns that could match type usage
            List<string> patterns = new List<string>
            {
                // 1. Basic word boundary pattern
                $@"\b{escapedTypeName}\b",

                // 2. "is" operator patterns
                $@"\bis\s+{escapedTypeName}\b",                 
                $@"\bis\s+{escapedTypeName}Attribute\b",         

                // 3. "as" operator patterns
                $@"\bas\s+{escapedTypeName}\b",                  
                $@"\bas\s+{escapedTypeName}Attribute\b",        

                // 4. typeof patterns
                $@"typeof\s*\(\s*{escapedTypeName}\s*\)",          
                $@"typeof\s*\(\s*{escapedTypeName}Attribute\s*\)", 

                // 5. Attribute patterns
                $@"\[\s*{escapedTypeName}\s*",                    
                $@"\[\s*{escapedTypeName}Attribute\s*",            

                // 6. Custom attribute patterns
                $@"CustomPropertyDrawer\s*\(\s*typeof\s*\(\s*{escapedTypeName}\s*\)\s*\)",
                $@"CustomEditor\s*\(\s*typeof\s*\(\s*{escapedTypeName}\s*\)\s*\)",
                $@"CustomPropertyDrawer\s*\(\s*typeof\s*\(\s*{escapedTypeName}Attribute\s*\)\s*\)",
                $@"CustomEditor\s*\(\s*typeof\s*\(\s*{escapedTypeName}Attribute\s*\)\s*\)",

                // 7. Cast patterns
                $@"\(\s*{escapedTypeName}\s*\)",                     
                $@"\(\s*{escapedTypeName}Attribute\s*\)",          

                // 8. Generic patterns
                $@"<\s*{escapedTypeName}\s*>",                   
                $@"<\s*{escapedTypeName}Attribute\s*>",          
                $@"\b{escapedTypeName}\s*<",                       
                $@"\b{escapedTypeName}Attribute\s*<",          

                // 9. Variable declaration patterns
                $@"\b{escapedTypeName}\s+\w+\s*[;=,\{{\}}\(\)]",    
                $@"\b{escapedTypeName}Attribute\s+\w+\s*[;=,\{{\}}\(\)]",

                // 10. Constructor patterns
                $@"\bnew\s+{escapedTypeName}\s*\(",                
                $@"\bnew\s+{escapedTypeName}Attribute\s*\(",      

                // 11. Inheritance patterns
                $@":\s*{escapedTypeName}\b",                       
                $@":\s*{escapedTypeName}Attribute\b",             

                // 12. Method parameter patterns
                $@"\(\s*{escapedTypeName}\s+\w+\s*\)",             
                $@"\(\s*{escapedTypeName}Attribute\s+\w+\s*\)",    

                // 13. Switch case patterns
                $@"case\s+{escapedTypeName}\b",                    
                $@"case\s+{escapedTypeName}Attribute\b",

                // 14. Member access patterns
                $@"\b{escapedTypeName}\s*\.",
                $@"\b{escapedTypeName}Attribute\s*\.",

                // 15. Method call patterns with the type
                $@"\b\w+\s*<\s*{escapedTypeName}\s*>\(?",        
                $@"\b\w+\s*<\s*{escapedTypeName}Attribute\s*>\(?"
            };

            return false;
        }

        /// <summary>
        /// Extract existing using statements from content
        /// </summary>
        private static List<string> ExtractExistingUsings(string content)
        {
            var usings = new List<string>();
            var lines = content.Split('\n');

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("using ") && trimmed.EndsWith(";"))
                {
                    usings.Add(trimmed);
                }
            }

            return usings;
        }

        /// <summary>
        /// Check if a using statement exists for a namespace
        /// </summary>
        private static bool HasUsingForNamespace(List<string> existingUsings, string namespaceName)
        {
            foreach (string usingStmt in existingUsings)
            {
                // Extract namespace from "using Some.Namespace;"
                string ns = usingStmt.Substring(6).TrimEnd(';').Trim();

                if (ns == namespaceName || namespaceName.StartsWith(ns + "."))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Remove comments and string literals from content
        /// </summary>
        private static string RemoveCommentsAndStrings(string content)
        {
            var result = new System.Text.StringBuilder();
            bool inSingleLineComment = false;
            bool inMultiLineComment = false;
            bool inString = false;
            bool inChar = false;
            char stringChar = '"';

            for (int i = 0; i < content.Length; i++)
            {
                char current = content[i];
                char next = i + 1 < content.Length ? content[i + 1] : '\0';

                if (inSingleLineComment)
                {
                    if (current == '\n')
                        inSingleLineComment = false;
                    continue;
                }

                if (inMultiLineComment)
                {
                    if (current == '*' && next == '/')
                    {
                        inMultiLineComment = false;
                        i++;
                    }
                    continue;
                }

                if (inString)
                {
                    if (current == '\\' && next == stringChar)
                    {
                        i++;
                        continue;
                    }

                    if (current == stringChar)
                        inString = false;
                    continue;
                }

                if (inChar)
                {
                    if (current == '\\' && next == '\'')
                    {
                        i++;
                        continue;
                    }

                    if (current == '\'')
                        inChar = false;
                    continue;
                }

                // Check for comment starts
                if (current == '/' && next == '/')
                {
                    inSingleLineComment = true;
                    i++;
                    continue;
                }

                if (current == '/' && next == '*')
                {
                    inMultiLineComment = true;
                    i++;
                    continue;
                }

                // Check for string/char literals
                if (current == '"')
                {
                    inString = true;
                    stringChar = '"';
                    continue;
                }

                if (current == '\'')
                {
                    inChar = true;
                    continue;
                }

                // Add the character to result
                result.Append(current);
            }

            return result.ToString();
        }

        /// <summary>
        /// Enhanced analysis for a single script
        /// </summary>
        public static List<string> FindReferencedTypesInScript(
            string scriptPath,
            Dictionary<string, string> typeNamespaceMap,
            string currentScriptNamespace)
        {
            var requiredUsings = new List<string>();

            try
            {
                string content = File.ReadAllText(scriptPath);
                string cleanContent = RemoveCommentsAndStrings(content);

                // Get existing usings
                var existingUsings = ExtractExistingUsings(content);

                foreach (var kvp in typeNamespaceMap)
                {
                    string typeName = kvp.Key;
                    string namespaceName = kvp.Value;

                    // Skip if namespace is the same (no using needed)
                    if (namespaceName == currentScriptNamespace)
                        continue;

                    // Skip if using already exists
                    if (HasUsingForNamespace(existingUsings, namespaceName))
                        continue;

                    // Check if this type is referenced in the script
                    if (IsTypeReferenced(cleanContent, typeName))
                    {
                        string usingStatement = $"using {namespaceName};";
                        if (!requiredUsings.Contains(usingStatement))
                        {
                            requiredUsings.Add(usingStatement);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to analyze {scriptPath}: {e.Message}");
            }

            return requiredUsings;
        }
    }
}
#endif