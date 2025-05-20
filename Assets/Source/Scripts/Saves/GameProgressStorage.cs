using System;
using System.Collections.Generic;
using System.Linq;
using YG;

public class GameProgressStorage
{
    private LevelSettings _levelSettings;
    private GameProgressSerializer _progressSerializer;
    private GameProgress _progress;

    public event Action GoldAmountChanged;
    public event Action LevelProgressChanged;
    public event Action<BoostType> BoostsAmountChanged;
    public event Action<UpgradeType> Upgraded;
    public event Action RemoveAdsStateChanged;

    public GameProgressStorage(LevelSettings levelSettings)
    {
        _levelSettings = levelSettings;
        _progressSerializer = new GameProgressSerializer();

        LoadSavedProgress();

        if (IsNewLevelsCreatedInBuild(_levelSettings.Levels, out List<LevelSettingsData> newLevels))
        {
            ActulizeGameProgress(newLevels);
            Save();
        }
    }

    public LevelProgress FirstUnfinishedLevel => Levels.FirstOrDefault(level => level.IsDone == false);
    public IReadOnlyCollection<LevelProgress> Levels => _progress.Levels;
    public int ScoreAmount => _progress.ScoreAmount;
    public int GoldAmount => _progress.GoldAmount;
    public bool IsAdsRemoved => _progress.IsAdsRemoved;

    public int GetBoostAmount(BoostType boostType)=> _progress.GetBoostAmount(boostType);
    public int GetUpgradeStage(UpgradeType upgradeType) => _progress.GetUpgradeStage(upgradeType);

    //Used Under TEST UI only
    public void ResetProgress()
    {
        CreateNewSave();

        GoldAmountChanged?.Invoke();
        LevelProgressChanged?.Invoke();
        BoostsAmountChanged?.Invoke(BoostType.GrowBuferIsland);
        BoostsAmountChanged?.Invoke(BoostType.FinishIsland);
        BoostsAmountChanged?.Invoke(BoostType.FreezeObjectives);
        BoostsAmountChanged?.Invoke(BoostType.ReducePaints);
        Upgraded?.Invoke(UpgradeType.BuferIslandSize);
        RemoveAdsStateChanged?.Invoke();
    }


    public void ApplyRemoveAddBonus(bool autoSave = true)
    {
        _progress.ApplyRemoveAddBonus();
        RemoveAdsStateChanged?.Invoke();

        if (autoSave)
        {
            Save();
        }
    }

    public void SetBoostAmount(BoostType boostType, int amount, bool isAutoSave = true)
    {
        _progress.SetBoostAmount(boostType, amount);
        BoostsAmountChanged?.Invoke(boostType);

        if (isAutoSave)
        {
            Save();
        }
    }

    public void SetUpgradeStage(UpgradeType upgradeType, int stage, bool autoSave = true)
    {
        _progress.SetUpgradeStage(upgradeType, stage);
        Upgraded?.Invoke(upgradeType);

        if (autoSave)
        {
            Save();
        }
    }

    public void SetGoldAmount(int amount, bool autoSave = true)
    {
        _progress.SetGoldAmount(amount);
        GoldAmountChanged?.Invoke();

        if (autoSave)
        {
            Save();
        }
    }

    public void SetScoreAmount(int amount, bool autoSave = true)
    {
        _progress.SetScoreAmount(amount);

        if (autoSave)
        {
            Save();
        }
    }

    public void UpdateLevelProgress(LevelProgress levelProgress, bool autoSave = true)
    {
        _progress.UpdateLevelProgress(levelProgress);
        LevelProgressChanged?.Invoke();

        if (autoSave)
        {
            Save();
        }
    }

    public void Save()
    {
        string json = _progressSerializer.Serialize(_progress);
        YandexGame.savesData.GameProgress = json;
        YandexGame.SaveProgress();
    }

    private void LoadSavedProgress()
    {
        string json = YandexGame.savesData.GameProgress;

        if (json != "")
        {
            try
            {
                _progress = _progressSerializer.Deserialize(json);
            }
            catch
            {
                CreateNewSave();
            }
        }
        else
        {
            CreateNewSave();
        }
    }

    private void CreateNewSave()
    {
        _progress = new GameProgress();

        foreach (LevelSettingsData level in _levelSettings.Levels)
        {
            _progress.AddLevel(new LevelProgress(level.Id));
        }

        Save();
    }

    private bool IsNewLevelsCreatedInBuild(IReadOnlyCollection<LevelSettingsData> actualLevels, 
                                            out List<LevelSettingsData> newLevels)
    {
        newLevels = new List<LevelSettingsData>();

        foreach (LevelSettingsData actualLevel in actualLevels)
        {
            bool isLevelSaved = _progress.Levels.Any(level => level.Id == actualLevel.Id);

            if (isLevelSaved == false)
            {
                newLevels.Add(actualLevel);
            }
        }

        return newLevels.Count != 0;
    }

    private void ActulizeGameProgress(List<LevelSettingsData> newLevels)
    {
        foreach (LevelSettingsData newLevel in newLevels)
        {
            _progress.AddLevel(new LevelProgress(newLevel.Id));
        }
    }
}
