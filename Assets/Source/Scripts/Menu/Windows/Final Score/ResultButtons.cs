using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.FinalScore
{
	public class ResultButtons : MonoBehaviour
	{
		private const float MinScale = 1f;
		private const float MaxScale = 1.15f;
		private const float AnimationDuration = 0.25f;

		[SerializeField] private List<Button> _buttons;

	    public void ResetButtons()
	    {
	        foreach (Button button in _buttons)
	        {
	            button.enabled = false;
	            button.transform.localScale = Vector3.one * MinScale;
	        }
	    }

	    public void Activate()
	    {
	        foreach (Button button in _buttons)
	        {
	            button.enabled = true;
	            ShowActivateAnimation(button.transform);
	        }
	    }

	    private void ShowActivateAnimation(Transform button)
	    {
	        int loops = 2;

	        button
	            .DOScale(MaxScale, AnimationDuration)
	            .SetEase(Ease.OutQuad)
	            .SetLoops(loops, LoopType.Yoyo);
	    }
	}
}
