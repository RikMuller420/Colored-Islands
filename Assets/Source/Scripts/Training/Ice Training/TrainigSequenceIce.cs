using DG.Tweening;
using UnityEngine;

public class TrainigSequenceIce : TrainigSequence
{
    [SerializeField] private CanvasGroup _iceHint;

    private float _appearTime = 0.7f;
    private float _disappearTime = 1.5f;


    public override void StartTraining()
    {
        _iceHint.DOFade(1f, _appearTime);
        _iceHint.transform.LookAt(MainCamera.transform);
        UnitMover.UnitsMoved += OnUnitsMoved;
    }

    private void OnUnitsMoved(UnitsMoveInfo _)
    {
        _iceHint.DOFade(0f, _disappearTime);
    }
}
