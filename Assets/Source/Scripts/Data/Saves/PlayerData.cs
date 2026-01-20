using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
    [JsonProperty] public bool IsTrainingFinished;
    [JsonProperty] public int AviableSpinCount;
    [JsonProperty] public Dictionary<BoostType, int> BoostsAmounts;
    [JsonProperty] public Dictionary<UpgradeType, int> UpgradeStages;
    [JsonProperty] public Dictionary<InAppType, int> EarnInAppWithAddProgress;
    [JsonProperty] public Dictionary<AudioGroup, bool> IsSoundOnStatus;
    [JsonProperty] public Dictionary<UnitSlotType, CustomizationPreferences> CustomizationPreferences;
    [JsonProperty] public Dictionary<int, bool> WasHatsUsed;
    [JsonProperty] public Dictionary<int, bool> WasLevelRewardReceived;
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
        WasLevelRewardReceived.Add(levelId, wasReceived);
    }

    public void AddHat(int hatId, bool wasUsed)
    {
        WasHatsUsed.Add(hatId, wasUsed);
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
        WasHatsUsed = new Dictionary<int, bool>();
        WasLevelRewardReceived = new Dictionary<int, bool>();
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

