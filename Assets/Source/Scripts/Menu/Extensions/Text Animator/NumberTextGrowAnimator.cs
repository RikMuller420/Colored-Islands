using DG.Tweening;
using TMPro;
using UnityEngine;

namespace SlimeGround.Menu.Extensions.TextAnimator
{
	public class NumberTextGrowAnimator : MonoBehaviour
	{
	    [SerializeField] private TextMeshProUGUI _text;

	    private TextGrowAnimatorSettings _settings;

	    public void ResetAnimation()
	    {
	        _text.transform.localScale = Vector3.one;
	        _text.text = string.Empty;
	    }

	    public void ShowGrowAnimation(TextGrowAnimatorSettings settings, int resultValue, int startValue = 0)
	    {
	        _settings = settings;
	        ResetAnimation();
	        ShowTextValueGrowAnimation(resultValue, startValue);
	        ShowPulseAnimation();
	    }

	    public void SetValueImediatly(int value)
	    {
	        _text.text = value.ToString();
	    }

	    private void ShowTextValueGrowAnimation(int resultValue, int startValue)
	    {
	        int value = startValue;

	        DOTween.To(
				() =>
					value,
					newValue =>
					{
						value = newValue;
						_text.text = value.ToString();
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
}
