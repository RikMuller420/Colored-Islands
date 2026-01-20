using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private UpgradeSettings _upgradeSettings;
    [SerializeField] private LayerMask _allIslandsAndUnitsLayer;

    [Header("Component Initializers")]
    [SerializeField] private MetricInitializer _metricInitializer;
    [SerializeField] private BoostInitializer _boostInitializer;
    [SerializeField] private LeaderboardInitializer _leaderboardInitializer;
    [SerializeField] private MenuInitializer _menuInitializer;
    [SerializeField] private EffectsInitializer _effectsInitializer;
    [SerializeField] private GameplayInitializer _gameplayInitializer;
    [SerializeField] private DeviceStyleChangeInitializer _deviceStyleChangeInitializer;

    [Header("Gameplay")]
    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _unitsLookAtPoint;
    [SerializeField] private CameraPositionChanger _cameraPositionChanger;
    [SerializeField] private TrainigSequenceLoader _trainigLoader;

    private void Start()
    {
        InitializeGame();

        _levelLoader.LoadMainMenu();

        var inAppConsumer = new InAppPurchaseConsumeProvider();
        inAppConsumer.ConsumePurchase();

        _trainigLoader.TryLoadTrainingLevel();
    }

    private void InitializeGame()
    {
        _playerData.Initialize();

        var upgradesProvider = new UpgradesProvider(_playerData, _upgradeSettings);
        var boostAmountProvider = new BoostAmountProvider(_playerData);
        var walletProvider = new WalletProvider(_playerData);
        var rewardedAdProvider = new RewardedAdProvider();
        var authorizationProvider = new AuthorizationProvider();
        var leaderboardProvider = new LeaderboardProvider();

        var levelDataHolder = new LevelDataHolder(_levelSettings.MainMenuSettings);

        var unitMover = new UnitMover(_unitsLookAtPoint);
        var clickHandler = new ClickHandler(unitMover, _inputHandler, _camera,
                                            _allIslandsAndUnitsLayer,
                                            out IUnitsSelectedEvent unitsSelectedEvent);

        _gameplayInitializer.Initialize(upgradesProvider, leaderboardProvider,
                                        levelDataHolder, unitMover);
        
        _levelLoader.Initialize(upgradesProvider, unitMover, levelDataHolder);

        _boostInitializer.Initialize(unitMover, clickHandler, levelDataHolder,
                                     boostAmountProvider, walletProvider, rewardedAdProvider,
                                     out IBoostStopApplyedEvent freezeBoostApplyedEvent);

        _menuInitializer.Initialize(upgradesProvider, authorizationProvider, rewardedAdProvider,
                                    boostAmountProvider, walletProvider, freezeBoostApplyedEvent);

        _trainigLoader.Initilize(unitsSelectedEvent, unitMover);
        _deviceStyleChangeInitializer.Initialize();
        _metricInitializer.Initilize(levelDataHolder);
        _effectsInitializer.Initialize(unitMover);
        _cameraPositionChanger.Initialize(levelDataHolder);
        _leaderboardInitializer.Initialize(leaderboardProvider, authorizationProvider);
    }
}
