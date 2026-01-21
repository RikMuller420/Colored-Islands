using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Parity.NamespaceGenerator.Editor
{
    /// <summary>
    /// Manages the folder tree structure for the namespace generator.
    /// </summary>
    public class FolderTreeManager
    {
        /// <summary>
        /// Builds a hierarchical tree structure of folders starting from the specified root path.
        /// </summary>
        public FolderNode BuildFolderTree(string rootPath)
        {
            if (!Directory.Exists(rootPath))
            {
                Debug.LogError($"Folder not found: {rootPath}");
                return null;
            }

            FolderNode root = new FolderNode
            {
                name = Path.GetFileName(rootPath),
                fullPath = rootPath
            };

            BuildFolderTreeRecursive(Path.GetFullPath(rootPath), root);
            return root;
        }

        /// <summary>
        /// Marks folders in the tree that contain scripts or have children that contain scripts.
        /// </summary>
        public void MarkFoldersWithScripts(FolderNode root, HashSet<string> foldersWithScripts)
        {
            if (root == null) return;

            foreach (var child in root.children)
            {
                MarkFoldersWithScripts(child, foldersWithScripts);
            }

            root.hasScriptsOrScriptChildren =
                foldersWithScripts.Contains(root.fullPath) ||
                root.children.Any(c => c.hasScriptsOrScriptChildren);
        }

        /// <summary>
        /// Recursively builds the folder tree structure.
        /// </summary>
        private void BuildFolderTreeRecursive(string currentPath, FolderNode currentNode)
        {
            try
            {
                string[] subDirectories = Directory.GetDirectories(currentPath);

                foreach (string subDir in subDirectories)
                {
                    string folderName = Path.GetFileName(subDir);
                    string fullPath = GetRelativePath(subDir);

                    if (string.IsNullOrEmpty(fullPath)) continue;

                    FolderNode childNode = new FolderNode
                    {
                        name = folderName,
                        fullPath = fullPath
                    };

                    currentNode.children.Add(childNode);
                    BuildFolderTreeRecursive(subDir, childNode);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Debug.LogWarning($"No access to directory: {currentPath}");
            }
        }

        /// <summary>
        /// Converts an absolute path to a Unity-relative path.
        /// </summary>
        private string GetRelativePath(string absPath)
        {
            absPath = absPath.Replace('\\', '/');
            string dataPath = Application.dataPath.Replace('\\', '/');

            if (absPath.StartsWith(dataPath))
            {
                return "Assets" + absPath.Substring(dataPath.Length);
            }

            if (absPath.StartsWith("Assets/") || absPath == "Assets")
            {
                return absPath;
            }

            return null;
        }

        /// <summary>
        /// Forces all folders in the tree to expand or collapse.
        /// </summary>
        public void ForceExpandAll(FolderNode node, bool expand)
        {
            if (node == null) return;
            node.isExpanded = expand;
            foreach (var child in node.children) ForceExpandAll(child, expand);
        }
    }

    /// <summary>
    /// Represents a node in the folder tree structure.
    /// </summary>
    public class FolderNode
    {
        /// <summary>
        /// The name of the folder.
        /// </summary>
        public string name, fullPath;

        /// <summary>
        /// List of child folder nodes.
        /// </summary>
        public List<FolderNode> children = new List<FolderNode>();

        /// <summary>
        /// Whether the folder is expanded in the UI.
        /// </summary>
        public bool isExpanded = true, hasScriptsOrScriptChildren;

        /// <summary>
        /// Whether this folder has any child folders.
        /// </summary>
        public bool hasChildren => children.Count > 0;
    }
}