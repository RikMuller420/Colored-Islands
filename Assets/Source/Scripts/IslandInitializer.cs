using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandInitializer : MonoBehaviour
{
    [SerializeField] private Island _island;
    [SerializeField] private Paint _paint;
    [SerializeField] private UnitCreator _unitCreator;
    [SerializeField] private Transform _rootOfPoints;
    [SerializeField] private List<IslandStartUnits> _startUnits;
    [SerializeField] private List<Transform> _points;

    private void Start()
    {
        InitializeIsland();
    }

    public void InitializeIsland()
    {
        List<PlacementPoint> placementPoints = new List<PlacementPoint>();

        foreach(Transform point in _points)
        {
            placementPoints.Add(new PlacementPoint(point));            
        }

        _island.Initialize(placementPoints, _paint);

        foreach (IslandStartUnits startUnits in _startUnits)
        {
            for (int i = 0; i < startUnits.Count; i++)
            {
                Unit unit = _unitCreator.Create();
                unit.Initialize(_island, startUnits.Paint);
                _island.AddUnit(unit, out PlacementPoint placementPoint);
                unit.transform.position = placementPoint.Point.position;
            }
        }
    }

    [ContextMenu("Fill Points")]
    private void FillPoints()
    {
        _points.Clear();
        _points.AddRange(_rootOfPoints.GetComponentsInChildren<Transform>()
                .Where(t => t != _rootOfPoints));
    }
}
