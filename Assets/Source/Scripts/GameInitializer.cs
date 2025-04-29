using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private LayerMask _clickLayer;

    public void InitializeGame()
    {
        Camera camera = Camera.main;
        UnitHighlighter unitHighlighter = new UnitHighlighter();
        UnitMover unitMover = new UnitMover();
        SelectHandler selectHandler = new SelectHandler(unitHighlighter, unitMover);
        GameClickHandler gameClickHandler = new GameClickHandler(_inputHandler, camera, _clickLayer, selectHandler);
    }
}
