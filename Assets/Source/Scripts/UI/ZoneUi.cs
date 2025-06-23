using DG.Tweening;
using UnityEngine;

public class ZoneUi : MonoBehaviour
{
    [SerializeField] private bool _isOpened = false;
    [SerializeField] private CanvasGroup _canvasGroup;

    private float _fadeDuration = 0.2f;
    private float _maxAlpha = 1f;
    private float _minAlpha = 0f;

    public bool IsOpened => _isOpened;

    private void OnValidate()
    {
        if (_canvasGroup == null)
        {
            return;
        }

        if (_isOpened)
        {
            _canvasGroup.alpha = _maxAlpha;
            _canvasGroup.blocksRaycasts = true;
            ActivateInteractivity();
        }
        else
        {
            _canvasGroup.alpha = _minAlpha;
            DeactivateInteractivity();
        }
    }

    public virtual void Open()
    {
        if (IsOpened)
        {
            return;
        }

        _isOpened = true;
        _canvasGroup.DOKill();
        _canvasGroup
            .DOFade(_maxAlpha, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(ActivateInteractivity);
    }

    public virtual void Close()
    {
        if (IsOpened == false)
        {
            return;
        }

        _isOpened = false;
        _canvasGroup.DOKill();
        _canvasGroup
            .DOFade(_minAlpha, _fadeDuration)
            .SetEase(Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(DeactivateInteractivity);
    }

    public void CloseImmediate()
    {
        if (IsOpened == false)
        {
            return;
        }

        _isOpened = false;
        _canvasGroup.DOKill();
        _canvasGroup.alpha = _minAlpha;
        DeactivateInteractivity();
    }

    private void ActivateInteractivity()
    {
        _canvasGroup.blocksRaycasts = true;
    }

    private void DeactivateInteractivity()
    {
        _canvasGroup.blocksRaycasts = false;
    }
}
