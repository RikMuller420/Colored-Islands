using System;
using GameAnalyticsSDK;

[Serializable]
public abstract class Boost 
{
    private BoostAmountProvider _boostAmountProvider;

    public event Action<Boost> BoostApplyed;

    public Boost(BoostAmountProvider boostAmountProvider)
    {
        _boostAmountProvider = boostAmountProvider;
    }

    public abstract BoostType Type { get; }

    public abstract void TryApplyBoost();

    protected void SpendBoost(BoostType boostType)
    {
        BoostApplyed?.Invoke(this);
        _boostAmountProvider.SpendBoost(boostType);
        MetricSaver.SpentBoost(boostType);
    }
}
