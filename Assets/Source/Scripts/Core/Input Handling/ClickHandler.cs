using System.Collections.Generic;
using SlimeGround.Gameplay.Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SlimeGround.Core.InputHandling
{
	public class ClickHandler
	{
	    private Camera _camera;
	    private DefaultClickHandler _defaultClickBehaviour;

	    private ClickBehaviour _currentClickBehaviour;

	    public ClickHandler(UnitMover unitMover, InputHandler inputHandler,
	                        Camera camera, LayerMask allIslandsAndUnitsLayer,
	                        out IUnitsSelectedEvent unitsSelectedEvent)
	    {
	        _camera = camera;

	        _defaultClickBehaviour = new DefaultClickHandler(unitMover, allIslandsAndUnitsLayer);

	        SetDeafultClickHandler();
	        inputHandler.Clicked += OnClick;

	        unitsSelectedEvent = _defaultClickBehaviour;
	    }

	    public void SetDeafultClickHandler()
	    {
	        SetClickBehaviour(_defaultClickBehaviour);
	    }

	    public void SetClickBehaviour(ClickBehaviour clickBehaviour)
	    {
	        if (_currentClickBehaviour != null)
	        {
	            _currentClickBehaviour.ResetBehaviour();
	        }

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

}
