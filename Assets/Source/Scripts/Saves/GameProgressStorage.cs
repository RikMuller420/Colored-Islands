using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UI.TabSystem;

public class GameProgressStorage
{
    private LevelSettings _levelSettings;
    private UnitsFaceSettings _unitsFaceSettings;
    private UnitsHatSettings _unitsHatSettings;
    private LevelRewardSettings _levelRewardSettings;
    private GameProgressSerializer _progressSerializer;
    private GameProgress _progress;
    private SaveProvider _saveProvider;
    private GameProgressSaver _gameProgressSaver;

    private int _lastLevelId;

    public event Action GoldAmountChanged;
    public event Action<int> LevelProgressChanged;
    public event Action<BoostType> BoostsAmountChanged;
    public event Action<UpgradeType> Upgraded;
    public event Action<InAppType> EarnInAppWithAddProgressUpdated;

    public event Action RemoveAdsStateChanged;
    public event Action<AudioGroup> SoundEnabledChanged;
    public event Action<Paint> CustomizationPreferenceChanged;
    public event Action TrainingFinished;
    public event Action<int> FaceUnlocked;
    public event Action SpinCountChanged;

    public GameProgressStorage(LevelSettings levelSettings, UnitsFaceSettings unitsFaceSettings,
                            UnitsHatSettings unitsHatSettings, LevelRewardSettings levelRewardSettings,
                            SaveProvider saveProvider, GameProgressSaver gameProgressSaver)
    {
        _levelSettings = levelSettings;
        _unitsFaceSettings = unitsFaceSettings;
        _unitsHatSettings = unitsHatSettings;
        _levelRewardSettings = levelRewardSettings;
        _saveProvider = saveProvider;
        _gameProgressSaver = gameProgressSaver;
        _progressSerializer = new GameProgressSerializer();

        LoadSavedProgress();
        ActulizeSavedLevels();
        ActulizeSavedFaces();
        ActualizeSavedHats();
        ActualizeTrainingFinishedStatus();
        //ActualizeReceivedLevelRewards();

        _lastLevelId = Levels.Max(level => level.Id);
    }

    public int LastAvailableLevelId => Levels.FirstOrDefault(level => !level.IsDone)?.Id ?? _lastLevelId;
    public LevelProgress FirstUnfinishedLevel => Levels.FirstOrDefault(level => level.IsDone == false);
    public IReadOnlyCollection<LevelProgress> Levels => _progress.Levels;
    public int ScoreAmount => _progress.ScoreAmount;
    public int GoldAmount => _progress.GoldAmount;
    public bool IsAdsRemoved => _progress.IsAdsRemoved;
    public bool IsLanguageSaved => _progress.IsLanguageSaved;
    public Language Language => _progress.Language;
    public bool IsTrainingFinished => _progress.IsTrainingFinished;
    public int AviableSpinCount => _progress.AviableSpinCount;
    public IReadOnlyCollection<FaceAvailabilitie> FaceAvailabilities => _progress.FaceAvailabilities;
    public Dictionary<int, bool> IsHatWasUsed => _progress.WasHatsUsed;
    public bool WasHatUsed(int hatId) => _progress.WasHatsUsed.ContainsKey(hatId) ? _progress.WasHatsUsed[hatId] : true;
    public bool WasLevelRewardReceived(int levelId) => _progress.WasLevelRewardReceived.ContainsKey(levelId) ? _progress.WasLevelRewardReceived[levelId] : true;


    public CustomizationPreferences GetCustomizationPreference(Paint paint) => _progress.GetCustomizationPreference(paint);

    public int GetBoostAmount(BoostType boostType) => _progress.GetBoostAmount(boostType);
    public int GetUpgradeStage(UpgradeType upgradeType) => _progress.GetUpgradeStage(upgradeType);
    public int GetEarnedInAppWithAddProgress(InAppType inAppType) => _progress.GetEarnedInAppWithAddProgress(inAppType);
    public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _progress.GetIsSoundOnStatus(audioGroup);

    public void SetSpinCount(int spinCount)
    {
        _progress.SetSpinCount(spinCount);
        Save();
        SpinCountChanged?.Invoke();
    }

    public void SetTrainingFinished(bool isFinished)
    {
        _progress.SetTrainingFinished(isFinished);
        TrainingFinished?.Invoke();
    }

    public void Save() => _gameProgressSaver.TrySave(_progress);

    //Used Under TEST UI only
    public void ResetProgress()
    {
        CreateNewSave();

        GoldAmountChanged?.Invoke();
        LevelProgressChanged?.Invoke(0);
        BoostsAmountChanged?.Invoke(BoostType.GrowBuferIsland);
        BoostsAmountChanged?.Invoke(BoostType.FinishIsland);
        BoostsAmountChanged?.Invoke(BoostType.FreezeObjectives);
        BoostsAmountChanged?.Invoke(BoostType.ReducePaints);
        Upgraded?.Invoke(UpgradeType.BuferIslandSize);
        RemoveAdsStateChanged?.Invoke();
    }

    public void SetLanguage(Language language) => _progress.SetLanguage(language);
    public void MarkFaceUsed(int faceId) => _progress.MarkFaceUsed(faceId);
    public void MarkHatUsed(int hatId) => _progress.MarkHatUsed(hatId);

    public void MarkLevelRewardReceived(int levelId)
    {
        _progress.MarkLevelRewardReceived(levelId);
        Save();
    }

    public void UnlockFace(int faceId)
    {
        _progress.UnlockFace(faceId);
        FaceUnlocked?.Invoke(faceId);
    }

    public void ChangeCustomizationPreferenceFace(Paint paint, int faceId)
    {
        _progress.ChangeCustomizationPreferenceFace(paint, faceId);
        CustomizationPreferenceChanged?.Invoke(paint);
    }

    public void ChangeCustomizationPreferenceHat(Paint paint, int hatId)
    {
        _progress.ChangeCustomizationPreferenceHat(paint, hatId);
        CustomizationPreferenceChanged?.Invoke(paint);
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

    public void SetSoundToggle(AudioGroup audioGroup, bool isOn)
    {
        _progress.SetSoundEnabledStatus(audioGroup, isOn);
        SoundEnabledChanged?.Invoke(audioGroup);
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

    public void SetEarnInAppWithAddProgress(InAppType inAppType, int progress)
    {
        _progress.SetEarnInAppWithAddProgress(inAppType, progress);
        EarnInAppWithAddProgressUpdated?.Invoke(inAppType);
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
        LevelProgressChanged?.Invoke(levelProgress.Id);

        if (autoSave)
        {
            Save();
        }
    }

    private void LoadSavedProgress()
    {
        string json = _saveProvider.Load();

        if (json == null || json == "")
        {
            CreateNewSave();
        }
        else
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
    }

    private void CreateNewSave()
    {
        _progress = new GameProgress(_unitsHatSettings);

        foreach (LevelSettingsData level in _levelSettings.Levels)
        {
            _progress.AddLevel(new LevelProgress(level.Id));
        }

        foreach (UnitFaceData unitFace in _unitsFaceSettings.Faces)
        {
            _progress.AddFace(unitFace.Id, unitFace.IsAviableOnStart, unitFace.IsAviableOnStart);
        }

        foreach(UnitHatData unitHat in _unitsHatSettings.Hats)
        {
            _progress.AddHat(unitHat.Id, false);
        }

        foreach (LevelRewardData reward in _levelRewardSettings.LevelRewards)
        {
            _progress.AddLevelReward(reward.LevelId, false);
        }

        Save();
    }

    private void ActulizeSavedLevels()
    {
        foreach (LevelSettingsData actualLevel in _levelSettings.Levels)
        {
            bool isLevelSaved = _progress.Levels.Any(level => level.Id == actualLevel.Id);

            if (isLevelSaved == false)
            {
                _progress.AddLevel(new LevelProgress(actualLevel.Id));
                Save();
            }
        }
    }

    private void ActulizeSavedFaces()
    {
        foreach (UnitFaceData actualFace in _unitsFaceSettings.Faces)
        {
            FaceAvailabilitie savedFace = _progress.FaceAvailabilities.FirstOrDefault(face => face.FaceId == actualFace.Id);

            if (savedFace == null)
            {
                _progress.AddFace(actualFace.Id, actualFace.IsAviableOnStart, actualFace.IsAviableOnStart);
                Save(); 
            }
            else
            {
                if (actualFace.IsAviableOnStart && savedFace.IsAviable == false)
                {
                    _progress.UnlockFace(actualFace.Id);
                    _progress.MarkFaceUsed(actualFace.Id);
                    Save();
                }
            }
        }
    }

    private void ActualizeSavedHats()
    {
        foreach (UnitHatData actualHat in _unitsHatSettings.Hats)
        {
            bool isHatSaved = _progress.WasHatsUsed.ContainsKey(actualHat.Id);

            if (isHatSaved == false)
            {
                _progress.AddHat(actualHat.Id, false);
                Save();
            }
        }
    }

    private void ActualizeReceivedLevelRewards()
    {
        foreach (LevelRewardData actualReward in _levelRewardSettings.LevelRewards)
        {
            bool isRewardSaved = _progress.WasLevelRewardReceived.ContainsKey(actualReward.LevelId);

            if (isRewardSaved == false)
            {
                _progress.AddLevelReward(actualReward.LevelId, false);
                Save();
            }
        }
    }

    private void ActualizeTrainingFinishedStatus()
    {
        if (IsTrainingFinished == false && LastAvailableLevelId > _levelSettings.LastTrainingLevel)
        {
            _progress.SetTrainingFinished(true);
            Save();
        }
    }
}
