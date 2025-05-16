using UnityEngine;

public class GameClickHandler
{
    private Camera _camera;

    private ClickHandlerData _currentClickHandler;
    private ClickHandlerData _defaultClickHandler;

    public GameClickHandler(InputHandler inputHandler, Camera camera,
                            LayerMask allIslandsAndUnitsLayer,
                            SelectHandler selectHandler)
    {
        _camera = camera;
        var defaultClickHandler = new DefaultClickHandlier(selectHandler);

        _defaultClickHandler = new ClickHandlerData(defaultClickHandler, allIslandsAndUnitsLayer);

        ResetClickHandler();
        inputHandler.Clicked += OnClick;
    }

    public void ResetClickHandler()
    {
        _currentClickHandler = _defaultClickHandler;
    }

    public void SetClickHandler(ClickHandlerData clickHandler)
    {
        _currentClickHandler = clickHandler;
    }

    private void OnClick(Vector2 clickPosition)
    {
        bool isPaused = Time.timeScale == 0;

        if (isPaused)
        {
            return;
        }

        Ray ray = _camera.ScreenPointToRay(clickPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _currentClickHandler.MaxClickDistance, _currentClickHandler.LayerMask))
        {
            _currentClickHandler.ClickHandler.HandleClick(hit);
        }
    }
}
