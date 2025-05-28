using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseIsland : MonoBehaviour, ISelectable
{
    private List<IslandPoint> _placementPoints;

    public event Action UnitAdded;

    public int FreePointsCount => _placementPoints.Count(point => point.IsFree);
    public IReadOnlyCollection<IslandPoint> Points => new ReadOnlyCollection<IslandPoint>(_placementPoints);

    public void Initialize(List<IslandPoint> placementPoints)
    {
        _placementPoints = placementPoints;
    }

    public IEnumerable<Unit> GetUnits(Paint paint)
    {
        foreach (IslandPoint point in _placementPoints)
        {
            if (point.IsFree == false && point.OccupiedUnit.Paint == paint)
            {
                yield return point.OccupiedUnit;
            }
        }
    }

    public void RemoveUnit(Unit unit)
    {
        IslandPoint point = _placementPoints.FirstOrDefault(p => p.IsFree == false && p.OccupiedUnit == unit);

        if (point != null)
        {
            point.RemoveUnit();

            return;
        }

        throw new InvalidOperationException("Unit not found in placement points");
    }

    public void AddUnit(Unit unit, out IslandPoint placementPoint)
    {
        placementPoint = _placementPoints.FirstOrDefault(point => point.IsFree);

        if (placementPoint != null)
        {
            placementPoint.SetUnit(unit);
            UnitAdded?.Invoke();

            return;
        }

        throw new InvalidOperationException("No available free placement points");
    }
}