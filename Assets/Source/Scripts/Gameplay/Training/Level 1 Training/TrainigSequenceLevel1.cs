using System.Collections.Generic;
using DG.Tweening;
using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceLevel1 : TrainigSequence
	{
	    [SerializeField] private List<Level1TrainingMove> _trainingMoves = new();

		private bool _isEventsSubscribed = false;
		private int _currentMoveIndex;
	    private Level1TrainingMove _currentTrainingMove;

	    private void OnDestroy()
	    {
			if (_isEventsSubscribed)
			{
				UnitsSelectedEvent.UnitsSelected -= OnUnitsSelected;
				UnitMovedEvent.UnitsMoved -= OnUnitsMoved;
				ScreenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged; 
				
				_isEventsSubscribed = false;
			}

	        ResetLevelState();
	    }

	    public override void StartTraining()
	    {
			BoostButtonActivator.DeactivateAllButtons();

			if (_isEventsSubscribed == false)
			{
				UnitsSelectedEvent.UnitsSelected += OnUnitsSelected;
				UnitMovedEvent.UnitsMoved += OnUnitsMoved;
				ScreenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;

				_isEventsSubscribed = true;
			}

			_currentMoveIndex = 0;
			_currentTrainingMove = _trainingMoves[_currentMoveIndex];
	        ActivateTrainingMove();
	    }

	    private void ActivateTrainingMove()
	    {
	        DeactivateColliders();

	        DOTween.Sequence().Append(PointerImage.DOFade(0f, FadeDuration)
	                  .SetEase(Ease.InOutQuad))
	                  .AppendCallback(UpdatePointerPosition)
	                  .Append(PointerImage.DOFade(1f, FadeDuration)
	                  .SetEase(Ease.InOutQuad))
	                  .OnComplete(ActivateTrainingMoveColliders);
	    }

	    private void OnScreenSizeChanged(Vector2 _) => UpdatePointerPosition();

	    private void UpdatePointerPosition()
	    {
	        Pointer.position = _currentTrainingMove.PointerPosition.position;
	        Pointer.LookAt(MainCamera.transform.position);
	    }

	    private void ActivateTrainingMoveColliders()
	    {
	        BaseIsland targetIsland = _currentTrainingMove.IsUseBufferIsland ? BuferIslandsHolder.CurrentIsland : _currentTrainingMove.Island;

	        switch (_currentTrainingMove.Type)
	        {
	            case Level1TrainingMoveType.SelectUnits:
	                ActivateUnitsColliders(targetIsland, _currentTrainingMove.UnitsSlot);
	                break;

	            case Level1TrainingMoveType.MoveUnits:
	                targetIsland.Collider.enabled = true;
	                break;

	        }
	    }

	    private void OnUnitsSelected()
	    {
	        if (_trainingMoves[_currentMoveIndex].Type == Level1TrainingMoveType.SelectUnits)
	        {
	            ActivateNextTrainingMove();
	        }
	    }

	    private void OnUnitsMoved(UnitsMoveInfo _)
	    {
	        if (_trainingMoves[_currentMoveIndex].Type == Level1TrainingMoveType.MoveUnits)
	        {
	            ActivateNextTrainingMove();
	        }
	    }

	    private void ActivateNextTrainingMove()
	    {
	        _currentMoveIndex++;

	        if (_currentMoveIndex < _trainingMoves.Count)
	        {
	            _currentTrainingMove = _trainingMoves[_currentMoveIndex];
	            ActivateTrainingMove();
	        }
	        else
	        {
	            DeactivatePointer();
	        }
	    }

	    private void ActivateUnitsColliders(BaseIsland island, UnitSlotType unitsSlot)
	    {
	        foreach (IslandPoint islandPoint in island.Points)
	        {
	            if (islandPoint.IsFree == false && islandPoint.OccupiedUnit.Slot == unitsSlot)
	            {
	                islandPoint.OccupiedUnit.Collider.enabled = true;
	            }
	        }
	    }
	}
}
