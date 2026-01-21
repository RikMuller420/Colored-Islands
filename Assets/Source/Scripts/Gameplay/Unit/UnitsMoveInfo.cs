using System.Collections.Generic;
using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;

namespace SlimeGround.Gameplay.Units
{
	public class UnitsMoveInfo
	{
	    public UnitsMoveInfo(BaseIsland startIsland, BaseIsland endIsland,
	                         UnitSlotType unitsSlot, IReadOnlyCollection<Unit> units)
	    {
	        StartIsland = startIsland;
	        EndIsland = endIsland;
	        UnitsSlot = unitsSlot;
	        Units = units;
	    }

	    public BaseIsland StartIsland { get; }
	    public BaseIsland EndIsland { get; }
	    public UnitSlotType UnitsSlot { get; }
	    public IReadOnlyCollection<Unit> Units { get; }
	}
}
