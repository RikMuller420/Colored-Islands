using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuferIslandInitializer : MonoBehaviour
{
    [SerializeField] private BaseIsland _island;
    [SerializeField] private Transform _rootOfPoints;
    [SerializeField] private List<Transform> _points;

    public int Size => _points.Count;

    public void Initialize()
    {
        List<PlacementPoint> placementPoints = new List<PlacementPoint>();

        foreach (Transform point in _points)
        {
            placementPoints.Add(new PlacementPoint(point));
        }

        _island.Initialize(placementPoints);
        gameObject.SetActive(true);
    }

    [ContextMenu("Fill Points")]
    public void FillPoints()
    {
        _points.Clear();
        _points.AddRange(_rootOfPoints.GetComponentsInChildren<Transform>()
                .Where(transform => transform != _rootOfPoints));
    }
}
