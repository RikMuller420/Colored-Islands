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
    [SerializeField] private List<SpriteRenderer> _points = new List<SpriteRenderer>();
    [SerializeField] private List<IslandStartUnits> _startUnits = new List<IslandStartUnits>();

    public int PointsCount => _points.Count;
    public Paint Paint => _paint;
    public Island Island => _island;
    public Transform RootOfPoints => _rootOfPoints;
    public IReadOnlyList<SpriteRenderer> Points => new List<SpriteRenderer>(_points);
    public List<IslandStartUnits> StartUnits => new List<IslandStartUnits>(_startUnits);

    public void Initialize(Func<Unit> createUnit, PaintMaterials paintMaterials, Transform unitsLookAtPoint,
                          CustomizationSettingsHolder customizationSettings, ColorSample colorSample)
    {
        FindRequireComponents();
        List<IslandPoint> placementPoints = new List<IslandPoint>();

        foreach(SpriteRenderer point in _points)
        {
            placementPoints.Add(new IslandPoint(point));            
        }

        _island.Initialize(placementPoints, Paint, colorSample, paintMaterials);

        foreach (IslandStartUnits startUnits in _startUnits)
        {
            for (int i = 0; i < startUnits.Amout; i++)
            {
                Unit unit = createUnit.Invoke();
                unit.Initialize(customizationSettings);
                unit.ResetRotation();
                unit.SetIsland(_island);
                unit.SetPaint(startUnits.Paint);
                unit.Activate();

                _island.AddStartUnit(unit, out IslandPoint placementPoint);
                unit.transform.position = placementPoint.Transform.position;
                unit.MeshTransform.LookAt(unitsLookAtPoint);
            }
        }
    }

    public void SetStartUnits(List<IslandStartUnits> startUnits)
    {
        _startUnits = startUnits;
    }

    public void FindRequireComponents()
    {
        _island = GetComponent<Island>();
    }

    public void SetPaint(Paint paint)
    {
        _paint = paint;
    }

    public void FillPoints(Transform rootOfPoints)
    {
        _rootOfPoints = rootOfPoints;
        _points.Clear();
        _points.AddRange(_rootOfPoints.GetComponentsInChildren<SpriteRenderer>()
                .Where(transform => transform != RootOfPoints));
    }

    [ContextMenu("Fill Points")]
    public void FillPoints()
    {
        FillPoints(_rootOfPoints);
    }
}
