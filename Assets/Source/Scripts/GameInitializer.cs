using UnityEngine;
using UnityEngine.EventSystems;

public class GameInitializer : MonoBehaviour
{
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

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        Camera camera = Camera.main;
        UnitHighlighter unitHighlighter = new UnitHighlighter();
        SelectHandler selectHandler = new SelectHandler(unitHighlighter, _unitMover);
        GameClickHandler gameClickHandler = new GameClickHandler(_inputHandler, camera, _clickLayer, selectHandler);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _buferIslands,
                                _levelProgressTracker, _uiZoneActivator);
    }
}
