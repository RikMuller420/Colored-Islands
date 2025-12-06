using System.Linq;
using UnityEngine;

public class Ice : MonoBehaviour
{
    [SerializeField] private Island _island;
    [SerializeField] private int _movesToDeactivate = 10;

    [Space]
    [SerializeField] private IceView _iceView;

    private int _movesCount = 0;
    private UnitMover _unitMover;

    private void OnDestroy()
    {
        _unitMover.UnitsMoved -= OnUnitsMoved;
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
        _unitMover.UnitsMoved += OnUnitsMoved;
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
