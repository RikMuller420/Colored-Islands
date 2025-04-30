using UnityEngine;

public class PlacementPoint
{
    public PlacementPoint(Transform point, Unit occupiedUnit)
    {
        Point = point;
        IsFree = false;
        OccupiedUnit = occupiedUnit;
    }

    public PlacementPoint(Transform point)
    {
        Point = point;
        IsFree = true;
        OccupiedUnit = null;
    }

    public Transform Point { get; }
    public bool IsFree { get; private set; }
    public Unit OccupiedUnit { get; private set; }

    public void RemoveUnit()
    {
        IsFree = true;
        OccupiedUnit = null;
    }

    public void SetUnit(Unit unit)
    {
        IsFree = false;
        OccupiedUnit = unit;
    }
}
