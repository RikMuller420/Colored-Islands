using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data.Saves;

namespace SlimeGround.Gameplay.Boosts
{
	public class BoostAmountProvider
	{
		public BoostAmountProvider(PlayerDataProvider playerData)
	    {
	        _playerData = playerData;
	        _playerData.Resources.BoostsAmountChanged += OnBoostsAmountInSavedProgressChanged;
	    }
		private PlayerDataProvider _playerData { get; }

		public event Action<BoostType> BoostsAmountChanged;
		public event Action<BoostType> BoostApplyed;

		public int BoostAmount(BoostType boostType) => _playerData.Resources.GetBoostAmount(boostType);

		public void Dispose()
		{
			_playerData.Resources.BoostsAmountChanged -= OnBoostsAmountInSavedProgressChanged;
		}

		public void SpendBoost(BoostType boostType)
	    {
	        int boostAmount = BoostAmount(boostType);

	        if (boostAmount == 0)
	        {
	            throw new InvalidOperationException("not enough Boosts");
	        }

	        boostAmount--;
	        _playerData.Resources.SetBoostAmount(boostType, boostAmount);
	        _playerData.Save();
	        BoostApplyed?.Invoke(boostType);
	    }

	    public void AddBoost(BoostType boostType)
	    {
	        int boostAmount = BoostAmount(boostType);
	        boostAmount++;
	        _playerData.Resources.SetBoostAmount(boostType, boostAmount);
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
	            _playerData.Resources.SetBoostAmount(boostType, boostAmount);
	            _playerData.Save();
	        }
	    }

	    private void OnBoostsAmountInSavedProgressChanged(BoostType boostType)
	    {
	        BoostsAmountChanged?.Invoke(boostType);
	    }
	}
}
