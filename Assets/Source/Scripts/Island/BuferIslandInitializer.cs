using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuferIslandInitializer : MonoBehaviour
{
    [SerializeField] private BaseIsland _island;
    [SerializeField] private Transform _rootOfPoints;
    [SerializeField] private List<Transform> _points;

    public int Size => _points.Count;
    public BaseIsland Island => _island;

    public void ResetPoints()
    {
        List<IslandPoint> placementPoints = new List<IslandPoint>();

        foreach (Transform point in _points)
        {
            placementPoints.Add(new IslandPoint(point));
        }

        _island.Initialize(placementPoints);
    }

    [ContextMenu("Fill Points")]
    public void FillPoints()
    {
        _points.Clear();
        _points.AddRange(_rootOfPoints.GetComponentsInChildren<Transform>()
                .Where(transform => transform != _rootOfPoints));
    }
}
