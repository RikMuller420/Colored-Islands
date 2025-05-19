using System;

public class BoostAmountProvider 
{
    private GameProgressStorage _gameProgressStorage;

    public event Action<BoostType> BoostsAmountChanged;

    public BoostAmountProvider(GameProgressStorage gameProgressStorage)
    {
        _gameProgressStorage = gameProgressStorage;
        _gameProgressStorage.BoostsAmountChanged += OnBoostsAmountInSavedProgressChanged;
    }

    public int BoostAmount(BoostType boostType) => _gameProgressStorage.GetBoostAmount(boostType);

    public void SpendBoost(BoostType boostType)
    {
        int boostAmount = BoostAmount(boostType);

        if (boostAmount == 0)
        {
            throw new InvalidOperationException("not enough Boosts");
        }

        boostAmount--;
        _gameProgressStorage.SetBoostAmount(boostType, boostAmount);
    }

    public void AddBoost(BoostType boostType)
    {
        int boostAmount = BoostAmount(boostType);
        boostAmount++;
        _gameProgressStorage.SetBoostAmount(boostType, boostAmount);
    }

    private void OnBoostsAmountInSavedProgressChanged(BoostType boostType)
    {
        BoostsAmountChanged?.Invoke(boostType);
    }
}
