using System.Collections.Generic;

public class BoostAvailabilityUpdater
{
    private LevelLoader _levelLoader;
    
    private Dictionary<Boost, IEnumerable<BoostButton>> _boostsButtons;

    public BoostAvailabilityUpdater(Dictionary<Boost, IEnumerable<BoostButton>> boostsButtons,
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
        foreach (BoostButton button in _boostsButtons[boost])
        {
            button.DisableInteractable();
        }
    }

    private void OnLevelChanged()
    {
        EnableInteractibleInButtons();
    }

    private void EnableInteractibleInButtons()
    {
        foreach (var boostButton in _boostsButtons)
        {
            foreach (BoostButton button in boostButton.Value)
            {
                button.EnableInteractable();
            }
        }
    }
}
