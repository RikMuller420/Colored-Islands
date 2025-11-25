using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        if (IsPointerOverUI(clickPosition))
        {
            return;
        }

        if (Physics.Raycast(ray, out RaycastHit hit, _currentClickBehaviour.MaxClickDistance, _currentClickBehaviour.LayerMask))
        {
            _currentClickBehaviour.HandleClick(hit);
        }
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }
}
