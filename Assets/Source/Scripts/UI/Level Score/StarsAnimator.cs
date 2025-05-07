using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StarsAnimator : MonoBehaviour
{
    [SerializeField] private List<Image> _stars;

    private float _scaleDurationGrow = 0.7f;
    private float _scaleDurationDecrease = 0.25f;
    private float _fadeDuration = 0.5f;
    private float _scaleInGrow = 1.2f;
    private float _finalScale = 1f;

    private int _curentStar = 0;

    public float AnmationDuration { get => _scaleDurationGrow + _scaleDurationDecrease; }

    public void ResetStars()
    {
        foreach (Image star in _stars)
        {
            ResetStar(star);
        }

        _curentStar = 0;
    }

    public void PlayNextStarAnimation()
    {
        if (_curentStar == _stars.Count)
        {
            throw new InvalidOperationException("No next star");
        }

        PlayAnimation(_stars[_curentStar]);
        _curentStar++;
    }

    private void ResetStar(Image star)
    {
        star.transform.localScale = Vector3.zero;
        Color imageColor = star.color;
        imageColor.a = 0f;
        star.color = imageColor;
    }

    private void PlayAnimation(Image star)
    {
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(star.transform.DOScale(_scaleInGrow, _scaleDurationGrow))
                     .Append(star.transform.DOScale(_finalScale, _scaleDurationDecrease));

        star.DOFade(1f, _fadeDuration);
    }
}
