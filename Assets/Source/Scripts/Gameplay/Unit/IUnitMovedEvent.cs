using System;

namespace SlimeGround.Gameplay.Units
{

	public interface IUnitMovedEvent
	{
	    public event Action<UnitsMoveInfo> UnitsMoved;
	}

}
