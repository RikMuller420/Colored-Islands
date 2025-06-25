using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class GameProgress
{
    [JsonProperty] private List<LevelProgress> _levels;
    [JsonProperty] private int _scoreAmount;
    [JsonProperty] private int _goldAmount;
    [JsonProperty] private bool _isAdsRemoved;
    [JsonProperty] private bool _isLanguageSaved;
    [JsonProperty] private Language _language;
    [JsonProperty] private bool _isTrainingFinished;
    [JsonProperty] private Dictionary<BoostType, int> _boostsAmounts;
    [JsonProperty] private Dictionary<UpgradeType, int> _upgradeStages;
    [JsonProperty] private Dictionary<AudioGroup, bool> _isSoundOnStatus;
    [JsonProperty] private Dictionary<int, bool> _facesAvailabilities;
    [JsonProperty] private Dictionary<Paint, CustomizationPreferences> _customizationPreferences;

    public GameProgress()
    {
        SetDefaultValues();
    }

    public GameProgress(UnitsHatSettings unitsHatSettings)
    {
        SetDefaultValues(unitsHatSettings.NoHatId);
    }

    [JsonIgnore] public LevelProgress FirstUnfinishedLevel => _levels.FirstOrDefault(level => !level.IsDone);
    [JsonIgnore] public IReadOnlyCollection<LevelProgress> Levels => _levels.AsReadOnly();
    [JsonIgnore] public int ScoreAmount => _scoreAmount;
    [JsonIgnore] public int GoldAmount => _goldAmount;
    [JsonIgnore] public bool IsAdsRemoved => _isAdsRemoved;
    [JsonIgnore] public bool IsLanguageSaved => _isLanguageSaved;
    [JsonIgnore] public Language Language => _language;
    [JsonIgnore] public bool IsTrainingFinished => _isTrainingFinished;
    [JsonIgnore] public Dictionary<int, bool> FacesAvailabilities => _facesAvailabilities;

    public int GetBoostAmount(BoostType boostType) => _boostsAmounts[boostType];
    public int GetUpgradeStage(UpgradeType upgradeType) => _upgradeStages[upgradeType];
    public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _isSoundOnStatus[audioGroup];
    public CustomizationPreferences GetCustomizationPreference(Paint paint) => _customizationPreferences[paint];

    public void SetTrainingFinished(bool isFinished)
    {
        _isTrainingFinished = isFinished;
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
        _facesAvailabilities[faceId] = true;
    }

    public void ChangeCustomizationPreferenceFace(Paint paint, int faceId)
    {
        CustomizationPreferences preference = GetCustomizationPreference(paint);
        
        int hatId = preference.HatId;
        _customizationPreferences[paint] = new CustomizationPreferences(faceId, hatId);

    }

    public void ChangeCustomizationPreferenceHat(Paint paint, int hatId)
    {
        CustomizationPreferences preference = GetCustomizationPreference(paint);

        int faceId = preference.FaceId;
        _customizationPreferences[paint] = new CustomizationPreferences(faceId, hatId);
    }

    public void AddFace(int faceId, bool isAviable)
    {
        _facesAvailabilities.Add(faceId, isAviable);
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
        _facesAvailabilities = new Dictionary<int, bool>();
        _customizationPreferences = new Dictionary<Paint, CustomizationPreferences>();
        _scoreAmount = 0;
        _goldAmount = 0;
        _isAdsRemoved = false;
        _boostsAmounts = new Dictionary<BoostType, int>()
        {
            { BoostType.FinishIsland, 0 },
            { BoostType.FreezeObjectives, 0 },
            { BoostType.GrowBuferIsland, 0 },
            { BoostType.ReducePaints, 0 }
        };
        _upgradeStages = new Dictionary<UpgradeType, int>()
        {
            { UpgradeType.BuferIslandSize, 0 }
        };
        _isSoundOnStatus = new Dictionary<AudioGroup, bool>()
        {
            { AudioGroup.MusicVolume, true },
            { AudioGroup.EffectsVolume, true },
        };
        foreach (Paint paint in Enum.GetValues(typeof(Paint)))
        {
            _customizationPreferences.Add(paint, new CustomizationPreferences((int)paint + 1, noHatId));
        }
    }
}

