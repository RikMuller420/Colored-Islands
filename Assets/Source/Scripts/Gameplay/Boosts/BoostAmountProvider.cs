using System;
using System.Collections.Generic;
using System.Linq;

public class BoostAmountProvider
{
    private GameProgressStorage _gameProgressStorage;

    public event Action<BoostType> BoostsAmountChanged;
    public event Action<BoostType> BoostApplyed;

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
        BoostApplyed?.Invoke(boostType);
    }

    public void AddBoost(BoostType boostType)
    {
        int boostAmount = BoostAmount(boostType);
        boostAmount++;
        _gameProgressStorage.SetBoostAmount(boostType, boostAmount);
    }

    public void AddBoostBundle(int amount)
    {
        IEnumerable<BoostType> boostTypes = Enum.GetValues(typeof(BoostType)).Cast<BoostType>();
        BoostType lastType = boostTypes.Last();

        foreach (BoostType boostType in boostTypes)
        {
            int boostAmount = BoostAmount(boostType);
            boostAmount += amount;
            _gameProgressStorage.SetBoostAmount(boostType, boostAmount);
        }
    }

    private void OnBoostsAmountInSavedProgressChanged(BoostType boostType)
    {
        BoostsAmountChanged?.Invoke(boostType);
    }
}
