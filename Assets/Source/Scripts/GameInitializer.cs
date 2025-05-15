using UnityEngine;
using YG;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private TestUI _testUI;

    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private LayerMask _clickLayer;
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
    [SerializeField] private BoostButtonInitializer _boostButtonInitializer;

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        LevelObjectsHolder levelDataHolder = new LevelObjectsHolder();
        UnitMover unitMover = new UnitMover();
        SelectHandler selectHandler = new SelectHandler(unitMover, _buferIslands, levelDataHolder);
        GameClickHandler gameClickHandler = new GameClickHandler(_inputHandler, _camera, _clickLayer, selectHandler);
        GameProgressStorage gameProgressStorage = new GameProgressStorage(_levelSettings);
        GameProgressUpdater levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, gameProgressStorage);
        WalletProvider walletProvider = new WalletProvider(gameProgressStorage);
        FullscreenAdOpener fullscreenAdOpener = new FullscreenAdOpener(_levelLoader, _yandexGame);
        BufferIslandBoost bufferIslandBoost = new BufferIslandBoost(_buferIslands, unitMover);
        ObjectivesFreezeBoost objectivesFreezeBoost = new ObjectivesFreezeBoost(_levelProgressTracker, unitMover);
        PaintAmountReduceBoost paintAmountReduceBoost = new PaintAmountReduceBoost(levelDataHolder, _buferIslands, _materials);

        _nextLevelButton.Initialize(_levelLoader);
        _firstUnfinishedLevelButton.Initialize(gameProgressStorage, _levelLoader);
        _boostButtonInitializer.InitializeButtons(bufferIslandBoost, objectivesFreezeBoost, paintAmountReduceBoost);
        _walletView.Initialize(walletProvider);
        _lastLevelTextUpdater.Initialize(gameProgressStorage);

        _levelProgressTracker.Initialize(gameProgressStorage, levelDataHolder, unitMover);
        _finalScoreWindow.Initialize(gameProgressStorage, _levelProgressTracker);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _levelProgressTracker,
                                _uiZoneActivator, levelDataHolder, _buferIslands);

        _testUI.Initialize(gameProgressStorage, walletProvider);
    }
}
