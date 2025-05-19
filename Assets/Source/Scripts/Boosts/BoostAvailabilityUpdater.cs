using System.Collections.Generic;

public class BoostAvailabilityUpdater
{
    private LevelLoader _levelLoader;
    
    private Dictionary<Boost, BoostButton> _boostsButtons;

    public BoostAvailabilityUpdater(Dictionary<Boost, BoostButton> boostsButtons,
                                    LevelLoader levelLoader)
    {
        _boostsButtons = boostsButtons;
        _levelLoader = levelLoader;

        _levelLoader.LevelChanged += OnLevelChanged;

        foreach (var boostButton in _boostsButtons)
        {
            boostButton.Key.BoostApplyed += OnBoostApplyed;
        }
    }

    private void OnBoostApplyed(Boost boost)
    {
        _boostsButtons[boost].DisableInteractable();
    }

    private void OnLevelChanged()
    {
        EnableInteractibleInButtons();
    }

    private void EnableInteractibleInButtons()
    {
        foreach (var boostButton in _boostsButtons)
        {
            boostButton.Value.EnableInteractable();
        }
    }
}
