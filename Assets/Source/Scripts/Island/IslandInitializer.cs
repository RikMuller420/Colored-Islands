using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Island))]
public class IslandInitializer : MonoBehaviour
{
    [SerializeField] private Paint _paint;
    [SerializeField] private Island _island;
    [SerializeField] private Transform _rootOfPoints;
    [SerializeField] private List<Transform> _points = new List<Transform>();
    [SerializeField] private List<IslandStartUnits> _startUnits = new List<IslandStartUnits>();

    public int PointsCount => _points.Count;
    public Paint Paint => _paint;
    public Island Island => _island;
    public Transform RootOfPoints => _rootOfPoints;
    public List<Transform> Points => new List<Transform>(_points);
    public List<IslandStartUnits> StartUnits => new List<IslandStartUnits>(_startUnits);

    public void Initialize(Func<Unit> createUnit, PaintMaterials paintMaterials)
    {
        FindRequireComponents();
        List<PlacementPoint> placementPoints = new List<PlacementPoint>();

        foreach(Transform point in _points)
        {
            placementPoints.Add(new PlacementPoint(point));            
        }

        _island.Initialize(placementPoints, Paint, paintMaterials);

        foreach (IslandStartUnits startUnits in _startUnits)
        {
            for (int i = 0; i < startUnits.Amout; i++)
            {
                Unit unit = createUnit.Invoke();
                unit.Initialize(_island, startUnits.Paint, paintMaterials);
                _island.AddUnit(unit, out PlacementPoint placementPoint);
                unit.transform.position = placementPoint.Point.position;
            }
        }
    }

    public void SetStartUnits(List<IslandStartUnits> startUnits)
    {
        _startUnits = startUnits;
    }

    public void FillPoints(Transform rootOfPoints)
    {
        _rootOfPoints = rootOfPoints;
        _points.Clear();
        _points.AddRange(_rootOfPoints.GetComponentsInChildren<Transform>()
                .Where(transform => transform != RootOfPoints));
    }

    public void FindRequireComponents()
    {
        _island = GetComponent<Island>();
    }

    public void SetPaint(Paint paint)
    {
        _paint = paint;
    }
}
