using System;
using System.Collections.Generic;
using SlimeGround.Effects.Sound;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Integration.Localization;
using SlimeGround.Menu.Windows.Customization;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using SlimeGround.Menu.Windows.InAppPurchase;

namespace SlimeGround.Data.Saves
{
	public interface IPlayerData 
	{
		public event Action GoldAmountChanged;
		public event Action<int> LevelProgressChanged;
		public event Action<BoostType> BoostsAmountChanged;
		public event Action<UpgradeType> Upgraded;
		public event Action<InAppType> EarnInAppWithAddProgressUpdated;

		public event Action RemoveAdsStateChanged;
		public event Action<AudioGroup> SoundEnabledChanged;
		public event Action<UnitSlotType> CustomizationPreferenceChanged;
		public event Action TrainingFinished;
		public event Action<int> FaceUnlocked;
		public event Action SpinCountChanged;

		public int LastAvailableLevelId { get; }
	    public LevelProgress FirstUnfinishedLevel { get; }
	    public bool IsTrainingFinished { get; }
	    public IReadOnlyCollection<LevelProgress> Levels { get; }
	    public int GoldAmount { get; }
	    public int ScoreAmount { get; }
	    public bool IsAdsRemoved { get; }
	    public int AviableSpinCount { get; }
	    public IReadOnlyCollection<FaceAvailabilitie> FaceAvailabilities { get; }
	    public bool IsLanguageSaved { get; }
	    public Language Language { get; }

	    public bool IsHatUsed(int hatId);
	    public bool IsLevelRewardReceived(int levelId);
	    public CustomizationPreferences GetCustomizationPreference(UnitSlotType slot);
	    public int GetBoostAmount(BoostType boostType);
	    public int GetUpgradeStage(UpgradeType upgradeType);
	    public int GetEarnedInAppWithAddProgress(InAppType inAppType);
	    public bool GetIsSoundOnStatus(AudioGroup audioGroup);
	}
}
