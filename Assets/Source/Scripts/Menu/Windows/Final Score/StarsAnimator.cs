using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.FinalScore
{
	public class StarsAnimator : MonoBehaviour
	{
		private const float ScaleDurationGrow = 0.7f;
		private const float ScaleDurationDecrease = 0.25f;
		private const float FadeDuration = 0.5f;
		private const float ScaleInGrow = 1.2f;
		private const float FinalScale = 1f;

		[SerializeField] private List<Image> _stars;

	    private int _curentStar = 0;

	    public float AnmationDuration { get => ScaleDurationGrow + ScaleDurationDecrease; }

	    public void ResetStars()
	    {
	        foreach (Image star in _stars)
	        {
	            ResetStar(star);
	        }

	        _curentStar = 0;
	    }

	    public void PlayNextStarAnimation()
	    {
	        if (_curentStar == _stars.Count)
	        {
	            throw new InvalidOperationException("No next star");
	        }

	        PlayAnimation(_stars[_curentStar]);
	        _curentStar++;
	    }

	    private void ResetStar(Image star)
	    {
	        star.transform.localScale = Vector3.zero;
	        Color imageColor = star.color;
	        imageColor.a = 0f;
	        star.color = imageColor;
	        star.gameObject.SetActive(false);
	    }

	    private void PlayAnimation(Image star)
	    {
	        star.gameObject.SetActive(true);
	        Sequence scaleSequence = DOTween.Sequence();
	        scaleSequence.Append(star.transform.DOScale(ScaleInGrow, ScaleDurationGrow))
	                     .Append(star.transform.DOScale(FinalScale, ScaleDurationDecrease));

	        star.DOFade(1f, FadeDuration);
	    }
	}
}
