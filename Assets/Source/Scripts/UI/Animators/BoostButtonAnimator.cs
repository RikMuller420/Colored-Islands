using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BoostButtonAnimator : MonoBehaviour
{
    [SerializeField] private Image _buttonBackground;

    private Color _originalColor = Color.white;
    private Color _blinkColor = new Color(0.6f, 0.6f, 0.6f);
    private float _blinkDuration = 1f;
    private Tween _blinkSequence;

    public void ShowFinishIslandAnimation()
    {
    
    }

    public void StopAnimation()
    {
    
    }

    public void StartBlinking()
    {
        _blinkSequence = DOTween.Sequence()
            .Append(_buttonBackground.DOColor(_blinkColor, _blinkDuration))
            .Append(_buttonBackground.DOColor(_originalColor, _blinkDuration))
            .SetLoops(-1)
            .SetEase(Ease.Linear);
    }

    public void StopBlinking()
    {
        if (_blinkSequence != null)
        {
            _blinkSequence.Kill();
            _blinkSequence = null;
        }

        _buttonBackground.color = _originalColor;
    }
}
