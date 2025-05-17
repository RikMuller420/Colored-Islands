using UnityEngine;
using YG;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private TestUI _testUI;

    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private LayerMask _allIslandsAndUnitsLayer;
    [SerializeField] private UnitsPool _unitsPool;
    [SerializeField] private BuferIslandsHolder _buferIslands;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private UIZoneSwitcher _uiZoneActivator;
    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private Camera _camera;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private LastLevelTextUpdater _lastLevelTextUpdater;
    [SerializeField] private FinalScoreWindow _finalScoreWindow;
    [SerializeField] private YandexGame _yandexGame;

    [SerializeField] private FirstUnfinishedLevelButton _firstUnfinishedLevelButton;
    [SerializeField] private NextLevelButton _nextLevelButton;
    [SerializeField] private BoostInitializer _boostButtonInitializer;
    [SerializeField] private InGameShopInitializer _inGameShopInitializer;

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        var levelDataHolder = new LevelObjectsHolder();
        var unitMover = new UnitMover();
        var selectHandler = new SelectHandler(unitMover, _buferIslands, levelDataHolder);

        var defaultClickHandler = new DefaultClickHandler(selectHandler, _allIslandsAndUnitsLayer);
        var gameClickHandler = new ClickHandler(_inputHandler, _camera, defaultClickHandler);
        var gameProgressStorage = new GameProgressStorage(_levelSettings);
        var levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, gameProgressStorage);
        var walletProvider = new WalletProvider(gameProgressStorage);
        var fullscreenAdOpener = new FullscreenAdOpener(_levelLoader, _yandexGame);


        _nextLevelButton.Initialize(_levelLoader);
        _firstUnfinishedLevelButton.Initialize(gameProgressStorage, _levelLoader);

        var boostAmountProvider = new BoostAmountProvider(gameProgressStorage);

        _boostButtonInitializer.Initialize(unitMover, gameClickHandler, selectHandler, levelDataHolder,
                                           boostAmountProvider);
        _inGameShopInitializer.Initialize(boostAmountProvider, walletProvider);

        _walletView.Initialize(walletProvider);
        _lastLevelTextUpdater.Initialize(gameProgressStorage);

        _levelProgressTracker.Initialize(gameProgressStorage, levelDataHolder, unitMover);
        _finalScoreWindow.Initialize(gameProgressStorage, _levelProgressTracker);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _levelProgressTracker,
                                _uiZoneActivator, levelDataHolder, _buferIslands);

        _testUI.Initialize(gameProgressStorage, walletProvider);
    }
}
