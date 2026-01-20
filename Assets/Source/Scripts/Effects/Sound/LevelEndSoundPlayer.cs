public class LevelEndSoundPlayer
{
    private LevelProgressTracker _progressTracker;
    private GameplaySoundPlayer _gameplaySoundPlayer;

    public LevelEndSoundPlayer(LevelProgressTracker progressTracker, GameplaySoundPlayer gameplaySoundPlayer)
    {
        _progressTracker = progressTracker;
        _gameplaySoundPlayer = gameplaySoundPlayer;
        _progressTracker.LevelFinished += PlayWinSound;
    }

    private void PlayWinSound(ILevelData _)
    {
        _gameplaySoundPlayer.PlaySound(GameplaySoundType.WinSound);
    }
}
