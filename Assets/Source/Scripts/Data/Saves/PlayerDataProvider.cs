using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.LevelRewards;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using SlimeGround.Effects.Sound;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Integration.Localization;
using SlimeGround.Integration.Saves;
using SlimeGround.Menu.Windows.Customization;
using SlimeGround.Menu.Windows.InAppPurchase;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Data.Saves
{
	public class PlayerDataProvider : MonoBehaviour, IPlayerData
	{
	    public const string SaveSignatureKey = "TestVersion3";

	    [SerializeField] private LevelSettings _levelSettings;
	    [SerializeField] private UnitsFaceSettings _unitsFaceSettings;
	    [SerializeField] private UnitsHatSettings _unitsHatSettings;
	    [SerializeField] private LevelRewardSettings _levelRewardSettings;
	    [SerializeField] private PlayerDataSaver _playerDataSaver;

	    private PlayerData _playerData;
	    private SaveProvider _saveProvider;

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

	    public void Initialize()
	    {
	        _saveProvider = new SaveProvider();
	        _playerDataSaver.Initialize(_saveProvider);

	        PlayerDataReader playerDataReader = new PlayerDataReader(_levelSettings, _unitsFaceSettings, _unitsHatSettings,
	                                                                 _levelRewardSettings, _saveProvider);
	        _playerData = playerDataReader.GetData();
	        Save();
	    }

	    public void Save() => _playerDataSaver.TrySave(_playerData);

	    public int LastAvailableLevelId => Levels.FirstOrDefault(level => level.IsDone == false)?.Id ?? Levels.Max(level => level.Id);
	    public LevelProgress FirstUnfinishedLevel => Levels.FirstOrDefault(level => level.IsDone == false);
	    public bool IsTrainingFinished => _playerData.IsTrainingFinished;
	    public IReadOnlyCollection<LevelProgress> Levels => _playerData.Levels;
	    public int GoldAmount => _playerData.GoldAmount;
	    public int ScoreAmount => _playerData.ScoreAmount;
	    public bool IsAdsRemoved => _playerData.IsAdsRemoved;
	    public int AviableSpinCount => _playerData.AviableSpinCount;
	    public IReadOnlyCollection<FaceAvailabilitie> FaceAvailabilities => _playerData.FaceAvailabilities;
	    public bool WasHatUsed(int hatId) => _playerData.WasHatsUsed.ContainsKey(hatId) ? _playerData.WasHatsUsed[hatId] : true;
	    public bool WasLevelRewardReceived(int levelId) => _playerData.WasLevelRewardReceived.ContainsKey(levelId) ? _playerData.WasLevelRewardReceived[levelId] : true;
	    public bool IsLanguageSaved => _playerData.IsLanguageSaved;
	    public Language Language => _playerData.Language;
	    public CustomizationPreferences GetCustomizationPreference(UnitSlotType unitSlot) => _playerData.CustomizationPreferences[unitSlot];
	    public int GetBoostAmount(BoostType boostType) => _playerData.BoostsAmounts[boostType];
	    public int GetUpgradeStage(UpgradeType upgradeType) => _playerData.UpgradeStages[upgradeType];
	    public int GetEarnedInAppWithAddProgress(InAppType inAppType) => _playerData.EarnInAppWithAddProgress[inAppType];
	    public bool GetIsSoundOnStatus(AudioGroup audioGroup) => _playerData.IsSoundOnStatus[audioGroup];

	    public void MarkHatUsed(int hatId) => _playerData.WasHatsUsed[hatId] = true;
	    public void MarkLevelRewardReceived(int levelId) => _playerData.WasLevelRewardReceived[levelId] = true;

	    public void UnlockFace(int faceId)
	    {
	        FaceAvailabilitie face = _playerData.FaceAvailabilities.Find(face => face.FaceId == faceId);
	        FaceAvailabilitie newFace = new FaceAvailabilitie(face.FaceId, true, face.WasUsed);

	        _playerData.FaceAvailabilities.Remove(face);
	        _playerData.FaceAvailabilities.Add(newFace);

	        FaceUnlocked?.Invoke(faceId);
	    }

	    public void MarkFaceUsed(int faceId)
	    {
	        FaceAvailabilitie face = _playerData.FaceAvailabilities.Find(face => face.FaceId == faceId);
	        FaceAvailabilitie newFace = new FaceAvailabilitie(face.FaceId, face.IsAviable, true);

	        _playerData.FaceAvailabilities.Remove(face);
	        _playerData.FaceAvailabilities.Add(newFace);
	    }

	    public void SetLanguage(Language language)
	    {
	        _playerData.Language = language;
	        _playerData.IsLanguageSaved = true;
	    }

	    public void SetScoreAmount(int amount)
	    {
	        if (amount < 0)
	        {
	            throw new ArgumentException(nameof(amount));
	        }

	        _playerData.ScoreAmount = amount;
	    }

	    public void SetSpinCount(int spinCount)
	    {
	        _playerData.AviableSpinCount = spinCount;
	        SpinCountChanged?.Invoke();
	    }

	    public void SetTrainingFinished()
	    {
	        _playerData.IsTrainingFinished = true;
	        TrainingFinished?.Invoke();
	    }

	    public void ChangeCustomizationPreferenceFace(UnitSlotType unitSlot, int faceId)
	    {
	        CustomizationPreferences preference = _playerData.CustomizationPreferences[unitSlot];
	        int hatId = preference.HatId;
	        _playerData.CustomizationPreferences[unitSlot] = new CustomizationPreferences(faceId, hatId, preference.ColorSample);

	        CustomizationPreferenceChanged?.Invoke(unitSlot);
	    }

	    public void ChangeCustomizationPreferenceHat(UnitSlotType unitSlot, int hatId)
	    {
	        CustomizationPreferences preference = _playerData.CustomizationPreferences[unitSlot];
	        int faceId = preference.FaceId;
	        _playerData.CustomizationPreferences[unitSlot] = new CustomizationPreferences(faceId, hatId, preference.ColorSample);

	        CustomizationPreferenceChanged?.Invoke(unitSlot);
	    }

	    public void ChangeCustomizationPreferenceColor(UnitSlotType unitSlot, ColorSample colorSample)
	    {
	        CustomizationPreferences preference = _playerData.CustomizationPreferences[unitSlot];
	        int hatId = preference.HatId;
	        int faceId = preference.FaceId;
	        _playerData.CustomizationPreferences[unitSlot] = new CustomizationPreferences(faceId, hatId, colorSample);

	        CustomizationPreferenceChanged?.Invoke(unitSlot);
	    }

	    public void ApplyRemoveAddBonus()
	    {
	        _playerData.IsAdsRemoved = true;
	        RemoveAdsStateChanged?.Invoke();
	    }

	    public void SetSoundToggle(AudioGroup audioGroup, bool isOn)
	    {
	        _playerData.IsSoundOnStatus[audioGroup] = isOn;
	        SoundEnabledChanged?.Invoke(audioGroup);
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

	    public void UpdateLevelProgress(LevelProgress levelProgress)
	    {
	        int index = _playerData.Levels.FindIndex(level => level.Id == levelProgress.Id);
	        _playerData.Levels[index] = levelProgress;

	        LevelProgressChanged?.Invoke(levelProgress.Id);
	    }
	}
}
