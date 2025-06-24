using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrainigSequenceLevel1 : TrainigSequence
{
    [SerializeField] private List<Level1TrainingMove> _trainingMoves = new();

    private int _currentMoveIndex;
    private Level1TrainingMove _currentTrainingMove;

    public override void StartTraining()
    {
        _currentMoveIndex = 0;
        BoostButtonActivator.DeactivateAllButtons();
        SelectHandler.UnitsSelected += OnUnitsSelected;
        UnitMover.UnitsMoved += OnUnitsMoved;

        _currentTrainingMove = _trainingMoves[_currentMoveIndex];
        ActivateTrainingMove();
    }

    private void ActivateTrainingMove()
    {
        DeactivateAllColliders();

        DOTween.Sequence().Append(PointerImage.DOFade(0f, FadeDuration)
                  .SetEase(Ease.InOutQuad))
                  .AppendCallback(UpdatePointerPosition)
                  .Append(PointerImage.DOFade(1f, FadeDuration)
                  .SetEase(Ease.InOutQuad))
                  .OnComplete(ActivateTrainingMoveColliders);
    }

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
                ActivateUnitsColliders(targetIsland, _currentTrainingMove.UnitsPaint);
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

    private void OnUnitsMoved(UnitsMoveInfo unitsMoveInfo)
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

    private void ActivateUnitsColliders(BaseIsland island, Paint unitsPaint)
    {
        foreach (IslandPoint islandPoint in island.Points)
        {
            if (islandPoint.IsFree == false && islandPoint.OccupiedUnit.Paint == unitsPaint)
            {
                islandPoint.OccupiedUnit.Collider.enabled = true;
            }
        }
    }
}
