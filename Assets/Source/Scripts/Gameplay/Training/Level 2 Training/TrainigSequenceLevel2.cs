using System.Collections;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Menu.Boosts;
using UnityEngine;

namespace SlimeGround.Gameplay.Training
{
	public class TrainigSequenceLevel2 : TrainigSequence
	{
	    private float _startDelay = 1f;
	    private WaitForSeconds _startWait;
	    private BoostButton _boostButton;
		private bool _isEventsSubscribed = false;
		private bool _isTrainingDone = false;
	    private RectTransform _buttonRectTransform;

	    private void Awake()
	    {
	        _startWait = new WaitForSeconds(_startDelay);
	    }

	    private void OnDestroy()
	    {
			if (_isEventsSubscribed)
			{
				InGameMenu.MenuOpened -= OnMenuOpened;
				InGameMenu.MenuClosed -= OnMenuClosed;
				_boostButton.TryBoostApplying -= OnTryApplyingBoost;
				ScreenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged;

				_isEventsSubscribed = false;
			}

	        ResetLevelState();
	    }

	    public override void StartTraining()
	    {
			BoostButtonActivator.DeactivateAllButtons();
	        DeactivateAllColliders();
	        
	        _boostButton = BoostButtonActivator.GetBoostButton(BoostType.GrowBuferIsland);
	        BoostButtonActivator.ActivateButtonImmediate(BoostType.GrowBuferIsland);
	        _buttonRectTransform = BoostButtonActivator.GetBoostButtonRectTransform(BoostType.GrowBuferIsland);

			if (_isEventsSubscribed == false)
			{
				InGameMenu.MenuOpened += OnMenuOpened;
				InGameMenu.MenuClosed += OnMenuClosed;
				ScreenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;
				_boostButton.TryBoostApplying += OnTryApplyingBoost;

				_isEventsSubscribed = true;
			}

			StartCoroutine(StartFirstTrainingMove());
	    }

	    private void OnScreenSizeChanged(Vector2 _) => UpdatePointerPosition(_buttonRectTransform);

	    private void OnMenuOpened()
	    {
	        if (_isTrainingDone == false)
	        {
	            Pointer.gameObject.SetActive(false);
	        }
	    }

	    private void OnMenuClosed()
	    {
	        if (_isTrainingDone == false)
	        {
	            Pointer.gameObject.SetActive(true);
	        }
	    }

	    private IEnumerator StartFirstTrainingMove()
	    {
	        yield return _startWait;

	        UpdatePointerPosition(_buttonRectTransform);
	        ActivatePointer();
			GameplayDimmer.Activate();
	    }

	    private void OnTryApplyingBoost()
	    {
	        AddBost(BoostType.GrowBuferIsland);
	        ActivateAllColliders();
	        DeactivatePointer();
			GameplayDimmer.Deactivate();
			_isTrainingDone = true;
	    }
	}
}
