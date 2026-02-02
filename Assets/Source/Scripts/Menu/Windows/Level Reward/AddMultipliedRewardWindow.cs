using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.LevelRewards;
using SlimeGround.Menu.Extensions.Windows;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Menu.Windows.LevelReward
{
	public class AddMultipliedRewardWindow : MenuWindow
	{
	    [SerializeField] private LevelRewardView _levelRewardView;

	    public void Initialize(UnitsHatSettings unitsHatSettings, UpgradesProvider upgradesProvider)
	    {
	        _levelRewardView.Initialize(unitsHatSettings, upgradesProvider);
	    }

	    public void Open(LevelRewardData levelRewardData, int adsMultiplier)
	    {
	        _levelRewardView.SetIcons(levelRewardData, adsMultiplier);
	        Open();
	    }
	}
}
