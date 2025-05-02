using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandsGroupInitializer : MonoBehaviour
{
    [SerializeField] private List<IslandInitializer> _islands = new List<IslandInitializer>();

    public IReadOnlyCollection<Island> Islands => _islands.Select(initializer => initializer.Island).ToList().AsReadOnly();

    public void Initialize(Func<Unit> createUnit, PaintMaterials materials)
    {
        foreach (IslandInitializer island in _islands)
        {
            island.Initialize(createUnit, materials);
        }
    }

    public void SetIslands(List<IslandInitializer> islands)
    {
        _islands = islands;
    }
}
