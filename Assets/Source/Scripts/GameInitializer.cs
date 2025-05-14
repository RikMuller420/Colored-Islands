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
    [SerializeField] private UnitMover _unitMover;
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
        LevelDataHolder levelDataHolder = new LevelDataHolder();
        SelectHandler selectHandler = new SelectHandler(_unitMover, _buferIslands, levelDataHolder);
        GameClickHandler gameClickHandler = new GameClickHandler(_inputHandler, _camera, _clickLayer, selectHandler);
        GameProgressStorage gameProgressStorage = new GameProgressStorage(_levelSettings);
        GameProgressUpdater levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, gameProgressStorage);
        WalletProvider walletProvider = new WalletProvider(gameProgressStorage);
        FullscreenAdOpener fullscreenAdOpener = new FullscreenAdOpener(_levelLoader, _yandexGame);
        BufferIslandBoost bufferIslandBoost = new BufferIslandBoost(_buferIslands, _unitMover);
        ObjectivesFreezeBoost objectivesFreezeBoost = new ObjectivesFreezeBoost(_levelProgressTracker, _unitMover);

        _nextLevelButton.Initialize(_levelLoader);
        _firstUnfinishedLevelButton.Initialize(gameProgressStorage, _levelLoader);
        _boostButtonInitializer.InitializeButtons(bufferIslandBoost, objectivesFreezeBoost);
        _walletView.Initialize(walletProvider);
        _lastLevelTextUpdater.Initialize(gameProgressStorage);

        _levelProgressTracker.Initialize(gameProgressStorage, levelDataHolder);
        _finalScoreWindow.Initialize(gameProgressStorage, _levelProgressTracker);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _buferIslands,
                                _levelProgressTracker, _uiZoneActivator, levelDataHolder);

        _testUI.Initialize(gameProgressStorage, walletProvider);
    }
}
