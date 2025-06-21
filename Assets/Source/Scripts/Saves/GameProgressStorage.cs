using System;
using System.Collections.Generic;
using System.Linq;

public class GameProgressStorage
{
    private LevelSettings _levelSettings;
    private UnitsFaceSettings _unitsFaceSettings;
    private UnitsHatSettings _unitsHatSettings;
    private GameProgressSerializer _progressSerializer;
    private GameProgress _progress;
    private SaveProvider _saveProvider;
    private GameProgressSaver _gameProgressSaver;

    public event Action GoldAmountChanged;
    public event Action LevelProgressChanged;
    public event Action<BoostType> BoostsAmountChanged;
    public event Action<UpgradeType> Upgraded;
    public event Action RemoveAdsStateChanged;
    public event Action<AudioGroup> SoundEnabledChanged;
    public event Action<Paint> CustomizationPreferenceChanged;

    public GameProgressStorage(LevelSettings levelSettings, UnitsFaceSettings unitsFaceSettings,
                            UnitsHatSettings unitsHatSettings,
                            SaveProvider saveProvider, GameProgressSaver gameProgressSaver)
    {
        _levelSettings = levelSettings;
        _unitsFaceSettings = unitsFaceSettings;
        _unitsHatSettings = unitsHatSettings;
        _saveProvider = saveProvider;
        _gameProgressSaver = gameProgressSaver;
        _progressSerializer = new GameProgressSerializer();

        LoadSavedProgress();
        ActulizeSavedLevels();
        ActulizeSavedFaces();
    }

    public LevelProgress FirstUnfinishedLevel => Levels.FirstOrDefault(level => level.IsDone == false);
    public IReadOnlyCollection<LevelProgress> Levels => _progress.Levels;
    public int ScoreAmount => _progress.ScoreAmount;
    public int GoldAmount => _progress.GoldAmount;
    public bool IsAdsRemoved => _progress.IsAdsRemoved;
    public bool IsLanguageSaved => _progress.IsLanguageSaved;
    public Language Language => _progress.Language;
    public Dictionary<int, bool> FacesAvailabilities => _progress.FacesAvailabilities;
    public CustomizationPreferences GetCustomizationPreference(Paint paint) => _progress.GetCustomizationPreference(paint);

    public int GetBoostAmount(BoostType boostType) => _progress.GetBoostAmount(boostType);
    public int GetUpgradeStage(UpgradeType upgradeType) => _progress.GetUpgradeStage(upgradeType);
    public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _progress.GetIsSoundOnStatus(audioGroup);

    public void Save() => _gameProgressSaver.TrySave(_progress);

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

    public void SetLanguage(Language language) => _progress.SetLanguage(language);
    public void UnlockFace(int faceId) => _progress.UnlockFace(faceId);

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
            _progress.AddFace(unitFace.Id, unitFace.IsAviableOnStart);
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
            bool isFaceSaved = _progress.FacesAvailabilities.ContainsKey(actualFace.Id);

            if (isFaceSaved)
            {
                if (actualFace.IsAviableOnStart && _progress.FacesAvailabilities[actualFace.Id] == false)
                {
                    _progress.UnlockFace(actualFace.Id);
                    Save();
                }
            }
            else
            {
                _progress.AddFace(actualFace.Id, actualFace.IsAviableOnStart);
                Save();
            }
        }
    }
}
