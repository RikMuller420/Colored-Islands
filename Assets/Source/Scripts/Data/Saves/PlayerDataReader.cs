using System;
using System.Linq;
using Newtonsoft.Json;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.LevelRewards;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using SlimeGround.Integration.Saves;
using SlimeGround.Menu.Windows.Customization;

namespace SlimeGround.Data.Saves
{
	public class PlayerDataReader
	{
		private const string SaveSignatureKey = "TestVersion5";

		private readonly LevelSettings _levelSettings;
		private readonly UnitsFaceSettings _unitsFaceSettings;
		private readonly UnitsHatSettings _unitsHatSettings;
		private readonly LevelRewardSettings _levelRewardSettings;
		private readonly SaveProvider _saveProvider;

		public PlayerDataReader(LevelSettings levelSettings, UnitsFaceSettings unitsFaceSettings,
	                            UnitsHatSettings unitsHatSettings, LevelRewardSettings levelRewardSettings,
	                            SaveProvider saveProvider)
	    {
	        _levelSettings = levelSettings;
	        _unitsFaceSettings = unitsFaceSettings;
	        _unitsHatSettings = unitsHatSettings;
	        _levelRewardSettings = levelRewardSettings;
	        _saveProvider = saveProvider;
	    }

		public PlayerData GetData()
	    {
	        PlayerData playerData = LoadSavedProgress();

	        ActulizeSavedLevels(playerData);
	        ActulizeSavedFaces(playerData);
	        ActualizeSavedHats(playerData);
	        ActualizeTrainingFinishedStatus(playerData);
	        ActualizeReceivedLevelRewards(playerData);

	        return playerData;
	    }

	    private PlayerData LoadSavedProgress()
	    {
	        string json = _saveProvider.Load();
	        PlayerData playerData = null;

	        if (json == null || json == "")
	        {
	            playerData = CreateNewSave();
	        }
	        else
	        {
	            try
	            {
	                playerData = JsonConvert.DeserializeObject<PlayerData>(json);

					if (playerData.SaveSignatureKey != SaveSignatureKey)
	                {
	                    playerData = CreateNewSave();
	                }
	            }
	            catch
	            {
	                playerData = CreateNewSave();
	            }
	        }

	        return playerData;
	    }

	    private PlayerData CreateNewSave()
	    {
	        PlayerData playerData = new PlayerData(_unitsHatSettings);
	        playerData.SaveSignatureKey = SaveSignatureKey;

	        foreach (LevelSettingsData level in _levelSettings.Levels)
	        {
	            AddLevel(playerData, new LevelProgress(level.Id));
	        }

	        foreach (UnitFaceData unitFace in _unitsFaceSettings.Faces)
	        {
	            AddFace(playerData, unitFace.Id, unitFace.IsAviableOnStart, unitFace.IsAviableOnStart);
	        }

	        foreach (UnitHatData unitHat in _unitsHatSettings.Hats)
	        {
	            AddHat(playerData, unitHat.Id, false);
	        }

	        foreach (LevelRewardData reward in _levelRewardSettings.LevelRewards)
	        {
	            AddLevelReward(playerData, reward.LevelId, false);
	        }

	        return playerData;
	    }

	    private void ActulizeSavedLevels(PlayerData playerData)
	    {
	        foreach (LevelSettingsData actualLevel in _levelSettings.Levels)
	        {
	            bool isLevelSaved = playerData.Levels.Any(level => level.Id == actualLevel.Id);

	            if (isLevelSaved == false)
	            {
	                AddLevel(playerData, new LevelProgress(actualLevel.Id));
	            }
	        }
	    }

	    private void ActulizeSavedFaces(PlayerData playerData)
	    {
	        foreach (UnitFaceData actualFace in _unitsFaceSettings.Faces)
	        {
	            FaceAvailabilitie savedFace = playerData.FaceAvailabilities.FirstOrDefault(face => face.FaceId == actualFace.Id);

	            if (savedFace == null)
	            {
	                AddFace(playerData, actualFace.Id, actualFace.IsAviableOnStart, actualFace.IsAviableOnStart);
	            }
	            else
	            {
	                if (actualFace.IsAviableOnStart && savedFace.IsAviable == false)
	                {
	                    FaceAvailabilitie newFace = new FaceAvailabilitie(savedFace.FaceId, true, true);
	                    playerData.FaceAvailabilities.Remove(savedFace);
	                    playerData.FaceAvailabilities.Add(newFace);
	                }
	            }
	        }
	    }

	    private void ActualizeSavedHats(PlayerData playerData)
	    {
	        foreach (UnitHatData actualHat in _unitsHatSettings.Hats)
	        {
	            bool isHatSaved = playerData.IsHatsUsed.ContainsKey(actualHat.Id);

	            if (isHatSaved == false)
	            {
	                AddHat(playerData, actualHat.Id, false);
	            }
	        }
	    }

	    private void ActualizeReceivedLevelRewards(PlayerData playerData)
	    {
	        foreach (LevelRewardData actualReward in _levelRewardSettings.LevelRewards)
	        {
	            bool isRewardSaved = playerData.IsLevelRewardReceived.ContainsKey(actualReward.LevelId);

	            if (isRewardSaved == false)
	            {
	                AddLevelReward(playerData, actualReward.LevelId, false);
	            }
	        }
	    }

	    private void ActualizeTrainingFinishedStatus(PlayerData playerData)
	    {
	        int maxLevelId = playerData.Levels.Max(level => level.Id);
	        int lastAviableLevel = playerData.Levels.FirstOrDefault(level => level.IsDone == false)?.Id ?? maxLevelId;

	        if (playerData.IsTrainingFinished == false && lastAviableLevel > _levelSettings.LastTrainingLevel)
	        {
	            playerData.IsTrainingFinished = true;
	        }
	    }

		private void AddFace(PlayerData playerData, int faceId, bool isAviable, bool wasUsed)
		{
			playerData.FaceAvailabilities.Add(new FaceAvailabilitie(faceId, isAviable, wasUsed));
		}

		private void AddLevelReward(PlayerData playerData, int levelId, bool wasReceived)
		{
			playerData.IsLevelRewardReceived.Add(levelId, wasReceived);
		}

		private void AddHat(PlayerData playerData, int hatId, bool wasUsed)
		{
			playerData.IsHatsUsed.Add(hatId, wasUsed);
		}

		private void AddLevel(PlayerData playerData, LevelProgress levelProgress)
		{
			if (levelProgress == null)
			{
				throw new ArgumentNullException(nameof(levelProgress));
			}

			playerData.Levels.Add(levelProgress);
		}
	}
}