using System.Linq;
using UnityEngine;
using static YG.InfoYG;

public class GameProgressUpdater
{
    private LevelProgressTracker _progressTracker;
    private GameProgressStorage _progressStorage;
    private LeaderboardProvider _leaderboardProvider;
    private LeaderboardSettings _leaderboardSettings;
    private LeaderboardScoreCalculator _scoreCalculator;

    public GameProgressUpdater(LevelProgressTracker progressTracker, GameProgressStorage progressStorage,
                               LeaderboardProvider leaderboardProvider, LeaderboardSettings leaderboardSettings,
                               LeaderboardScoreCalculator scoreCalculator)
    {
        _progressTracker = progressTracker;
        _progressStorage = progressStorage;
        _leaderboardProvider = leaderboardProvider;
        _leaderboardSettings = leaderboardSettings;
        _scoreCalculator = scoreCalculator;

        _progressTracker.LevelFinished += UpdateSavedProgress;
    }

    private void UpdateSavedProgress()
    {
        LevelProgress savedLevel = _progressStorage.Levels
                                .FirstOrDefault(level => level.Id == _progressTracker.LevelData.Id);

        bool isLevelFinished = _progressTracker.IsLevelFinished || savedLevel.IsDone;
        bool isAngryTaskDone = _progressTracker.IsAngryTaskDone || savedLevel.IsAngryTaskDone;
        bool isMoveTaskDone = _progressTracker.IsMoveTaskDone || savedLevel.IsMoveTaskDone;

        int newGoldAmount = _progressStorage.GoldAmount + _progressTracker.ReachedGold;
        int newScoreAmount = _progressStorage.ScoreAmount + _progressTracker.ReachedScore;
        bool isNewTopScore = _progressTracker.ReachedScore > savedLevel.BestScore;
        int levelScore = isNewTopScore ? _progressTracker.ReachedScore : savedLevel.BestScore;
        LevelProgress updatedProgress = new LevelProgress(savedLevel.Id, isLevelFinished,
                                                          isMoveTaskDone, isAngryTaskDone, levelScore);

        _progressStorage.SetGoldAmount(newGoldAmount, false);
        _progressStorage.SetScoreAmount(newScoreAmount, false);
        _progressStorage.UpdateLevelProgress(updatedProgress, true);

        int totalScore = _scoreCalculator.GetScore(LeaderboardType.TotalGameScore);
        _leaderboardProvider.SaveScore(_leaderboardSettings.LeaderboardKey(LeaderboardType.TotalGameScore), totalScore);

        if (isNewTopScore)
        {
            int topResultScore = _scoreCalculator.GetScore(LeaderboardType.BestGameScore);
            _leaderboardProvider.SaveScore(_leaderboardSettings.LeaderboardKey(LeaderboardType.BestGameScore), topResultScore);
        }
    }
}
