using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Island : MonoBehaviour, ISelectable
{
    private List<PlacementPoint> _placementPoints;

    public void Initialize(List<PlacementPoint> placementPoints, Paint paint)
    {
        _placementPoints = placementPoints;
        Paint = paint;
    }

    public Paint Paint { get; private set; }
    public int FreePointsCount => _placementPoints.Count(point => point.IsFree);

    public IEnumerable<Unit> GetUnits(Paint paint)
    {
        foreach (PlacementPoint point in _placementPoints)
        {
            if (point.IsFree == false && point.OccupiedUnit.Paint == paint)
            {
                yield return point.OccupiedUnit;
            }
        }
    }

    public void RemoveUnit(Unit unit)
    {
        PlacementPoint point = _placementPoints.FirstOrDefault(p => p.IsFree == false && p.OccupiedUnit == unit);

        if (point != null)
        {
            point.RemoveUnit();

            return;
        }

        throw new InvalidOperationException("Unit not found in placement points");
    }

    public void AddUnit(Unit unit, out PlacementPoint placementPoint)
    {
        placementPoint = _placementPoints.FirstOrDefault(point => point.IsFree);

        if (placementPoint != null)
        {
            placementPoint.SetUnit(unit);

            return;
        }

        throw new InvalidOperationException("No available free placement points");
    }
}