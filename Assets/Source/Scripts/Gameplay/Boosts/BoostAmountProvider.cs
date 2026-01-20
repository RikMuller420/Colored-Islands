using System;
using System.Collections.Generic;
using System.Linq;

public class BoostAmountProvider
{
    private PlayerDataProvider _playerData;

    public event Action<BoostType> BoostsAmountChanged;
    public event Action<BoostType> BoostApplyed;

    public BoostAmountProvider(PlayerDataProvider playerData)
    {
        _playerData = playerData;
        _playerData.BoostsAmountChanged += OnBoostsAmountInSavedProgressChanged;
    }

    public int BoostAmount(BoostType boostType) => _playerData.GetBoostAmount(boostType);

    public void SpendBoost(BoostType boostType)
    {
        int boostAmount = BoostAmount(boostType);

        if (boostAmount == 0)
        {
            throw new InvalidOperationException("not enough Boosts");
        }

        boostAmount--;
        _playerData.SetBoostAmount(boostType, boostAmount);
        _playerData.Save();
        BoostApplyed?.Invoke(boostType);
    }

    public void AddBoost(BoostType boostType)
    {
        int boostAmount = BoostAmount(boostType);
        boostAmount++;
        _playerData.SetBoostAmount(boostType, boostAmount);
        _playerData.Save();
    }

    public void AddBoostBundle(int amount)
    {
        IEnumerable<BoostType> boostTypes = Enum.GetValues(typeof(BoostType)).Cast<BoostType>();
        BoostType lastType = boostTypes.Last();

        foreach (BoostType boostType in boostTypes)
        {
            int boostAmount = BoostAmount(boostType);
            boostAmount += amount;
            _playerData.SetBoostAmount(boostType, boostAmount);
            _playerData.Save();
        }
    }

    private void OnBoostsAmountInSavedProgressChanged(BoostType boostType)
    {
        BoostsAmountChanged?.Invoke(boostType);
    }
}
