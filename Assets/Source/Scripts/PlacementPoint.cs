using UnityEngine;

public class PlacementPoint
{
    public PlacementPoint(Transform point, Unit occupiedUnit)
    {
        Point = point;
        IsOccupied = true;
        OccupiedUnit = occupiedUnit;
    }

    public PlacementPoint(Transform point)
    {
        Point = point;
        IsOccupied = false;
        OccupiedUnit = null;
    }

    public Transform Point { get; }
    public bool IsOccupied { get; private set; }
    public Unit OccupiedUnit { get; private set; }
}
