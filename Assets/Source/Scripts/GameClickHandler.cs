using UnityEngine;

public class GameClickHandler
{
    private InputHandler _inputHandler;
    private Camera _camera;
    private LayerMask _clickLayer;
    private SelectHandler _selectHandler;

    private float _maxClickDistance = 1000f;

    public GameClickHandler(InputHandler inputHandler, Camera camera, LayerMask clickLayer, SelectHandler selectHandler)
    {
        _clickLayer = clickLayer;
        _camera = camera;
        _inputHandler = inputHandler;
        _inputHandler.Clicked += OnClick;
        _selectHandler = selectHandler;
    }

    private void OnClick(Vector2 clickPosition)
    {
        Ray ray = _camera.ScreenPointToRay(clickPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, _maxClickDistance, _clickLayer))
        {
            if (hit.collider.TryGetComponent(out ISelectable selectable))
            {
                _selectHandler.Select(selectable);

                return;
            }
        }
    }
}
