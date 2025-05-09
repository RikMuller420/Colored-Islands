using System.Linq;

public class LevelProgressUpdater
{
    private LevelProgressTracker _progressTracker;
    private GameProgressStorage _progressStorage;

    public LevelProgressUpdater(LevelProgressTracker progressTracker, GameProgressStorage progressStorage)
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

        _progressStorage.UpdateLevelProgress(updatedProgress, false);
        _progressStorage.SetGoldAmount(newGoldAmount, false);
        _progressStorage.SetScoreAmount(newScoreAmount, true);
    }
}
