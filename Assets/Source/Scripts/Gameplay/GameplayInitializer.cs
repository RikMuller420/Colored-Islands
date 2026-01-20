using UnityEngine;

public class GameplayInitializer : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private UnitsFaceSettings _faceSettings;
    [SerializeField] private UnitsHatSettings _hatSettings;
    [SerializeField] private LeaderboardSettings _leaderboardSettings;

    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private BuferIslandsHolder _buferIslands;

    public void Initialize(IUpgradesData upgradesData, LeaderboardProvider leaderboardProvider,
                           ILevelData currentLevelData, UnitMover unitMover)
    {
        var levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, _playerData, leaderboardProvider,
                                            _leaderboardSettings);

        _levelProgressTracker.Initialize(currentLevelData, unitMover, upgradesData, _playerData);
        _buferIslands.Initialize(unitMover);
    }
}
