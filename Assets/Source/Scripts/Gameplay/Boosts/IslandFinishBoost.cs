using System;
using SlimeGround.Core.InputHandling;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;

namespace SlimeGround.Gameplay.Boosts
{
	public class IslandFinishBoost : Boost
	{
	    private ClickHandler _clickHandler;
	    private IslandFinishClickBehaviour _islandFinishBehaviour;
	    private LevelChangeEventTracker _levelChangeEventTracker;

	    private bool _isBoostApplying = false;

	    public IslandFinishBoost(ClickHandler clickHandler,
	                             IslandFinishClickBehaviour islandInstantFinisher,
	                             LevelChangeEventTracker levelChangeEventTracker,
								 BoostAmountProvider boostAmountProvider) : base(boostAmountProvider)
	    {
	        _clickHandler = clickHandler;
	        _islandFinishBehaviour = islandInstantFinisher;
	        _levelChangeEventTracker = levelChangeEventTracker;

	        _islandFinishBehaviour.IslandFinished += OnIslandFinished;
	        _levelChangeEventTracker.LevelChanged += OnLevelChanged;
	    }

		public event Action BoostStartApplyed;
		public event Action BoostStopApplyed;

		public override BoostType Type => BoostType.FinishIsland;

		public void Dispose()
		{
			_islandFinishBehaviour.IslandFinished -= OnIslandFinished;
			_levelChangeEventTracker.LevelChanged -= OnLevelChanged;
		}

	    public override void TryApplyBoost()
	    {
	        if (_isBoostApplying)
	        {
	            StopBoostApplying();
	        }
	        else
	        {
	            StartBoostApplying();
	        }
	    }

	    private void StartBoostApplying()
	    {
	        _clickHandler.SetClickBehaviour(_islandFinishBehaviour);
	        _isBoostApplying = true;
	        BoostStartApplyed?.Invoke();
	    }

	    private void StopBoostApplying()
	    {
	        _clickHandler.SetDeafultClickHandler();
	        _isBoostApplying = false;
	        BoostStopApplyed?.Invoke();
	    }

	    private void OnIslandFinished(Island _)
	    {
	        if (_isBoostApplying)
	        {
	            StopBoostApplying();
	            SpendBoost(Type);
	        }
	    }

	    private void OnLevelChanged(ILevelData _)
	    {
	        if (_isBoostApplying)
	        {
	            StopBoostApplying();
	        }
	    }
	}
}
