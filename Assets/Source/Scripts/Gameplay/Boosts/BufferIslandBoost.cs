using SlimeGround.Gameplay.Islands;

namespace SlimeGround.Gameplay.Boosts
{
	[System.Serializable]
	public class BufferIslandBoost : Boost
	{
	    private const int ExtraSize = 2;

		public BufferIslandBoost(BuferIslands buferIslandsHolder,
	                             BoostAmountProvider boostAmountProvider)
								 : base(boostAmountProvider)
	    {
	        _buferIslandsHolder = buferIslandsHolder;
	    }

		private BuferIslands _buferIslandsHolder { get; }

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
