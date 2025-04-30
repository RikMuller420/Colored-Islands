using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandInitializer : MonoBehaviour
{
    [SerializeField] private Island _island;
    [SerializeField] private Paint _paint;
    [SerializeField] private UnitCreator _unitCreator;
    [SerializeField] private Transform _rootOfPoints;
    [SerializeField] private List<Transform> _points;
    [SerializeField] private bool isBuffer = false;

    private void Start()
    {
        InitializeIsland();
    }

    public void InitializeIsland()
    {
        List<PlacementPoint> placementPoints = new List<PlacementPoint>();

        foreach(Transform point in _points)
        {
            if (isBuffer)
            {
                placementPoints.Add(new PlacementPoint(point));
            }
            else
            {
                Unit unit = _unitCreator.Create(point);
                placementPoints.Add(new PlacementPoint(point, unit));
            }       
        }

        foreach (PlacementPoint point in placementPoints)
        {
            if (point.IsFree)
            {
                continue;
            }

            point.OccupiedUnit.Initialize(_island, Paint.Blue);
        }

        _island.Initialize(placementPoints, _paint);
    }

    [ContextMenu("Fill Points")]
    private void FillPoints()
    {
        _points.Clear();
        _points.AddRange(_rootOfPoints.GetComponentsInChildren<Transform>()
                .Where(t => t != _rootOfPoints));
    }
}
