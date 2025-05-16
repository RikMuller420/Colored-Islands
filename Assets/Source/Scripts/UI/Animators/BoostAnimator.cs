using System.Collections.Generic;
using UnityEngine;

public class BoostAnimator
{
    private Dictionary<Boost, IEnumerable<BoostButton>> _boostsButtons;
    private IEnumerable<BoostButton> _islandFinishBoostButtons;
    private GameObject _objectiveFreezeAnimator;

    public BoostAnimator(Dictionary<Boost, IEnumerable<BoostButton>> boostsButtons,
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

                _islandFinishBoostButtons = boostButton.Value;
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
        foreach (BoostButton button in _islandFinishBoostButtons)
        {
            button.Animator.StartBlinking();
        }
    }
    private void StopBlinkFinishBoostButton()
    {
        foreach (BoostButton button in _islandFinishBoostButtons)
        {
            button.Animator.StopBlinking();
        }
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
