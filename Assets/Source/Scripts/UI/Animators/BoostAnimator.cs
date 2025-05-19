using System.Collections.Generic;
using UnityEngine;

public class BoostAnimator
{
    private Dictionary<Boost, BoostButton> _boostsButtons;
    private BoostButton _islandFinishBoostButton;
    private GameObject _objectiveFreezeAnimator;

    public BoostAnimator(Dictionary<Boost, BoostButton> boostsButtons,
                        GameObject objectiveFreezeAnimator)
    {
        _boostsButtons = boostsButtons;
        _objectiveFreezeAnimator = objectiveFreezeAnimator;

        foreach (var boostButton in _boostsButtons)
        {
            if (boostButton.Key is IslandFinishBoost finishIslandBoost)
            {
                finishIslandBoost.BoostStartApplyed += StartBlinkFinishBoostButton;
                finishIslandBoost.BoostStopApplyed += StopBlinkFinishBoostButton;

                _islandFinishBoostButton = boostButton.Value;
            }

            if (boostButton.Key is ObjectivesFreezeBoost objectivesFreezeBoost)
            {
                objectivesFreezeBoost.BoostApplyed += StartObjectivesFreezeAnimation;
                objectivesFreezeBoost.BoostStopApplyed += StopObjectivesFreezeAnimation;
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

    private void StartObjectivesFreezeAnimation(Boost _)
    {
        _objectiveFreezeAnimator.SetActive(true);
    }

    private void StopObjectivesFreezeAnimation()
    {
        _objectiveFreezeAnimator.SetActive(false);
    }
}
