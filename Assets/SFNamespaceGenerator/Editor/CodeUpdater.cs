using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Provides methods to add or update namespaces in C# code files.
    /// </summary>
    public static class CodeUpdater
    {
        /// <summary>
        /// Adds a namespace declaration to the provided code content.
        /// This method is now robust against malformed input where attributes might be outside the namespace.
        /// It first normalizes the file by removing any existing namespaces, then wraps the content in the new namespace.
        /// </summary>
        public static string AddNamespace(string content, string namespaceName)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            content = NormalizeLineEndings(content);

            // Check if namespace already exists and update it if it's the same.
            if (HasNamespace(content))
            {
                string existingNamespace = ExtractNamespace(content);
                if (existingNamespace == namespaceName)
                {
                    return NormalizeLineEndings(content);  // <-- Always normalize
                }
                return UpdateNamespace(content, namespaceName, existingNamespace);
            }

            // If no namespace exists, we need to add one.
            // First, remove any existing, potentially malformed, namespaces to ensure a clean slate.
            string contentWithoutNamespace = RemoveAllNamespaces(content);

            // Split the flattened content into lines to find the end of the 'using' statements.
            string[] lines = contentWithoutNamespace.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            // Find the last 'using' statement to determine where the code block starts.
            int lastUsingIndex = -1;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Trim().StartsWith("using "))
                {
                    lastUsingIndex = i;
                }
                else if (!string.IsNullOrWhiteSpace(lines[i].Trim()) && !lines[i].Trim().StartsWith("//"))
                {
                    // Stop at the first non-empty, non-using, non-comment line.
                    break;
                }
            }

            StringBuilder result = new StringBuilder();

            // Add all 'using' statements and any preceding whitespace/comments.
            if (lastUsingIndex >= 0)
            {
                for (int i = 0; i <= lastUsingIndex; i++)
                {
                    result.AppendLine(lines[i]);
                }
                // Add a blank line after the last 'using' statement for separation.
                result.AppendLine();
            }
            else
            {
                // If there are no 'using' statements, just add a blank line at the top.
                result.AppendLine();
            }

            // Add the new namespace declaration.
            result.AppendLine($"namespace {namespaceName}");
            result.AppendLine("{");

            // Add the rest of the file (attributes, classes, etc.), indented by one level.
            int firstCodeLineIndex = lastUsingIndex + 1;
            for (int i = firstCodeLineIndex; i < lines.Length; i++)
            {
                string line = lines[i];
                // Indent non-empty lines.
                if (!string.IsNullOrEmpty(line))
                {
                    result.Append("\t");
                }
                result.AppendLine(line);
            }

            // Close the namespace.
            result.AppendLine("}");

            return result.ToString();
        }

        /// <summary>
        /// Updates an existing namespace declaration in the provided code content.
        /// Also updates any using statements that reference the old namespace.
        /// </summary>
        public static string UpdateNamespace(string content, string newNamespace, string oldNamespace)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(oldNamespace) || newNamespace == oldNamespace)
                return content;

            content = NormalizeLineEndings(content);
            string lineEnding = "\r\n";

            // Extract the old root namespace (e.g., "YourCompany.YourGame" from "YourCompany.YourGame.Runtime.UI.Variableui")
            string oldRootNamespace = ExtractRootNamespace(oldNamespace);
            string newRootNamespace = ExtractRootNamespace(newNamespace);

            // Better pattern that handles more variations
            // This pattern matches: namespace OldNamespace { (with optional whitespace/comments)
            string pattern = $@"(^\s*namespace\s+){Regex.Escape(oldNamespace)}((?:\s*(?://[^\n]*)?\s*[\{{]?\s*\r?\n?))";
            string replacement = $"namespace {newNamespace}{lineEnding}{{";

            string result = Regex.Replace(content, pattern, replacement, RegexOptions.Multiline);

            // If regex replacement didn't work, try manual replacement
            if (result == content)
            {
                result = ManualUpdateNamespace(content, newNamespace, oldNamespace);
            }

            // NEW: Update using statements that reference the old root namespace
            if (!string.IsNullOrEmpty(oldRootNamespace) && oldRootNamespace != newRootNamespace)
            {
                result = UpdateUsingStatements(result, oldRootNamespace, newRootNamespace);
            }

            return result;
        }

        /// <summary>
        /// Extracts the root namespace from a full namespace (e.g., "YourCompany.YourGame" from "YourCompany.YourGame.Runtime.UI.Variableui").
        /// Assumes the root is the first two parts separated by '.'.
        /// </summary>
        private static string ExtractRootNamespace(string fullNamespace)
        {
            var parts = fullNamespace.Split('.');
            return parts.Length >= 2 ? $"{parts[0]}.{parts[1]}" : fullNamespace;
        }

        /// <summary>
        /// Updates using statements that start with the old root namespace.
        /// </summary>
        private static string UpdateUsingStatements(string content, string oldRootNamespace, string newRootNamespace)
        {
            // Pattern to match: using OldRootNamespace...;
            string usingPattern = $@"(^\s*using\s+){Regex.Escape(oldRootNamespace)}(\.[^;]*;)(?:\s*(?://[^\n]*)?\s*\r?\n?)";
            string replacement = $"using {newRootNamespace}$2";

            return Regex.Replace(content, usingPattern, replacement, RegexOptions.Multiline);
        }

        /// <summary>
        /// Manual namespace update as fallback for regex failures
        /// </summary>
        private static string ManualUpdateNamespace(string content, string newNamespace, string oldNamespace)
        {
            string[] lines = content.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int namespaceIndex = line.IndexOf("namespace ");
                if (namespaceIndex >= 0)
                {
                    // Found a namespace line
                    string afterNamespace = line.Substring(namespaceIndex + "namespace ".Length);

                    // Check if this line contains the old namespace
                    if (afterNamespace.TrimStart().StartsWith(oldNamespace))
                    {
                        // Replace the old namespace with new one
                        string beforeNamespace = line.Substring(0, namespaceIndex + "namespace ".Length);
                        string afterOldNamespace = afterNamespace.Substring(afterNamespace.IndexOf(oldNamespace) + oldNamespace.Length);

                        lines[i] = beforeNamespace + newNamespace + afterOldNamespace;
                        break;
                    }
                }
            }

            return string.Join("\r\n", lines);
        }

        /// <summary>
        /// Checks if the content already has a namespace declaration.
        /// </summary>
        public static bool HasNamespace(string content)
        {
            // Remove comments to avoid false positives
            string cleanContent = RemoveCommentsAndStrings(content);

            // Look for namespace keyword
            int namespaceIndex = cleanContent.IndexOf("namespace ");
            if (namespaceIndex < 0)
                return false;

            // Check if it's at the start of a line or after whitespace
            string before = cleanContent.Substring(0, namespaceIndex);
            if (!string.IsNullOrWhiteSpace(before) && !before.EndsWith("\n"))
                return false;

            return true;
        }

        /// <summary>
        /// Extracts the existing namespace from the content.
        /// </summary>
        public static string ExtractNamespace(string content)
        {
            string cleanContent = RemoveCommentsAndStrings(content);

            // Find namespace declaration
            int start = cleanContent.IndexOf("namespace ");
            if (start < 0)
                return string.Empty;

            start += "namespace ".Length;

            // Find the end of the namespace name
            int end = start;
            while (end < cleanContent.Length &&
                   (char.IsLetterOrDigit(cleanContent[end]) ||
                    cleanContent[end] == '.' ||
                    cleanContent[end] == '_'))
            {
                end++;
            }

            if (end > start)
                return cleanContent.Substring(start, end - start).Trim();

            return string.Empty;
        }

        /// <summary>
        /// Normalizes line endings to \r\n for consistent processing.
        /// </summary>
        public static string NormalizeLineEndings(string content)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            // First replace \r\n with \n (in case of mixed endings)
            content = content.Replace("\r\n", "\n");
            // Then replace \r with \n (for old Mac line endings)
            content = content.Replace("\r", "\n");
            // Finally replace \n with \r\n (standard Windows line endings)
            return content.Replace("\n", "\r\n");
        }

        /// <summary>
        /// Removes comments and string literals from the content to avoid false matches.
        /// </summary>
        private static string RemoveCommentsAndStrings(string content)
        {
            // Remove string literals
            content = Regex.Replace(content, @"""[^""\\]*(?:\\.[^""\\]*)*""", "\"\"");
            content = Regex.Replace(content, @"'[^'\\]*(?:\\.[^'\\]*)*'", "''");

            // Remove single line comments
            content = Regex.Replace(content, @"//.*", "");

            // Remove multi-line comments
            content = Regex.Replace(content, @"/\*.*?\*/", "", RegexOptions.Singleline);

            return content;
        }

        /// <summary>
        /// Removes all top-level namespace declarations from the content, effectively "flattening" the file.
        /// It correctly handles brace matching to ensure only the namespace block is removed.
        /// </summary>
        private static string RemoveAllNamespaces(string content)
        {
            // We use a recursive approach to handle multiple, sequential top-level namespaces.
            string currentContent = content;
            while (true)
            {
                int startIndex = currentContent.IndexOf("namespace ");
                if (startIndex == -1)
                {
                    // No more namespaces found, return the flattened content.
                    return currentContent;
                }

                // Find the opening brace for the namespace declaration.
                int openBraceIndex = currentContent.IndexOf('{', startIndex);
                if (openBraceIndex == -1) return currentContent; // Malformed, return as is.

                // Find the matching closing brace by tracking depth.
                int depth = 1;
                int searchIndex = openBraceIndex + 1;
                int closeBraceIndex = -1;
                while (depth > 0 && searchIndex < currentContent.Length)
                {
                    if (currentContent[searchIndex] == '{') depth++;
                    if (currentContent[searchIndex] == '}') depth--;
                    if (depth == 0)
                    {
                        closeBraceIndex = searchIndex;
                        break;
                    }
                    searchIndex++;
                }

                if (closeBraceIndex == -1) return currentContent; // Malformed, return as is.

                // Extract the parts of the file.
                string beforeNamespace = currentContent.Substring(0, startIndex);
                string insideNamespace = currentContent.Substring(openBraceIndex + 1, closeBraceIndex - openBraceIndex - 1);
                string afterNamespace = currentContent.Substring(closeBraceIndex + 1);

                // De-indent the content that was inside the namespace by one level.
                string[] insideLines = insideNamespace.Split(new[] { "\r\n" }, StringSplitOptions.None);
                var flattenedInside = new StringBuilder();
                foreach (var line in insideLines)
                {
                    // Only remove one level of tab indentation.
                    if (line.StartsWith("\t"))
                    {
                        flattenedInside.AppendLine(line.Substring(1));
                    }
                    else
                    {
                        flattenedInside.AppendLine(line);
                    }
                }

                // Reconstruct the content without the namespace wrapper.
                currentContent = beforeNamespace + flattenedInside.ToString() + afterNamespace;
            }
        }
    }
}