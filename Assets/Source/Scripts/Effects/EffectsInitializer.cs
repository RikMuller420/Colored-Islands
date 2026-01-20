using UnityEngine;

public class EffectsInitializer : MonoBehaviour
{
    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private IslandFinishParticlePool _islandFinishParticlePool;

    [SerializeField] private BackgroundMusicChanger _backgroundMusicChanger;
    [SerializeField] private UnitsMoveSoundPlayer _unitsMoveSoundPlayer;
    [SerializeField] private AudioSource _unitMoveSound;
    [SerializeField] private GameplaySoundPlayer _gameplaySoundPlayer;

    public void Initialize(IUnitMovedEvent unitMover)
    {
        var islandFinishParticlePlayer = new IslandFinishParticlePlayer(_levelProgressTracker, _islandFinishParticlePool);
        _backgroundMusicChanger.Initialize(_levelChangeEventTracker);
        _unitsMoveSoundPlayer.Initialize(unitMover, _unitMoveSound);
        var levelEndSoundPlayer = new LevelEndSoundPlayer(_levelProgressTracker, _gameplaySoundPlayer);
    }
}
