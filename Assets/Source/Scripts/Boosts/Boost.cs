using System;

public abstract class Boost 
{
    public event Action<Boost> BoostApplyed;

    public abstract void TryApplyBoost();

    protected void InvokeBoostApplyedEvent()
    {
        BoostApplyed?.Invoke(this);
    }
}
