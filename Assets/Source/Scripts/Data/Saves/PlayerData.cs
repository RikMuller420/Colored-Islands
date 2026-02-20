using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Effects.Sound;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Integration.Localization;
using SlimeGround.Menu.Windows.Customization;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using SlimeGround.Menu.Windows.InAppPurchase;

namespace SlimeGround.Data.Saves
{
	[Serializable]
	public class PlayerData
	{
	    [JsonProperty] public string SaveSignatureKey;
	    [JsonProperty] public List<LevelProgress> Levels;
	    [JsonProperty] public int ScoreAmount;
	    [JsonProperty] public int GoldAmount;
	    [JsonProperty] public bool IsAdsRemoved;
	    [JsonProperty] public bool IsLanguageSaved;
	    [JsonProperty] public Language Language;
		[JsonProperty] public bool IsCustomizationWindowWasOpened;
		[JsonProperty] public bool IsTrainingFinished;
	    [JsonProperty] public int AviableSpinCount;
	    [JsonProperty] public Dictionary<BoostType, int> BoostsAmounts;
	    [JsonProperty] public Dictionary<UpgradeType, int> UpgradeStages;
	    [JsonProperty] public Dictionary<InAppType, int> EarnInAppWithAddProgress;
	    [JsonProperty] public Dictionary<AudioGroup, bool> IsSoundOnStatus;
	    [JsonProperty] public Dictionary<UnitSlotType, CustomizationPreferences> CustomizationPreferences;
	    [JsonProperty] public Dictionary<int, bool> IsHatsUsed;
	    [JsonProperty] public Dictionary<int, bool> IsLevelRewardReceived;
	    [JsonProperty] public List<FaceAvailabilitie> FaceAvailabilities;

	    public PlayerData()
	    {
	        SetDefaultValues();
	    }

	    public PlayerData(UnitsHatSettings unitsHatSettings)
	    {
	        SetDefaultValues(unitsHatSettings.NoHatId);
	    }

	    public void AddFace(int faceId, bool isAviable, bool wasUsed)
	    {
	        FaceAvailabilities.Add(new FaceAvailabilitie(faceId, isAviable, wasUsed));
	    }

	    public void AddLevelReward(int levelId, bool wasReceived)
	    {
	        IsLevelRewardReceived.Add(levelId, wasReceived);
	    }

	    public void AddHat(int hatId, bool wasUsed)
	    {
	        IsHatsUsed.Add(hatId, wasUsed);
	    }

	    public void AddLevel(LevelProgress levelProgress)
	    {
	        if (levelProgress == null)
	        {
	            throw new ArgumentNullException(nameof(levelProgress));
	        }

	        Levels.Add(levelProgress);
	    }

	    private void SetDefaultValues(int noHatId = 0)
	    {
	        Levels = new List<LevelProgress>();
	        FaceAvailabilities = new List<FaceAvailabilitie>();
	        IsHatsUsed = new Dictionary<int, bool>();
	        IsLevelRewardReceived = new Dictionary<int, bool>();
	        BoostsAmounts = new Dictionary<BoostType, int>();
	        UpgradeStages = new Dictionary<UpgradeType, int>();
	        IsSoundOnStatus = new Dictionary<AudioGroup, bool>();
	        CustomizationPreferences = new Dictionary<UnitSlotType, CustomizationPreferences>();
	        ScoreAmount = 0;
	        GoldAmount = 0;
	        IsAdsRemoved = false;

	        EarnInAppWithAddProgress = new Dictionary<InAppType, int>()
	        {
	            { InAppType.RemoveAds, 0 }
	        };

	        foreach (BoostType boostType in Enum.GetValues(typeof(BoostType)))
	        {
	            BoostsAmounts.Add(boostType, 0);
	        }

	        foreach (UpgradeType upgradeType in Enum.GetValues(typeof(UpgradeType)))
	        {
	            UpgradeStages.Add(upgradeType, 0);
	        }

	        foreach (AudioGroup audioGroup in Enum.GetValues(typeof(AudioGroup)))
	        {
	            IsSoundOnStatus.Add(audioGroup, true);
	        }

	        foreach (UnitSlotType unitSlot in Enum.GetValues(typeof(UnitSlotType)))
	        {
	            int index = (int)unitSlot;
	            CustomizationPreferences.Add(unitSlot, new CustomizationPreferences(index + 1, noHatId, index));
	        }
	    }
	}
}
