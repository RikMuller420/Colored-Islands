using System.Collections.Generic;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.Boosts;

namespace SlimeGround.Gameplay.Boosts
{
	public class BoostAvailabilityUpdater
	{
	    private LevelChangeEventTracker _levelChangeEventTracker;
	    
	    private Dictionary<Boost, BoostButton> _boostsButtons;

	    public BoostAvailabilityUpdater(Dictionary<Boost, BoostButton> boostsButtons,
	                                    LevelChangeEventTracker levelChangeEventTracker)
	    {
	        _boostsButtons = boostsButtons;
	        _levelChangeEventTracker = levelChangeEventTracker;

	        _levelChangeEventTracker.LevelChanged += OnLevelChanged;

	        foreach (var boostButton in _boostsButtons)
	        {
	            boostButton.Key.BoostApplyed += OnBoostApplyed;
	        }
	    }

	    private void OnBoostApplyed(Boost boost)
	    {
	        _boostsButtons[boost].DisableInteractable();
	    }

	    private void OnLevelChanged(ILevelData _)
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
}
