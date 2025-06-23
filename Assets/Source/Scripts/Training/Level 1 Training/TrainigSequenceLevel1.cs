using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TrainigSequenceLevel1 : TrainigSequence
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private Image _pointerImage;
    [SerializeField] private List<Level1TrainingMove> _trainingMoves = new();

    private int _currentMoveIndex;
    private Level1TrainingMove _currentTrainingMove;

    public override void StartTraining()
    {
        _currentMoveIndex = 0;
        BoostButtonActivator.DeactivateAllButtons();
        SelectHandler.UnitsSelected += OnUnitsSelected;
        UnitMover.UnitsMoved += OnUnitsMoved;
        LevelProgressTracker.LevelFinished += OnLevelFinished;

        _currentTrainingMove = _trainingMoves[_currentMoveIndex];
        ActivateTrainingMove();
    }

    private void ActivateTrainingMove()
    {
        DeactivateAllColliders();

        DOTween.Sequence().Append(_pointerImage.DOFade(0f, FadeDuration)
                          .SetEase(Ease.InOutQuad))
                          .AppendCallback(UpdatePointerPosition)
                          .Append(_pointerImage.DOFade(1f, FadeDuration)
                          .SetEase(Ease.InOutQuad))
                          .OnComplete(ActivateTrainingMoveColliders);
    }

    private void UpdatePointerPosition()
    {
        _pointer.position = _currentTrainingMove.PointerPosition.position;
        _pointer.LookAt(MainCamera.transform.position);
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

    private void OnLevelFinished()
    {
        BoostButtonActivator.ActivateAllButtons();
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
            DOTween.Sequence().Append(_pointerImage.DOFade(0f, FadeDuration)
                              .SetEase(Ease.InOutQuad));
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

    private void DeactivateAllColliders()
    {
        foreach (Island island in LevelObjectsHolder.Islands)
        {
            DeactivateColliders(island);
        }

        DeactivateColliders(BuferIslandsHolder.CurrentIsland);
    }

    private void DeactivateColliders(BaseIsland island)
    {
        island.Collider.enabled = false;

        foreach (IslandPoint islandPoint in island.Points)
        {
            if (islandPoint.IsFree)
            {
                continue;
            }

            islandPoint.OccupiedUnit.Collider.enabled = false;
        }
    }
}
