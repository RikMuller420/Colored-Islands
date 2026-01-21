using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Boosts
{

	public class ButtonAnimator
	{
	    private Image _buttonBackground;
	    private GameObject _glow;

	    private Color _originalColor = Color.white;
	    private Color _blinkColor = new Color(0.6f, 0.6f, 0.6f);
	    private float _blinkDuration = 1f;
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
	            .Append(_buttonBackground.DOColor(_blinkColor, _blinkDuration))
	            .Append(_buttonBackground.DOColor(_originalColor, _blinkDuration))
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
