using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.FinalScore
{
	public class ResultButtons : MonoBehaviour
	{
	    [SerializeField] private List<Button> _buttons;

	    private float _minScale = 1f;
	    private float _maxScale = 1.15f;
	    private float _animationDuration = 0.25f;

	    public void ResetButtons()
	    {
	        foreach (Button button in _buttons)
	        {
	            button.enabled = false;
	            button.transform.localScale = Vector3.one * _minScale;
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
	            .DOScale(_maxScale, _animationDuration)
	            .SetEase(Ease.OutQuad)
	            .SetLoops(loops, LoopType.Yoyo);
	    }
	}
}
