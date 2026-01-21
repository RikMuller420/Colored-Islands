using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	[System.Serializable]
	public class Level1TrainingMove
	{
	    [SerializeField] private Level1TrainingMoveType _type;
	    [SerializeField] private Island _island;
	    [SerializeField] private bool _isUseBufferIsland;
	    [SerializeField] private UnitSlotType _unitsSlot;
	    [SerializeField] private RectTransform _pointerPosition;

	    public Level1TrainingMoveType Type => _type;
	    public Island Island => _island;
	    public bool IsUseBufferIsland => _isUseBufferIsland;
	    public UnitSlotType UnitsSlot => _unitsSlot;
	    public RectTransform PointerPosition => _pointerPosition;
	}
}
