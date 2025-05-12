using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private TestUI _testUI;

    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private UnitsPool _unitsPool;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private BuferIslandsHolder _buferIslands;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private LayerMask _clickLayer;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private UIZoneSwitcher _uiZoneActivator;
    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private UnitMover _unitMover;
    [SerializeField] private Camera _camera;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private LastLevelTextUpdater _lastLevelTextUpdater;
    [SerializeField] private FinalScoreWindow _finalScoreWindow;

    [SerializeField] private FirstUnfinishedLevelButton _firstUnfinishedLevelButton;
    [SerializeField] private NextLevelButton _nextLevelButton;

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        UnitHighlighter unitHighlighter = new UnitHighlighter();
        SelectHandler selectHandler = new SelectHandler(unitHighlighter, _unitMover);
        GameClickHandler gameClickHandler = new GameClickHandler(_inputHandler, _camera, _clickLayer, selectHandler);
        GameProgressStorage gameProgressStorage = new GameProgressStorage(_levelSettings);
        GameProgressUpdater levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, gameProgressStorage);
        WalletProvider walletProvider = new WalletProvider(gameProgressStorage);

        _nextLevelButton.Initialize(_levelLoader);
        _firstUnfinishedLevelButton.Initialize(gameProgressStorage, _levelLoader);
        _walletView.Initialize(walletProvider);
        _lastLevelTextUpdater.Initialize(gameProgressStorage);

        _levelProgressTracker.Initialize(gameProgressStorage);
        _finalScoreWindow.Initialize(gameProgressStorage, _levelProgressTracker);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _buferIslands,
                                _levelProgressTracker, _uiZoneActivator);


        _testUI.Initialize(gameProgressStorage, walletProvider);
    }
}
