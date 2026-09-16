using System.Linq;
using SlimeGround.Data.ScriptableObjects.Leaderboard;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Integration.Leaderboards;
using SlimeGround.Menu.Windows.Leaderboard;

namespace SlimeGround.Data.Saves
{
	public class GameProgressUpdater
	{
		public GameProgressUpdater(LevelProgressTracker progressTracker, PlayerDataProvider playerData,
	                               LeaderboardProvider leaderboardProvider, LeaderboardSettings leaderboardSettings)
	    {
	        _progressTracker = progressTracker;
	        _playerData = playerData;
	        _leaderboardProvider = leaderboardProvider;
	        _leaderboardSettings = leaderboardSettings;

	        _playerScoreCalculator = new PlayerScoreCalculator(playerData);
	        _progressTracker.LevelFinished += UpdateSavedProgress;
	    }

		private LevelProgressTracker _progressTracker { get; }
		private PlayerDataProvider _playerData { get; }
		private LeaderboardProvider _leaderboardProvider { get; }
		private LeaderboardSettings _leaderboardSettings { get; }
		private PlayerScoreCalculator _playerScoreCalculator { get; }

		public void Dispose()
		{
			_progressTracker.LevelFinished -= UpdateSavedProgress;
		}

		private void UpdateSavedProgress(ILevelData levelData)
	    {
	        LevelProgress savedLevel = _playerData.Progress.Levels
	                                .FirstOrDefault(level => level.Id == levelData.LevelId);

	        bool isAngryTaskDone = _progressTracker.IsAngryTaskDone || savedLevel.IsAngryStarEarned;
	        bool isMoveTaskDone = _progressTracker.IsMoveTaskDone || savedLevel.IsMovesStarEarned;

	        int newGoldAmount = _playerData.Resources.GoldAmount + _progressTracker.ReachedGold;
	        int newScoreAmount = _playerData.Progress.ScoreAmount + _progressTracker.ReachedScore;
	        bool isNewTopScore = _progressTracker.ReachedScore > savedLevel.BestScore;
	        int levelScore = isNewTopScore ? _progressTracker.ReachedScore : savedLevel.BestScore;
	        LevelProgress updatedProgress = new LevelProgress(savedLevel.Id, true,
	                                                          isMoveTaskDone, isAngryTaskDone, levelScore);

	        _playerData.Resources.SetGoldAmount(newGoldAmount);
	        _playerData.Progress.SetScoreAmount(newScoreAmount);
	        _playerData.Progress.UpdateLevelProgress(updatedProgress);
	        _playerData.Save();

	        int totalScore = _playerScoreCalculator.GetScore(LeaderboardType.TotalGameScore);
	        _leaderboardProvider.SaveScore(_leaderboardSettings.LeaderboardKey(LeaderboardType.TotalGameScore), totalScore);

	        if (isNewTopScore)
	        {
	            int topResultScore = _playerScoreCalculator.GetScore(LeaderboardType.BestGameScore);
	            _leaderboardProvider.SaveScore(_leaderboardSettings.LeaderboardKey(LeaderboardType.BestGameScore), topResultScore);
	        }
	    }
	}
}
