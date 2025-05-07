using DG.Tweening;
using TMPro;
using UnityEngine;

public class NumberTextGrowAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _textPrefix = "+";

    private TextGrowAnimatorSettings _settings;


    public void ResetAnimation()
    {
        _text.transform.localScale = Vector3.one;
        _text.text = string.Empty;
    }

    public void ShowGrowAnimation(TextGrowAnimatorSettings settings, int resultValue)
    {
        _settings = settings;
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
            _settings.GrowAnimationDuration
            )
            .SetEase(Ease.OutQuad)
            .SetDelay(_settings.AnimationDelay);
    }

    private void ShowPulseAnimation()
    {
        _text.transform
            .DOScale(_settings.PulseMaxSize, _settings.PulseFrequency)
            .SetLoops(_settings.PulseCycles, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetDelay(_settings.AnimationDelay);
    }
}
