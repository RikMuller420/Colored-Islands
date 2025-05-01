using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Island))]
public class IslandInitializer : MonoBehaviour
{

    [SerializeField] private Paint _paint;
    [SerializeField] private Island _island;
    [SerializeField] private Transform _rootOfPoints;
    [SerializeField] private List<Transform> _points;
    [SerializeField] List<IslandStartUnits> _startUnits;

    [SerializeField] private UnitCreator _unitCreator;

    public int PointsCount => _points.Count;
    public Paint Paint => _paint;
    public Island Island => _island;
    public Transform RootOfPoints => _rootOfPoints;
    public List<Transform> Points => new List<Transform>(_points);
    public List<IslandStartUnits> StartUnits => new List<IslandStartUnits>(_startUnits);


    private void Start()
    {
        InitializeIsland();
    }

    public void SetStartUnits(List<IslandStartUnits> startUnits)
    {
        _startUnits = startUnits;
    }

    public void InitializeIsland()
    {
        _island = GetComponent<Island>();
        List<PlacementPoint> placementPoints = new List<PlacementPoint>();

        foreach(Transform point in _points)
        {
            placementPoints.Add(new PlacementPoint(point));            
        }

        _island.Initialize(placementPoints, Paint);

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

    public void SetPaint(Paint paint)
    {
        _paint = paint;
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        switch (Paint)
        {
            case Paint.Red:
                renderer.material.color = Color.red;
                break;

            case Paint.Blue:
                renderer.material.color = Color.blue;
                break;

            case Paint.Green:
                renderer.material.color = Color.green;
                break;

            case Paint.Yellow:
                renderer.material.color = Color.yellow;
                break;

            case Paint.Pink:
                renderer.material.color = Color.magenta;
                break;

        }

        Undo.RegisterCreatedObjectUndo(renderer, "Change material");
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
}
