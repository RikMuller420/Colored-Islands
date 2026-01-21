using SlimeGround.Data;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	[System.Serializable]
	public class IslandStartUnits
	{
	    [SerializeField] private UnitSlotType _slot;
	    [SerializeField] private int _amount;

	    public IslandStartUnits(UnitSlotType slot, int count = 1)
	    {
	        _slot = slot;
	        _amount = count;
	    }

	    public UnitSlotType Slot { get => _slot; }
	    public int Amout { get => _amount; }
	}
}
