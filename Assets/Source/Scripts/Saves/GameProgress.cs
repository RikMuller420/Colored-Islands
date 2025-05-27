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
    [JsonProperty] private Dictionary<BoostType, int> _boostsAmounts;
    [JsonProperty] private Dictionary<UpgradeType, int> _upgradeStages;
    [JsonProperty] private Dictionary<AudioGroup, bool> _isSoundOnStatus;

    public GameProgress()
    {
        _levels = new List<LevelProgress>();
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
    }

    [JsonIgnore] public LevelProgress FirstUnfinishedLevel => _levels.FirstOrDefault(level => !level.IsDone);
    [JsonIgnore] public IReadOnlyCollection<LevelProgress> Levels => _levels.AsReadOnly();
    [JsonIgnore] public int ScoreAmount => _scoreAmount;
    [JsonIgnore] public int GoldAmount => _goldAmount;
    [JsonIgnore] public bool IsAdsRemoved => _isAdsRemoved;

    public int GetBoostAmount(BoostType boostType) => _boostsAmounts[boostType];
    public int GetUpgradeStage(UpgradeType upgradeType) => _upgradeStages[upgradeType];
    public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _isSoundOnStatus[audioGroup];

    public void ApplyRemoveAddBonus()
    {
        _isAdsRemoved = true;
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
}

