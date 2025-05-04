using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _closeButton;
    [SerializeField] private MenuDimmer _menuDimmer;

    private bool _isOpen = false;
    private float _fadeDuration = 0.2f;
    private float _maxAlpha = 1f;
    private float _minAlpha = 0f;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Open()
    {
        if (_isOpen)
        {
            return;
        }

        _isOpen = true;
        _menuDimmer.Activate();
        _canvasGroup.DOKill();

        _canvasGroup
            .DOFade(_maxAlpha, _fadeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(ActivateInteractivity);
    }

    public void Close()
    {
        if (_isOpen == false)
        {
            return;
        } 

        _isOpen = false;
        _menuDimmer.Deactivate();
        _canvasGroup.DOKill();

        _canvasGroup
            .DOFade(_minAlpha, _fadeDuration)
            .SetEase(Ease.InQuad)
            .OnComplete(DeactivateInteractivity);
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
