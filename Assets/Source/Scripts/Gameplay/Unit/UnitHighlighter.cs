using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;

namespace SlimeGround.Gameplay.Units
{
	public class UnitHighlighter
	{
	    public void HighlightUnits(BaseIsland island, UnitSlotType slot)
	    {
	        foreach (Unit unit in island.GetUnits(slot))
	        {
	            unit.ActivateOutline();
	        }
	    }

	    public void UnhighlightUnits(BaseIsland island, UnitSlotType slot)
	    {
	        foreach (Unit unit in island.GetUnits(slot))
	        {
	            unit.DeactivateOutline();
	        }
	    }
	}
}
