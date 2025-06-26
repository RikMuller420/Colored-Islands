using DG.Tweening;
using UnityEngine;

public class ZoneUi : MonoBehaviour
{
    [SerializeField] private bool _isOpened = false;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _openFadeDuration = 0.2f;
    [SerializeField] private float _closeFadeDuration = 0.2f;

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

    public void OpenWithDelay(float delay)
    {
        if (IsOpened)
        {
            return;
        }

        _isOpened = true;
        _canvasGroup.DOKill();
        _canvasGroup
            .DOFade(_maxAlpha, _openFadeDuration)
            .SetDelay(delay)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true)
            .OnComplete(ActivateInteractivity);
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
            .DOFade(_maxAlpha, _openFadeDuration)
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
            .DOFade(_minAlpha, _closeFadeDuration)
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
