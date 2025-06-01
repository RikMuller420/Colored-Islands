using UnityEngine;

public class UnitMoveTask
{
    public UnitMoveTask(Unit unit, IslandPoint targetPoint)
    {
        Unit = unit;
        TargetPoint = targetPoint;
    }

    public Unit Unit { get; }
    public IslandPoint TargetPoint { get; }

    public Vector3 CurrentPosition => Unit.transform.position;
    public Vector3 TargetPosition => TargetPoint.Point.transform.position;
    public float SqrDistToTarget => (TargetPosition - CurrentPosition).sqrMagnitude;
}
