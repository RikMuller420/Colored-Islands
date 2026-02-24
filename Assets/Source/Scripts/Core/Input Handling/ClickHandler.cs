using System.Collections.Generic;
using SlimeGround.Gameplay.Levels;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SlimeGround.Core.InputHandling
{
	public class ClickHandler
	{
	    private Camera _camera;
	    private GameplayClickHandler _gameplayClickBehaviour;
		private MenuClickBehaviour _menuClickBehaviour;
		private InputHandler _inputHandler;
		private LevelLoader _levelLoader;

		private ClickBehaviour _currentClickBehaviour;

		public ClickHandler(LevelLoader levelLoader, GameplayClickHandler gameplayClickBehaviour,
							MenuClickBehaviour menuClickBehaviour, InputHandler inputHandler,
	                        Camera camera)
	    {
	        _camera = camera;
			_inputHandler = inputHandler;
			_levelLoader = levelLoader;
			_menuClickBehaviour = menuClickBehaviour;
			_gameplayClickBehaviour = gameplayClickBehaviour;

			SetClickBehaviour(_menuClickBehaviour);
			_levelLoader.LevelChanged += OnLevelChanged;
			_inputHandler.Clicked += OnClick;
	    }

		public void Dispose()
		{
			_levelLoader.LevelChanged -= OnLevelChanged;
			_inputHandler.Clicked -= OnClick;
		}

		public void ActivateGameplayClickHandler()
	    {
	        SetClickBehaviour(_gameplayClickBehaviour);
	    }

	    public void SetClickBehaviour(ClickBehaviour clickBehaviour)
		{
			if (_currentClickBehaviour != null)
	        {
	            _currentClickBehaviour.ResetBehaviour();
	        }

	        _currentClickBehaviour = clickBehaviour;
	    }

		private void OnLevelChanged(ILevelData levelData)
		{
			if (levelData.IsMenuLevel)
			{
				SetClickBehaviour(_menuClickBehaviour);
			}
			else
			{
				SetClickBehaviour(_gameplayClickBehaviour);
			}
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

			if (Physics.Raycast(ray, out RaycastHit hit,
								_currentClickBehaviour.MaxClickDistance,
								_currentClickBehaviour.LayerMask))
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
