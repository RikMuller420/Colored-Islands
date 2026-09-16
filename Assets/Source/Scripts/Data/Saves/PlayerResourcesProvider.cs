using System;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using SlimeGround.Menu.Windows.InAppPurchase;

namespace SlimeGround.Data.Saves
{
	public class PlayerResourcesProvider
	{
		private PlayerData _playerData;

		public event Action GoldAmountChanged;
		public event Action<BoostType> BoostsAmountChanged;
		public event Action<UpgradeType> Upgraded;
		public event Action<InAppType> EarnInAppWithAddProgressUpdated;
		public event Action RemoveAdsStateChanged;
		public event Action SpinCountChanged;

		public PlayerResourcesProvider(PlayerData playerData)
		{
			_playerData = playerData;
		}

		public int GoldAmount => _playerData.GoldAmount;
		public bool IsAdsRemoved => _playerData.IsAdsRemoved;
		public int AviableSpinCount => _playerData.AviableSpinCount;

		public int GetBoostAmount(BoostType boostType) => _playerData.BoostsAmounts[boostType];
		public int GetUpgradeStage(UpgradeType upgradeType) => _playerData.UpgradeStages[upgradeType];
		public int GetEarnedInAppWithAddProgress(InAppType inAppType) => _playerData.EarnInAppWithAddProgress[inAppType];

		public void SetSpinCount(int spinCount)
		{
			_playerData.AviableSpinCount = spinCount;
			SpinCountChanged?.Invoke();
		}

		public void ApplyRemoveAddBonus()
		{
			_playerData.IsAdsRemoved = true;
			RemoveAdsStateChanged?.Invoke();
		}

		public void SetBoostAmount(BoostType boostType, int amount)
		{
			_playerData.BoostsAmounts[boostType] = amount;
			BoostsAmountChanged?.Invoke(boostType);
		}

		public void SetUpgradeStage(UpgradeType upgradeType, int stage)
		{
			_playerData.UpgradeStages[upgradeType] = stage;
			Upgraded?.Invoke(upgradeType);
		}

		public void SetEarnInAppWithAddProgress(InAppType inAppType, int progress)
		{
			_playerData.EarnInAppWithAddProgress[inAppType] = progress;
			EarnInAppWithAddProgressUpdated?.Invoke(inAppType);
		}

		public void SetGoldAmount(int amount)
		{
			if (amount < 0)
			{
				throw new ArgumentException(nameof(amount));
			}

			_playerData.GoldAmount = amount;
			GoldAmountChanged?.Invoke();
		}
	}
}
