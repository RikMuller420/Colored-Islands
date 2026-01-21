using System.Collections.Generic;
using SlimeGround.Data.ScriptableObjects.Boosts;
using SlimeGround.Data.ScriptableObjects.Upgrades;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Menu.Windows.GameShop
{
	public class InGameShopInitializer : MonoBehaviour
	{
	    [SerializeField] private UpgradeSettings _upgradeSettings;
	    [SerializeField] private BoostSettings _boostSettings;
	    [SerializeField] private List<UpgradeOfferLine> _upgradeOfferLines = new();
	    [SerializeField] private List<BoostOfferLine> _boostOfferLines = new ();

	    public void Initialize(UpgradesProvider upgradesProvider, BoostAmountProvider boostAmountProvider,
	                            WalletProvider walletProvider)
	    {
	        foreach (UpgradeOfferLine upgradeOfferLine in _upgradeOfferLines)
	        {
	            upgradeOfferLine.Initialize(upgradesProvider, _upgradeSettings, walletProvider);
	        }

	        foreach (BoostOfferLine boostOfferLine in _boostOfferLines)
	        {
	            boostOfferLine.Initialize(boostAmountProvider, _boostSettings, walletProvider);
	        }
	    }
	}
}
