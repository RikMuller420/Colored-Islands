using System.Collections.Generic;
using System.Linq;
using SlimeGround.Core.InputHandling;
using SlimeGround.Effects.Sound;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Integration.Ads;
using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;

namespace SlimeGround.Menu.Boosts
{
	public class BoostViewInitializer : MonoBehaviour
	{
	    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
	    [SerializeField] private GameplaySoundPlayer _gameplaySoundPlayer;
	    [SerializeField] private MenuWindow _outOfBoostWindow;

	    [SerializeField] private BoostButton _buferIslandBoostButton;
	    [SerializeField] private BoostButton _objectivesFreezeButton;
	    [SerializeField] private BoostButton _paintAmountReduceBoostButton;
	    [SerializeField] private BoostButton _islandFinishBoostButton;

	    [SerializeField] private GameObject _finishIslandEffect;
	    [SerializeField] private GameObject _reduceColorEffect;
	    [SerializeField] private BoostBuyConfirmationWindow _boostBuyWindow;

		private BoostSoundPlayer _boostSoundPlayer;
		private BoostAvailabilityUpdater _boostAvailabilityUpdater;
		private BoostAnimator _boostAnimator;

		public void Initialize(IEnumerable<Boost> boosts, BoostAmountProvider boostAmountProvider,
	                           WalletProvider walletProvider, IIslandFinishEvent islandFinishEvent,
	                           RewardedAdProvider rewardedAdProvider)
	    {
			_boostSoundPlayer = new BoostSoundPlayer(_gameplaySoundPlayer, _outOfBoostWindow, boosts);

	        Dictionary<Boost, BoostButton> boostsButtons = new Dictionary<Boost, BoostButton>()
	        {
	            { FindBoost(boosts, BoostType.GrowBuferIsland), _buferIslandBoostButton },
	            { FindBoost(boosts, BoostType.FreezeObjectives), _objectivesFreezeButton },
	            { FindBoost(boosts, BoostType.ReducePaints), _paintAmountReduceBoostButton },
	            { FindBoost(boosts, BoostType.FinishIsland), _islandFinishBoostButton }
	        };

			_boostAvailabilityUpdater = new BoostAvailabilityUpdater(boostsButtons, _levelChangeEventTracker);
			_boostAnimator = new BoostAnimator(boostsButtons, islandFinishEvent, _finishIslandEffect, _reduceColorEffect);

	        foreach (var boostButton in boostsButtons)
	        {
	            boostButton.Value.Initialize(boostButton.Key, boostAmountProvider, _boostBuyWindow);
	        }

	        _boostBuyWindow.Initialize(boostAmountProvider, walletProvider, rewardedAdProvider);
	    }

		public void Dispose()
		{
			_boostSoundPlayer.Dispose();
			_boostAvailabilityUpdater.Dispose();
			_boostAnimator.Dispose();
		}

		private Boost FindBoost(IEnumerable<Boost> boosts, BoostType boostType) =>
						boosts.FirstOrDefault(boost => boost.Type == boostType);
	}
}
