public class LevelEndSoundPlayer
{
    private LevelProgressTracker _progressTracker;
    private GameplaySoundPlayer _gameplaySoundPlayer;

    public LevelEndSoundPlayer(LevelProgressTracker progressTracker, GameplaySoundPlayer gameplaySoundPlayer)
    {
        _progressTracker = progressTracker;
        _gameplaySoundPlayer = gameplaySoundPlayer;
        _progressTracker.LevelFinished += PlayWinSound;
        _progressTracker.LevelFailed += PlayFailSound;
    }

    private void PlayWinSound()
    {
        _gameplaySoundPlayer.PlaySound(GameplaySoundType.WinSound);
    }

    private void PlayFailSound()
    {
        _gameplaySoundPlayer.PlaySound(GameplaySoundType.FailSound);
    }
}
