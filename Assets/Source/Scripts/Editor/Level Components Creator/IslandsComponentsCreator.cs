using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IslandsComponentsCreator
{
    public IReadOnlyCollection<IslandInitializer> CreateRequireComponents(Transform islandsParent)
    {
        List<IslandInitializer> islandInitializers = new List<IslandInitializer>();

        foreach (Transform child in islandsParent)
        {
            IslandInitializer initializer = child.GetComponent<IslandInitializer>();

            if (child.TryGetComponent<Collider>(out _) == false)
            {
                child.gameObject.AddComponent<MeshCollider>();
                EditorUtility.SetDirty(child.gameObject);
            }

            if (child.TryGetComponent<Island>(out _) == false)
            {
                child.gameObject.AddComponent<Island>();
                EditorUtility.SetDirty(child.gameObject);
            }

            if (initializer == null)
            {
                initializer = child.gameObject.AddComponent<IslandInitializer>();
                EditorUtility.SetDirty(child.gameObject);
            }

            if (initializer != null)
            {
                islandInitializers.Add(initializer);
            }
        }

        if (islandsParent.TryGetComponent<IslandsGroupInitializer>(out _) == false)
        {
            islandsParent.gameObject.AddComponent<IslandsGroupInitializer>();
        }

        islandsParent.GetComponent<IslandsGroupInitializer>().SetIslands(islandInitializers);

        return islandInitializers.AsReadOnly();
    }
}
