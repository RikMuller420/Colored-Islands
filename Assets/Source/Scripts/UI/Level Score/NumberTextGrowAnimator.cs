using DG.Tweening;
using TMPro;
using UnityEngine;

public class NumberTextGrowAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _textPrefix = "+";
    [SerializeField] private float _growAnimationDuration = 1f;
    [SerializeField] private int _pulseCycles = 8;
    [SerializeField] private float _pulseFrequency = 0.15f;
    [SerializeField] private float _pulseMinSize = 1f;
    [SerializeField] private float _pulseMaxSize = 1.15f;
    [SerializeField] private float _animationDelay = 0f;

    public void ResetAnimation()
    {
        _text.transform.localScale = Vector3.one * _pulseMinSize;
        _text.text = string.Empty;
    }

    public void ShowGrowAnimation(int resultValue)
    {
        ResetAnimation();
        ShowTextValueGrowAnimation(resultValue);
        ShowPulseAnimation();
    }

    private void ShowTextValueGrowAnimation(int resultValue)
    {
        int value = 0;

        DOTween.To(() =>
            value,
            newValue =>
            {
                value = newValue;
                _text.text = $"{_textPrefix}{value}";
            },
            resultValue,
            _growAnimationDuration
            )
            .SetEase(Ease.OutQuad)
            .SetDelay(_animationDelay);
    }

    private void ShowPulseAnimation()
    {
        _text.transform
            .DOScale(_pulseMaxSize, _pulseFrequency)
            .SetLoops(_pulseCycles, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(_animationDelay);
    }
}
