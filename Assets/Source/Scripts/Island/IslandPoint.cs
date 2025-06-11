using UnityEngine;

public class IslandPoint
{
    public IslandPoint(SpriteRenderer point)
    {
        Point = point;
        IsFree = true;
        OccupiedUnit = null;
    }

    public SpriteRenderer Point { get; }
    public Transform Transform => Point.transform;
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
