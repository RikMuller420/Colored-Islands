using DG.Tweening;
using UnityEngine;

public class Dimmer : MonoBehaviour
{
	private const float DimFadeDuration = 0.3f;
	private const float MinDimAlpha = 0f;
	private const float MaxDimAlpha = 1f;

	[SerializeField] private CanvasGroup _backgroundDim;

	public void Activate()
	{
		_backgroundDim.DOKill();
		_backgroundDim.blocksRaycasts = true;
		_backgroundDim
			.DOFade(MaxDimAlpha, DimFadeDuration)
			.SetEase(Ease.OutQuad)
			.SetUpdate(true);
	}

	public void Deactivate()
	{
		_backgroundDim.DOKill();
		_backgroundDim.blocksRaycasts = false;
		_backgroundDim
			.DOFade(MinDimAlpha, DimFadeDuration)
			.SetEase(Ease.InQuad)
			.SetUpdate(true)
			.OnComplete(DeactivateRaycasts);
	}

	private void DeactivateRaycasts()
	{
		_backgroundDim.blocksRaycasts = false;
	}
}
