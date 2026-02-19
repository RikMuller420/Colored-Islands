using DG.Tweening;
using UnityEngine;

public class Dimmer : MonoBehaviour
{
	[SerializeField] private CanvasGroup _backgroundDim;

	private float _dimFadeDuration = 0.3f;
	private float _maxDimAlpha = 1f;
	private float _minDimAlpha = 0f;

	public void Activate()
	{
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
			.OnComplete(DeactivateRaycasts);
	}

	private void DeactivateRaycasts()
	{
		_backgroundDim.blocksRaycasts = false;
	}
}
