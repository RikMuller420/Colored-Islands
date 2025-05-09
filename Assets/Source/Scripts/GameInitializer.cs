using System.Collections.Generic;
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

    [SerializeField] private List<NextLevelButton> _nextLevelButtons = new List<NextLevelButton>();

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
        LevelProgressUpdater levelProgressUpdater = new LevelProgressUpdater(_levelProgressTracker, gameProgressStorage);
        WalletProvider walletProvider = new WalletProvider(gameProgressStorage);

        foreach (NextLevelButton button in _nextLevelButtons)
        {
            button.Initialize(gameProgressStorage);
        }

        _walletView.Initialize(walletProvider);
        _levelProgressTracker.Initialize(gameProgressStorage);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _buferIslands,
                                _levelProgressTracker, _uiZoneActivator);


        _testUI.Initialize(gameProgressStorage, walletProvider);
    }
}
