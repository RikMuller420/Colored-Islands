using System.Linq;

public class GameProgressUpdater
{
    private LevelProgressTracker _progressTracker;
    private PlayerDataProvider _playerData;
    private LeaderboardProvider _leaderboardProvider;
    private LeaderboardSettings _leaderboardSettings;
    private PlayerScoreCalculator _playerScoreCalculator;

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

    private void UpdateSavedProgress(ILevelData levelData)
    {
        LevelProgress savedLevel = _playerData.Levels
                                .FirstOrDefault(level => level.Id == levelData.LevelId);

        bool isAngryTaskDone = _progressTracker.IsAngryTaskDone || savedLevel.IsAngryStarEarned;
        bool isMoveTaskDone = _progressTracker.IsMoveTaskDone || savedLevel.IsMovesStarEarned;

        int newGoldAmount = _playerData.GoldAmount + _progressTracker.ReachedGold;
        int newScoreAmount = _playerData.ScoreAmount + _progressTracker.ReachedScore;
        bool isNewTopScore = _progressTracker.ReachedScore > savedLevel.BestScore;
        int levelScore = isNewTopScore ? _progressTracker.ReachedScore : savedLevel.BestScore;
        LevelProgress updatedProgress = new LevelProgress(savedLevel.Id, true,
                                                          isMoveTaskDone, isAngryTaskDone, levelScore);

        _playerData.SetGoldAmount(newGoldAmount);
        _playerData.SetScoreAmount(newScoreAmount);
        _playerData.UpdateLevelProgress(updatedProgress);
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
