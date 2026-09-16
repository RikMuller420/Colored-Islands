using System;
using System.Collections.Generic;
using System.Linq;

namespace SlimeGround.Data.Saves
{
	public class GameProgressProvider
	{
		public event Action<int> LevelProgressChanged;
		public event Action TrainingFinished;

		public GameProgressProvider(PlayerData playerData)
		{
			_playerData = playerData;
		}

		private PlayerData _playerData { get; }

		public LevelProgress FirstUnfinishedLevel => Levels.FirstOrDefault(level => level.IsDone == false);
		public bool IsCustomizationWindowWasOpened => _playerData.IsCustomizationWindowWasOpened;
		public bool IsTrainingFinished => _playerData.IsTrainingFinished;
		public IReadOnlyCollection<LevelProgress> Levels => _playerData.Levels;

		public void MarkLevelRewardReceived(int levelId) => _playerData.IsLevelRewardReceived[levelId] = true;
		public int ScoreAmount => _playerData.ScoreAmount;


		public int GetLastAvailableLevelId()
		{
			LevelProgress level = Levels.FirstOrDefault(level => level.IsDone == false);

			if (level == null)
			{
				return Levels.Max(level => level.Id);
			}

			return level.Id;
		}

		public bool IsLevelRewardReceived(int levelId)
		{
			return _playerData.IsLevelRewardReceived.ContainsKey(levelId) ?
										_playerData.IsLevelRewardReceived[levelId]
										: true;
		}

		public void SetScoreAmount(int amount)
		{
			if (amount < 0)
			{
				throw new ArgumentException(nameof(amount));
			}

			_playerData.ScoreAmount = amount;
		}

		public void UpdateLevelProgress(LevelProgress levelProgress)
		{
			int index = _playerData.Levels.FindIndex(level => level.Id == levelProgress.Id);
			_playerData.Levels[index] = levelProgress;

			LevelProgressChanged?.Invoke(levelProgress.Id);
		}

		public void SetTrainingFinished()
		{
			_playerData.IsTrainingFinished = true;
			TrainingFinished?.Invoke();
		}
		public void SetCustomizationWindowWasOpened()
		{
			_playerData.IsCustomizationWindowWasOpened = true;
		}
	}
}
