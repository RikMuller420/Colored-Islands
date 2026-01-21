using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Microsoft.CodeAnalysis;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Handles namespace generation logic
    /// </summary>
    public class NamespaceGenerator
    {
        private string rootNamespace;
        private bool includeFolderStructure;
        private string selectedFolderPath;
        private Dictionary<string, string> folderOverrides;

        public NamespaceGenerator(string rootNamespace, bool includeFolderStructure,
                                 string selectedFolderPath, Dictionary<string, string> folderOverrides)
        {
            this.rootNamespace = rootNamespace;
            this.includeFolderStructure = includeFolderStructure;
            this.selectedFolderPath = selectedFolderPath;
            this.folderOverrides = folderOverrides ?? new Dictionary<string, string>();
        }

        public string GenerateNamespaceName(string scriptPath)
        {
            if (!includeFolderStructure) return rootNamespace;

            string dir = Path.GetDirectoryName(scriptPath).Replace('\\', '/');
            if (!dir.StartsWith(selectedFolderPath)) return rootNamespace;

            var parts = new List<string>();
            string current = selectedFolderPath;
            string relativeFolders = dir.Substring(selectedFolderPath.Length).Trim('/');

            if (string.IsNullOrEmpty(relativeFolders))
                return rootNamespace;

            foreach (string folder in relativeFolders.Split('/'))
            {
                current = Path.Combine(current, folder).Replace('\\', '/');

                string namespacePart;
                if (folderOverrides.TryGetValue(current, out string overrideValue))
                {
                    namespacePart = overrideValue;
                }
                else
                {
                    namespacePart = CleanNamespacePart(folder);
                }

                if (!string.IsNullOrWhiteSpace(namespacePart))
                {
                    parts.Add(namespacePart);
                }
            }

            return parts.Count == 0 ? rootNamespace : $"{rootNamespace}.{string.Join(".", parts)}";
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
