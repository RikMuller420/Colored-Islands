using DG.Tweening;
using UnityEngine;

namespace SlimeGround.Menu.Extensions.Windows
{
	public class ZoneUi : MonoBehaviour
	{
		private const float MinAlpha = 0f;
		private const float MaxAlpha = 1f;

		[SerializeField] private bool _isOpened = false;
	    [SerializeField] private CanvasGroup _canvasGroup;
	    [SerializeField] private float _openFadeDuration = 0.2f;
	    [SerializeField] private float _closeFadeDuration = 0.2f;

	    public bool IsOpened => _isOpened;

	    private void OnValidate()
	    {
	        if (_canvasGroup == null)
	        {
	            return;
	        }

	        if (_isOpened)
	        {
	            _canvasGroup.alpha = MaxAlpha;
	            _canvasGroup.blocksRaycasts = true;
	            ActivateInteractivity();
	        }
	        else
	        {
	            _canvasGroup.alpha = MinAlpha;
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
	            .DOFade(MaxAlpha, _openFadeDuration)
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
	            .DOFade(MaxAlpha, _openFadeDuration)
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
	        DeactivateInteractivity();
	        _canvasGroup.DOKill();
	        _canvasGroup
	            .DOFade(MinAlpha, _closeFadeDuration)
	            .SetEase(Ease.InQuad)
	            .SetUpdate(true);
	    }

	    public void CloseImmediate()
	    {
	        if (IsOpened == false)
	        {
	            return;
	        }

	        _isOpened = false;
	        _canvasGroup.DOKill();
	        _canvasGroup.alpha = MinAlpha;
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
}
