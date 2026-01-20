using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void Initialize(IEnumerable<Boost> boosts, BoostAmountProvider boostAmountProvider,
                           WalletProvider walletProvider, IIslandFinishEvent islandFinishEvent,
                           RewardedAdProvider rewardedAdProvider)
    {

        var boostSoundPlayer = new BoostSoundPlayer(_gameplaySoundPlayer, _outOfBoostWindow, boosts);

        Dictionary<Boost, BoostButton> boostsButtons = new Dictionary<Boost, BoostButton>()
        {
            { FindBoost(boosts, BoostType.GrowBuferIsland), _buferIslandBoostButton },
            { FindBoost(boosts, BoostType.FreezeObjectives), _objectivesFreezeButton },
            { FindBoost(boosts, BoostType.ReducePaints), _paintAmountReduceBoostButton },
            { FindBoost(boosts, BoostType.FinishIsland), _islandFinishBoostButton }
        };

        var boostAvailabilityUpdater = new BoostAvailabilityUpdater(boostsButtons, _levelChangeEventTracker);
        var boostAnimator = new BoostAnimator(boostsButtons, islandFinishEvent, _finishIslandEffect, _reduceColorEffect);

        foreach (var boostButton in boostsButtons)
        {
            boostButton.Value.Initialize(boostButton.Key, boostAmountProvider, _boostBuyWindow);
        }

        _boostBuyWindow.Initialize(boostAmountProvider, walletProvider, rewardedAdProvider);
    }

    private Boost FindBoost(IEnumerable<Boost> boosts, BoostType boostType)
    {
        return boosts.FirstOrDefault(boost => boost.Type == boostType);
    }
}
