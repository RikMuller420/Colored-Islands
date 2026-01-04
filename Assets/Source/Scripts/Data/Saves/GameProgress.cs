using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class GameProgress
{
    [JsonProperty] private string _saveSignatureKey;
    [JsonProperty] private List<LevelProgress> _levels;
    [JsonProperty] private int _scoreAmount;
    [JsonProperty] private int _goldAmount;
    [JsonProperty] private bool _isAdsRemoved;
    [JsonProperty] private bool _isLanguageSaved;
    [JsonProperty] private Language _language;
    [JsonProperty] private bool _isTrainingFinished;
    [JsonProperty] private int _aviableSpinCount;
    [JsonProperty] private Dictionary<BoostType, int> _boostsAmounts;
    [JsonProperty] private Dictionary<UpgradeType, int> _upgradeStages;
    [JsonProperty] private Dictionary<InAppType, int> _earnInAppWithAddProgress;
    [JsonProperty] private Dictionary<AudioGroup, bool> _isSoundOnStatus;
    [JsonProperty] private Dictionary<Paint, CustomizationPreferences> _customizationPreferences;
    [JsonProperty] private Dictionary<int, bool> _wasHatsUsed;
    [JsonProperty] private Dictionary<int, bool> _wasLevelRewardReceived;
    [JsonProperty] private List<FaceAvailabilitie> _faceAvailabilities;

    public GameProgress()
    {
        SetDefaultValues();
    }

    public GameProgress(UnitsHatSettings unitsHatSettings)
    {
        SetDefaultValues(unitsHatSettings.NoHatId);
    }

    [JsonIgnore] public string SaveSignatureKey => _saveSignatureKey;
    [JsonIgnore] public IReadOnlyCollection<LevelProgress> Levels => _levels.AsReadOnly();
    [JsonIgnore] public int ScoreAmount => _scoreAmount;
    [JsonIgnore] public int GoldAmount => _goldAmount;
    [JsonIgnore] public bool IsAdsRemoved => _isAdsRemoved;
    [JsonIgnore] public bool IsLanguageSaved => _isLanguageSaved;
    [JsonIgnore] public Language Language => _language;
    [JsonIgnore] public bool IsTrainingFinished => _isTrainingFinished;
    [JsonIgnore] public int AviableSpinCount => _aviableSpinCount;
    [JsonIgnore] public Dictionary<int, bool> WasHatsUsed => _wasHatsUsed;
    [JsonIgnore] public Dictionary<int, bool> WasLevelRewardReceived => _wasLevelRewardReceived;
    [JsonIgnore] public IReadOnlyCollection<FaceAvailabilitie> FaceAvailabilities => _faceAvailabilities.AsReadOnly();

    public int GetBoostAmount(BoostType boostType) => _boostsAmounts[boostType];
    public int GetUpgradeStage(UpgradeType upgradeType) => _upgradeStages[upgradeType];
    public int GetEarnedInAppWithAddProgress(InAppType inAppType) => _earnInAppWithAddProgress[inAppType];
    public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _isSoundOnStatus[audioGroup];
    public CustomizationPreferences GetCustomizationPreference(Paint paint) => _customizationPreferences[paint];

    public void SetSaveSignatureKey(string saveSignatureKey)
    {
        _saveSignatureKey = saveSignatureKey;
    }

    public void SetTrainingFinished(bool isFinished)
    {
        _isTrainingFinished = isFinished;
    }

    //Заменить во всех случаях на Add, Spend
    public void SetSpinCount(int spinCount)
    {
        _aviableSpinCount = spinCount;
    }
    public void SetLanguage(Language language)
    {
        _language = language;
        _isLanguageSaved = true;
    }

    public void ApplyRemoveAddBonus()
    {
        _isAdsRemoved = true;
    }

    public void UnlockFace(int faceId)
    {
        FaceAvailabilitie face = _faceAvailabilities.Find(face => face.FaceId == faceId);
        FaceAvailabilitie newFace = new FaceAvailabilitie(face.FaceId, true, face.WasUsed);

        _faceAvailabilities.Remove(face);
        _faceAvailabilities.Add(newFace);
    }

    public void MarkFaceUsed(int faceId)
    {
        FaceAvailabilitie face = _faceAvailabilities.Find(face => face.FaceId == faceId);
        FaceAvailabilitie newFace = new FaceAvailabilitie(face.FaceId, face.IsAviable, true);

        _faceAvailabilities.Remove(face);
        _faceAvailabilities.Add(newFace);
    }

    public void MarkLevelRewardReceived(int levelId)
    {
        if (_wasLevelRewardReceived.ContainsKey(levelId))
        {
            _wasLevelRewardReceived[levelId] = true;
        }
    }

    public void MarkHatUsed(int hatId)
    {
        if (_wasHatsUsed.ContainsKey(hatId))
        {
            _wasHatsUsed[hatId] = true;
        }
    }

    public void ChangeCustomizationPreferenceFace(Paint paint, int faceId)
    {
        CustomizationPreferences preference = GetCustomizationPreference(paint);
        
        int hatId = preference.HatId;
        _customizationPreferences[paint] = new CustomizationPreferences(faceId, hatId, preference.ColorSample);

    }

    public void ChangeCustomizationPreferenceHat(Paint paint, int hatId)
    {
        CustomizationPreferences preference = GetCustomizationPreference(paint);

        int faceId = preference.FaceId;
        _customizationPreferences[paint] = new CustomizationPreferences(faceId, hatId, preference.ColorSample);
    }

    public void ChangeCustomizationPreferenceColor(Paint paint, ColorSample colorSample)
    {
        CustomizationPreferences preference = GetCustomizationPreference(paint);

        int hatId = preference.HatId;
        int faceId = preference.FaceId;
        _customizationPreferences[paint] = new CustomizationPreferences(faceId, hatId, colorSample);
    }

    public void AddFace(int faceId, bool isAviable, bool wasUsed)
    {
        _faceAvailabilities.Add(new FaceAvailabilitie(faceId, isAviable, wasUsed));
    }

    public void AddLevelReward(int levelId, bool wasReceived)
    {
        _wasLevelRewardReceived.Add(levelId, wasReceived);
    }

    public void AddHat(int hatId, bool wasUsed)
    {
        _wasHatsUsed.Add(hatId, wasUsed);
    }

    public void SetBoostAmount(BoostType boostType, int amount)
    {
        if (_boostsAmounts.ContainsKey(boostType) == false)
        {
            throw new ArgumentException(nameof(boostType));
        }

        _boostsAmounts[boostType] = amount;
    }

    public void SetUpgradeStage(UpgradeType upgradeType, int stage)
    {
        if (_upgradeStages.ContainsKey(upgradeType) == false)
        {
            throw new ArgumentException(nameof(upgradeType));
        }

        _upgradeStages[upgradeType] = stage;
    }

    public void SetEarnInAppWithAddProgress(InAppType inAppType, int progress)
    {
        if (_earnInAppWithAddProgress.ContainsKey(inAppType) == false)
        {
            throw new ArgumentException(nameof(inAppType));
        }

        _earnInAppWithAddProgress[inAppType] = progress;
    }

    public void SetGoldAmount(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(nameof(amount));
        }

        _goldAmount = amount;
    }

    public void SetScoreAmount(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(nameof(amount));
        }

        _scoreAmount = amount;
    }

    public void SetSoundEnabledStatus(AudioGroup audioGroup, bool isOn)
    {
        if (_isSoundOnStatus.ContainsKey(audioGroup) == false)
        {
            throw new ArgumentException(nameof(audioGroup));
        }

        _isSoundOnStatus[audioGroup] = isOn;
    }

    public void UpdateLevelProgress(LevelProgress levelProgress)
    {
        int index = _levels.FindIndex(level => level.Id == levelProgress.Id);

        if (index == -1)
        {
            throw new ArgumentException($"Level with ID {levelProgress.Id} not found.");
        }

        _levels[index] = levelProgress;
    }

    public void AddLevel(LevelProgress levelProgress)
    {
        if (levelProgress == null)
        {
            throw new ArgumentNullException(nameof(levelProgress));
        }

        _levels.Add(levelProgress);
    }

    private void SetDefaultValues(int noHatId = 0)
    {
        _levels = new List<LevelProgress>();
        _faceAvailabilities = new List<FaceAvailabilitie>();
        _wasHatsUsed = new Dictionary<int, bool>();
        _wasLevelRewardReceived = new Dictionary<int, bool>();
        _customizationPreferences = new Dictionary<Paint, CustomizationPreferences>();
        _scoreAmount = 0;
        _goldAmount = 0;
        _isAdsRemoved = false;

        _earnInAppWithAddProgress = new Dictionary<InAppType, int>()
        {
            { InAppType.RemoveAds, 0 }
        };

        _boostsAmounts = new Dictionary<BoostType, int>()
        {
            { BoostType.FinishIsland, 0 },
            { BoostType.FreezeObjectives, 0 },
            { BoostType.GrowBuferIsland, 0 },
            { BoostType.ReducePaints, 0 }
        };

        _upgradeStages = new Dictionary<UpgradeType, int>()
        {
            { UpgradeType.BuferIslandSize, 0 },
            { UpgradeType.IncreaseRewards, 0 },
            { UpgradeType.SlowDownAngryBar, 0 }
        };

        _isSoundOnStatus = new Dictionary<AudioGroup, bool>()
        {
            { AudioGroup.MusicVolume, true },
            { AudioGroup.EffectsVolume, true },
        };

        foreach (Paint paint in Enum.GetValues(typeof(Paint)))
        {
            int index = (int)paint;
            _customizationPreferences.Add(paint, new CustomizationPreferences(index + 1, noHatId, index));
        }
    }
}

