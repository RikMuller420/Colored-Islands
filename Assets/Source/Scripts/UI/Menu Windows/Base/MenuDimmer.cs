using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuDimmer : MonoBehaviour
{
    [SerializeField] private CanvasGroup _backgroundDim;
    [SerializeField] private Button _button;

    private MenuWindow _openedMenu;

    private float _dimFadeDuration = 0.3f;
    private float _maxDimAlpha = 1f;
    private float _minDimAlpha = 0f;

    private void OnEnable()
    {
        _button.onClick.AddListener(CloseOpenedWindow);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(CloseOpenedWindow);
    }

    public void Activate(MenuWindow menu)
    {
        _openedMenu = menu;
        _backgroundDim.DOKill();
        _backgroundDim.blocksRaycasts = true;

        _backgroundDim
            .DOFade(_maxDimAlpha, _dimFadeDuration)
            .SetEase(Ease.OutQuad)
            .SetUpdate(true);
    }

    public void Deactivate()
    {
        _backgroundDim.DOKill();
        _backgroundDim.blocksRaycasts = false;
        _backgroundDim
            .DOFade(_minDimAlpha, _dimFadeDuration)
            .SetEase(Ease.InQuad)
            .SetUpdate(true)
            .OnComplete(StopBlockDimRaycasts);
    }

    private void StopBlockDimRaycasts()
    {
        _backgroundDim.blocksRaycasts = false;
    }

    private void CloseOpenedWindow()
    {
        _openedMenu.Close();
    }
}
