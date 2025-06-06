using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseIsland : MonoBehaviour, ISelectable
{
    public event Action UnitAdded;

    public int FreePointsCount => Points.Count(point => point.IsFree);
    public IReadOnlyCollection<IslandPoint> Points { get; private set; }


    public void Initialize(List<IslandPoint> placementPoints)
    {
        Points = placementPoints.OrderByDescending(point => point.Point.position.z).ToList();
    }

    public IEnumerable<Unit> GetUnits(Paint paint)
    {
        foreach (IslandPoint point in Points)
        {
            if (point.IsFree == false && point.OccupiedUnit.Paint == paint)
            {
                yield return point.OccupiedUnit;
            }
        }
    }

    public void RemoveUnit(Unit unit)
    {
        IslandPoint point = Points.FirstOrDefault(p => p.IsFree == false && p.OccupiedUnit == unit);

        if (point != null)
        {
            point.RemoveUnit();

            return;
        }

        throw new InvalidOperationException("Unit not found in placement points");
    }

    public void AddUnitToFreePoint(Unit unit, out IslandPoint placementPoint)
    {
        List<IslandPoint> aviablePoints = Points.Where(p => p.IsFree).ToList();
        placementPoint = IslandPaintDistributor.ClosestPoint(unit.transform.position, aviablePoints);
        placementPoint.SetUnit(unit);
        UnitAdded?.Invoke();
    }

    public void AddStartUnit(Unit unit, out IslandPoint placementPoint)
    {
        List<IslandPoint> aviablePoints = Points.Where(p => p.IsFree).ToList();
        placementPoint = Points.FirstOrDefault(point => point.IsFree == false && point.OccupiedUnit.Paint == unit.Paint);

        if (placementPoint != null)
        {
            IslandPoint startPoint = placementPoint;
            IslandPoint closestPoint = IslandPaintDistributor.ClosestPoint(startPoint.Point.position, aviablePoints);
            placementPoint = closestPoint;
            closestPoint.SetUnit(unit);
            UnitAdded?.Invoke();

            return;
        }

        placementPoint = Points.FirstOrDefault(point => point.IsFree);

        if (placementPoint != null)
        {
            placementPoint.SetUnit(unit);
            UnitAdded?.Invoke();

            return;
        }

        throw new InvalidOperationException("No available free placement points");
    }
}