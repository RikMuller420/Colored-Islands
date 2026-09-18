using SlimeGround.Gameplay.Levels;

namespace SlimeGround.Effects.Sound
{
	public class LevelEndSoundPlayer
	{
		private readonly LevelProgressTracker _progressTracker;
		private readonly GameplaySoundPlayer _gameplaySoundPlayer;

		public LevelEndSoundPlayer(LevelProgressTracker progressTracker, GameplaySoundPlayer gameplaySoundPlayer)
	    {
	        _progressTracker = progressTracker;
	        _gameplaySoundPlayer = gameplaySoundPlayer;
	        _progressTracker.LevelFinished += PlayWinSound;
	    }

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
