using System;

public interface IUnitMovedEvent
{
    public event Action<UnitsMoveInfo> UnitsMoved;
}
