using System;
using SlimeGround.Integration.Metrics;

namespace SlimeGround.Gameplay.Boosts
{
	[Serializable]
	public abstract class Boost 
	{
		public Boost(BoostAmountProvider boostAmountProvider)
	    {
	        _boostAmountProvider = boostAmountProvider;
	    }

	    private BoostAmountProvider _boostAmountProvider { get; }

		public event Action<Boost> BoostApplyed;

		public abstract BoostType Type { get; }

	    public abstract void TryApplyBoost();

	    protected void SpendBoost(BoostType boostType)
	    {
	        BoostApplyed?.Invoke(this);
	        _boostAmountProvider.SpendBoost(boostType);
	        MetricSaver.SpendBoost(boostType);
	    }
	}
}
