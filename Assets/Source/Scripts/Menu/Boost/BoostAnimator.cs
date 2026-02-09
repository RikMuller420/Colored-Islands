using System.Collections.Generic;
using SlimeGround.Core.InputHandling;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Menu.Boosts
{
	public class BoostAnimator
	{
	    private Dictionary<Boost, BoostButton> _boostsButtons;
	    private BoostButton _islandFinishBoostButton;
	    private IIslandFinishEvent _islandFinishEvent;

	    private GameObject _finishIslandEffect;
	    private GameObject _reduceColorEffect;
	    private Vector3 _effectOffset = new Vector3(0, 1f, 0f);

	    public BoostAnimator(Dictionary<Boost, BoostButton> boostsButtons, IIslandFinishEvent islandFinishEvent,
	                         GameObject finishIslandEffect, GameObject reduceColorEffect)
	    {
	        _boostsButtons = boostsButtons;
	        _islandFinishEvent = islandFinishEvent;
	        _finishIslandEffect = finishIslandEffect;
	        _reduceColorEffect = reduceColorEffect;

	        foreach (var boostButton in _boostsButtons)
	        {
	            if (boostButton.Key is IslandFinishBoost finishIslandBoost)
	            {
	                finishIslandBoost.BoostStartApplyed += StartBlinkFinishBoostButton;
	                finishIslandBoost.BoostStopApplyed += StopBlinkFinishBoostButton;

	                _islandFinishBoostButton = boostButton.Value;
	            }

	            if (boostButton.Key is PaintAmountReduceBoost paintReduceBoost)
	            {
	                paintReduceBoost.BoostApplyed += PlayReduceColorEffect;
	            }
	        }

	        _islandFinishEvent.IslandFinished += OnIslandAutoFinished;
	    }

		public void Dispose()
		{
			foreach (var boostButton in _boostsButtons)
			{
				if (boostButton.Key is IslandFinishBoost finishIslandBoost)
				{
					finishIslandBoost.BoostStartApplyed -= StartBlinkFinishBoostButton;
					finishIslandBoost.BoostStopApplyed -= StopBlinkFinishBoostButton;
				}

				if (boostButton.Key is PaintAmountReduceBoost paintReduceBoost)
				{
					paintReduceBoost.BoostApplyed -= PlayReduceColorEffect;
				}
			}

			_islandFinishEvent.IslandFinished -= OnIslandAutoFinished;
		}

	    private void OnIslandAutoFinished(Island island)
	    {
	        Vector3 islandCenter = Vector3.zero;

	        foreach (IslandPoint point in island.Points)
	        {
	            islandCenter += point.Transform.position;
	        }

	        islandCenter = islandCenter / island.Points.Count;

	        _finishIslandEffect.transform.position = islandCenter + _effectOffset;
	        _finishIslandEffect.SetActive(true);
	    }

	    private void PlayReduceColorEffect(Boost _)
	    {
	        _reduceColorEffect.SetActive(true);
	    }

	    private void StartBlinkFinishBoostButton()
	    {
	        _islandFinishBoostButton.Animator.StartBlinking();
	    }

	    private void StopBlinkFinishBoostButton()
	    {
	        _islandFinishBoostButton.Animator.StopBlinking();
	    }
	}
}
