using System.Linq;
using YG;

public class GameProgressUpdater
{
    private const string LeaderboardKey = "Leaderboard";

    private LevelProgressTracker _progressTracker;
    private GameProgressStorage _progressStorage;

    public GameProgressUpdater(LevelProgressTracker progressTracker, GameProgressStorage progressStorage)
    {
        _progressTracker = progressTracker;
        _progressStorage = progressStorage;

        _progressTracker.LevelFinished += UpdateSavedProgress;
        _progressTracker.LevelFailed += UpdateSavedProgress;
    }

    private void UpdateSavedProgress()
    {
        LevelProgress savedLevel = _progressStorage.Levels
                                .FirstOrDefault(level => level.Id == _progressTracker.LevelData.Id);

        bool isLevelFinished = _progressTracker.IsLevelFinished || savedLevel.IsDone;
        bool isTimeTaskDone = _progressTracker.IsTimeTaskDone || savedLevel.IsTimeTaskDone;
        bool isMoveTaskDone = _progressTracker.IsMoveTaskDone || savedLevel.IsMoveTaskDone;

        LevelProgress updatedProgress = new LevelProgress(savedLevel.Id, isLevelFinished, isMoveTaskDone, isTimeTaskDone);
        int newGoldAmount = _progressStorage.GoldAmount + _progressTracker.ReachedGold;
        int newScoreAmount = _progressStorage.ScoreAmount + _progressTracker.ReachedScore;

        _progressStorage.SetGoldAmount(newGoldAmount, false);
        _progressStorage.SetScoreAmount(newScoreAmount, false);
        _progressStorage.UpdateLevelProgress(updatedProgress, true);

        YandexGame.NewLeaderboardScores(LeaderboardKey, _progressStorage.ScoreAmount);
    }
}
