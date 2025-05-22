using System.Linq;

public class GameProgressUpdater
{
    private const string AllTimeLeaderboardKey = "LeaderboardAllTime";
    private const string TopResultLeaderboardKey = "LeaderboardAbsolute";

    private LevelProgressTracker _progressTracker;
    private GameProgressStorage _progressStorage;
    private LeaderboardProvider _leaderboardProvider;

    public GameProgressUpdater(LevelProgressTracker progressTracker, GameProgressStorage progressStorage,
                               LeaderboardProvider leaderboardProvider)
    {
        _progressTracker = progressTracker;
        _progressStorage = progressStorage;
        _leaderboardProvider = leaderboardProvider;

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

        int newGoldAmount = _progressStorage.GoldAmount + _progressTracker.ReachedGold;
        int newScoreAmount = _progressStorage.ScoreAmount + _progressTracker.ReachedScore;
        bool isNewTopScore = _progressTracker.ReachedScore > savedLevel.BestScore;
        int levelScore = isNewTopScore ? _progressTracker.ReachedScore : savedLevel.BestScore;
        LevelProgress updatedProgress = new LevelProgress(savedLevel.Id, isLevelFinished,
                                                          isMoveTaskDone, isTimeTaskDone, levelScore);

        _progressStorage.SetGoldAmount(newGoldAmount, false);
        _progressStorage.SetScoreAmount(newScoreAmount, false);
        _progressStorage.UpdateLevelProgress(updatedProgress, true);

        _leaderboardProvider.SaveScore(AllTimeLeaderboardKey, newScoreAmount);

        if (isNewTopScore)
        {
            int topResultScore = _progressStorage.Levels.Sum(level => level.BestScore); ;
            _leaderboardProvider.SaveScore(TopResultLeaderboardKey, topResultScore);
        }
    }
}
