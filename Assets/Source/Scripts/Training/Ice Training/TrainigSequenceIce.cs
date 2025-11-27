using DG.Tweening;
using UnityEngine;

public class TrainigSequenceIce : TrainigSequence
{
    [SerializeField] private CanvasGroup _iceHint;

    private float _appearTime = 0.7f;
    private float _showDuration = 6f;
    private float _disappearTime = 1.5f;


    public override void StartTraining()
    {
        _iceHint.DOFade(1f, _appearTime)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(_showDuration, () =>
                    {
                        _iceHint.DOFade(0f, _disappearTime);
                    });
                });

        _iceHint.transform.LookAt(MainCamera.transform);
    }
}
