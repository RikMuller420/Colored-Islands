using UnityEngine;

public class ClickHandler
{
    private Camera _camera;

    private ClickBehaviour _currentClickBehaviour;
    private ClickBehaviour _defaultClickBehaviour;

    public ClickHandler(InputHandler inputHandler, Camera camera, ClickBehaviour defaultClickHBehaviour)
    {
        _camera = camera;
        _defaultClickBehaviour = defaultClickHBehaviour;

        ResetClickHandler();
        inputHandler.Clicked += OnClick;
    }

    public void ResetClickHandler()
    {
        _currentClickBehaviour = _defaultClickBehaviour;
    }

    public void SetClickBehaviour(ClickBehaviour clickBehaviour)
    {
        _currentClickBehaviour = clickBehaviour;
    }

    private void OnClick(Vector2 clickPosition)
    {
        bool isPaused = Time.timeScale == 0;

        if (isPaused)
        {
            return;
        }

        Ray ray = _camera.ScreenPointToRay(clickPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _currentClickBehaviour.MaxClickDistance, _currentClickBehaviour.LayerMask))
        {
            _currentClickBehaviour.HandleClick(hit);
        }
    }
}
