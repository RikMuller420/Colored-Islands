using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private UnitsPool _unitsPool;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private BuferIslandsHolder _buferIslands;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private LayerMask _clickLayer;
    [SerializeField] private LevelLoader _levelLoader;

    private void Start()
    {
        InitializeGame();

        _levelLoader.LoadLevel(1);
    }

    public void InitializeGame()
    {
        Camera camera = Camera.main;
        UnitHighlighter unitHighlighter = new UnitHighlighter();
        UnitMover unitMover = new UnitMover();
        SelectHandler selectHandler = new SelectHandler(unitHighlighter, unitMover);
        GameClickHandler gameClickHandler = new GameClickHandler(_inputHandler, camera, _clickLayer, selectHandler);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _buferIslands);
    }
}
