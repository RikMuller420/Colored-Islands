using SlimeGround.Effects.Sound;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Effects
{
	public class EffectsInitializer : MonoBehaviour
	{
	    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
	    [SerializeField] private LevelProgressTracker _levelProgressTracker;
	    [SerializeField] private IslandFinishParticlePool _islandFinishParticlePool;

	    [SerializeField] private BackgroundMusicChanger _backgroundMusicChanger;
	    [SerializeField] private UnitsMoveSoundPlayer _unitsMoveSoundPlayer;
	    [SerializeField] private AudioSource _unitMoveSound;
	    [SerializeField] private GameplaySoundPlayer _gameplaySoundPlayer;

		private IslandFinishParticlePlayer _islandFinishParticlePlayer;
		private LevelEndSoundPlayer _levelEndSoundPlayer;

		public void Initialize(IUnitMovedEvent unitMover)
	    {
			_islandFinishParticlePlayer = new IslandFinishParticlePlayer(_levelProgressTracker, _islandFinishParticlePool);
	        _backgroundMusicChanger.Initialize(_levelChangeEventTracker);
	        _unitsMoveSoundPlayer.Initialize(unitMover, _unitMoveSound);
			_levelEndSoundPlayer = new LevelEndSoundPlayer(_levelProgressTracker, _gameplaySoundPlayer);
	    }

		public void Dispose()
		{
			_islandFinishParticlePlayer.Dispose();
			_unitsMoveSoundPlayer.Dispose();
			_levelEndSoundPlayer.Dispose();
		}
	}
}
