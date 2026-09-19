using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Boosts
{
	public class ButtonAnimator
	{
	    private const float BlinkDuration = 1f;
		private readonly Color _originalColor = Color.white;
	    private readonly Color _blinkColor = new Color(0.6f, 0.6f, 0.6f);

		private readonly Image _buttonBackground;
		private readonly GameObject _glow;

		private Tween _blinkSequence;

	    public ButtonAnimator(Image buttonBackground, GameObject glow)
	    {
	        _buttonBackground = buttonBackground;
	        _glow = glow;
	    }

		public void StartBlinking()
	    {
	        _glow.SetActive(true);
	        _blinkSequence = DOTween.Sequence()
	            .Append(_buttonBackground.DOColor(_blinkColor, BlinkDuration))
	            .Append(_buttonBackground.DOColor(_originalColor, BlinkDuration))
	            .SetLoops(-1)
	            .SetEase(Ease.InOutQuad);
	    }

	    public void StopBlinking()
	    {
	        if (_blinkSequence != null)
	        {
	            _blinkSequence.Kill();
	            _blinkSequence = null;
	        }

	        _buttonBackground.color = _originalColor;
	        _glow.SetActive(false);
	    }
	}
}
