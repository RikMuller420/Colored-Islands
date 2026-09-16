using SlimeGround.Gameplay.Levels;

namespace SlimeGround.Effects.Sound
{
	public class LevelEndSoundPlayer
	{
		public LevelEndSoundPlayer(LevelProgressTracker progressTracker, GameplaySoundPlayer gameplaySoundPlayer)
	    {
	        _progressTracker = progressTracker;
	        _gameplaySoundPlayer = gameplaySoundPlayer;
	        _progressTracker.LevelFinished += PlayWinSound;
	    }

		private LevelProgressTracker _progressTracker { get; }
		private GameplaySoundPlayer _gameplaySoundPlayer { get; }

		public void Dispose()
		{
			_progressTracker.LevelFinished -= PlayWinSound;
		}

		private void PlayWinSound(ILevelData _)
	    {
	        _gameplaySoundPlayer.PlaySound(GameplaySoundType.WinSound);
	    }
	}
}
