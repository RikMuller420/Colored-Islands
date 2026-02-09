using System.Linq;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Gameplay.Islands
{
	public class Ice : MonoBehaviour
	{
	    [SerializeField] private Island _island;
	    [SerializeField] private int _movesToDeactivate = 10;

	    [Space]
	    [SerializeField] private IceView _iceView;

	    private int _movesCount = 0;
	    private UnitMover _unitMover;
		private bool _isEventsSubscribed = false;

		private void OnDestroy()
	    {
			if (_isEventsSubscribed)
			{
				_unitMover.UnitsMoved -= OnUnitsMoved;
				_isEventsSubscribed = false;
			}
	    }

	    public void Initialize(UnitMover unitMover, Transform cameraTransform)
	    {
	        _unitMover = unitMover;

	        _island.Deactivate();

	        foreach (IslandPoint point in _island.Points)
	        {
	            if (point.IsFree == false)
	            {
	                point.OccupiedUnit.FreezeAnimation();
	            }
	        }

	        _iceView.SetMovesToDeactivateText(_movesToDeactivate);
	        _iceView.Activate(cameraTransform);

			if (_isEventsSubscribed == false)
			{
				_unitMover.UnitsMoved += OnUnitsMoved;
				_isEventsSubscribed = true;
			}
	    }

	    private void OnUnitsMoved(UnitsMoveInfo _)
	    {
	        _movesCount++;
	        int currentMovesToDeactivate = _movesToDeactivate - _movesCount;
	        _iceView.SetMovesToDeactivateText(currentMovesToDeactivate);

	        if (_movesCount == _movesToDeactivate)
	        {
	            _island.Activate();
	            _iceView.Deactivate();
	        }
	    }
	}
}
