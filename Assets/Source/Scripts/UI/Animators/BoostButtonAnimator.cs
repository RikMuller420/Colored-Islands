using System.Collections.Generic;

public class BoostButtonAnimator
{
    private IEnumerable<BoostButton> _buferIslandBoostButtons;
    private IEnumerable<BoostButton> _objectivesFreezeButtons;
    private IEnumerable<BoostButton> _paintAmountReduceBoostButtons;
    private IEnumerable<BoostButton> _islandFinishBoostButtons;

    public void Initialize(IEnumerable<BoostButton> buferIslandBoostButtons,
                           IEnumerable<BoostButton> objectivesFreezeButtons,
                           IEnumerable<BoostButton> paintAmountReduceBoostButtons,
                           IEnumerable<BoostButton> islandFinishBoostButtons)
    {
        _buferIslandBoostButtons = buferIslandBoostButtons;
        _objectivesFreezeButtons = objectivesFreezeButtons;
        _paintAmountReduceBoostButtons = paintAmountReduceBoostButtons;
        _islandFinishBoostButtons = islandFinishBoostButtons;
    }

    public void HighlightFinishIslandBoost()
    {
        DisableInteractable(_buferIslandBoostButtons);
        DisableInteractable(_objectivesFreezeButtons);
        DisableInteractable(_paintAmountReduceBoostButtons);
        StartBlinking(_islandFinishBoostButtons);
    }

    public void StopHighlightFinishIslandBoost()
    {
        EnableInteractable(_buferIslandBoostButtons);
        EnableInteractable(_objectivesFreezeButtons);
        EnableInteractable(_paintAmountReduceBoostButtons);
        StopBlinking(_islandFinishBoostButtons);
    }

    public void EnableInteractable(IEnumerable<BoostButton> buttons)
    {
        foreach (BoostButton button in buttons)
        {
            button.EnableInteractable();
        }
    }

    public void DisableInteractable(IEnumerable<BoostButton> buttons)
    {
        foreach (BoostButton button in buttons)
        {
            button.DisableInteractable();
        }
    }

    public void StartBlinking(IEnumerable<BoostButton> buttons)
    {
        foreach (BoostButton button in buttons)
        {
            button.Animator.StartBlinking();
        }
    }

    public void StopBlinking(IEnumerable<BoostButton> buttons)
    {
        foreach (BoostButton button in buttons)
        {
            button.Animator.StopBlinking();
        }
    }
}
