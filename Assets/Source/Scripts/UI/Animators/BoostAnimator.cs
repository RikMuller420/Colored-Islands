using System.Collections.Generic;
using UnityEngine;

public class BoostAnimator
{
    private Dictionary<Boost, BoostButton> _boostsButtons;
    private BoostButton _islandFinishBoostButton;

    public BoostAnimator(Dictionary<Boost, BoostButton> boostsButtons)
    {
        _boostsButtons = boostsButtons;

        foreach (var boostButton in _boostsButtons)
        {
            if (boostButton.Key is IslandFinishBoost finishIslandBoost)
            {
                finishIslandBoost.BoostStartApplyed += StartBlinkFinishBoostButton;
                finishIslandBoost.BoostStopApplyed += StopBlinkFinishBoostButton;

                _islandFinishBoostButton = boostButton.Value;
            }
        }
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
