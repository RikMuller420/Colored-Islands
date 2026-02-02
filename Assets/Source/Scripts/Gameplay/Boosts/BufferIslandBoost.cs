using SlimeGround.Gameplay.Islands;

namespace SlimeGround.Gameplay.Boosts
{
	[System.Serializable]
	public class BufferIslandBoost : Boost
	{
	    private const int ExtraSize = 2;

	    private BuferIslandsHolder _buferIslandsHolder;

	    public BufferIslandBoost(BuferIslandsHolder buferIslandsHolder,
	                             BoostAmountProvider boostAmountProvider)
								 : base(boostAmountProvider)
	    {
	        _buferIslandsHolder = buferIslandsHolder;
	    }

	    public override BoostType Type => BoostType.GrowBuferIsland;

	    public override void TryApplyBoost()
	    {
	        BaseIsland oldIsland = _buferIslandsHolder.CurrentIsland;
	        int newSize = oldIsland.Points.Count + ExtraSize;
	        _buferIslandsHolder.SwapToNewIsland(newSize);
	        SpendBoost(Type);
	    }
	}
}
