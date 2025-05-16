using System;

public abstract class Boost 
{
    private BoostAmountProvider _boostAmountProvider;

    public event Action<Boost> BoostApplyed;

    public Boost(BoostAmountProvider boostAmountProvider)
    {
        _boostAmountProvider = boostAmountProvider;
    }

    public abstract void TryApplyBoost();

    protected void SpendBoost<T>() where T : Boost
    {
        BoostApplyed?.Invoke(this);
        _boostAmountProvider.SpendBoost<T>();
    }
}
