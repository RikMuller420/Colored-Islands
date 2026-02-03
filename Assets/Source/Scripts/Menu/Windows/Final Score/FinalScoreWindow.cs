using System;
using System.Collections;
using System.Linq;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.LevelRewards;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.Extensions.Windows;
using SlimeGround.Menu.LevelNavigation;
using SlimeGround.Menu.Windows.LevelReward;
using UnityEngine;

namespace SlimeGround.Menu.Windows.FinalScore
{
	public class FinalScoreWindow : MenuWindow
	{
	    [SerializeField] private LevelRewardSettings _levelRewardSettings;

	    [SerializeField] private NextLevelButton _nextLevelButton;
	    [SerializeField] private StarsAnimator _starsAnimator;
	    [SerializeField] private ScoreAnimator _scoreAnimator;
	    [SerializeField] private ObjectivesAnimator _objectivesAnimator;
	    [SerializeField] private ResultButtons _resultButtons;
	    [SerializeField] private ZoneUi _boostButtonZone;
	    [SerializeField] private GameObject _confettiParticle;
	    [SerializeField] private LevelRewardWindow _levelRewardWindow;
	    [SerializeField] private ZoneUi _angryBar;
	    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
	    [SerializeField] private LevelProgressTracker _progressTracker;

	    private IPlayerData _playerData;

	    private float _openWindowDelay = 1f;
	    private float _lastAnimationTimeReduction = 2f;
	    private float _starAnimationTime = 0.75f;
	    private WaitForSeconds _openWindowAwait;
	    private WaitForSeconds _starAnimationInterval;
	    private Coroutine _openWindowCorutine;

	    public event Action ScoreShowed;

	    private new void OnEnable()
	    {
	        _progressTracker.LevelFinished += OnLevelFinished;
	        _levelChangeEventTracker.LevelChanged += OnLevelChanged;
	        base.OnEnable();
	    }

	    private new void OnDisable()
	    {
	        _progressTracker.LevelFinished -= OnLevelFinished;
	        _levelChangeEventTracker.LevelChanged -= OnLevelChanged;
	        base.OnDisable();
	    }

	    public void Initialize(IPlayerData playerData)
	    {
	        _playerData = playerData;
	        _openWindowAwait = new WaitForSeconds(_openWindowDelay);
	        _starAnimationInterval = new WaitForSeconds(_starAnimationTime);

	        enabled = true;
	    }

	    private void OnLevelChanged(ILevelData _)
	    {
	        if (_openWindowCorutine != null)
	        {
	            StopCoroutine(_openWindowCorutine);
	            _openWindowCorutine = null;
	        }
	    }

	    private void OnLevelFinished(ILevelData levelData)
	    {
	        int currentLevelId = levelData.LevelId;
	        LevelProgress nextLevel = _playerData.Levels.FirstOrDefault(level => level.Id > currentLevelId);
	        bool isNextLevelExist = _playerData.FirstUnfinishedLevel != null;
	        _nextLevelButton.gameObject.SetActive(nextLevel != null);
	        _boostButtonZone.Close();

	        if (nextLevel != null)
	        {
	            _nextLevelButton.SetNextLevelId(nextLevel.Id);
	        }

	        _openWindowCorutine = StartCoroutine(ShowWinAnimation(levelData));
	    }

	    private IEnumerator ShowWinAnimation(ILevelData levelData)
	    {
	        yield return _openWindowAwait;

	        _confettiParticle.SetActive(true);
	        PreparePanel();
	        OpenUnclosableWindow();
	        _angryBar.Close();
	        StartCoroutine(PlayStarAnimations());
	        StartCoroutine(WinAnimation(levelData));
	    }

	    private void PreparePanel()
	    {
	        _objectivesAnimator.ResetObjectives();
	        _starsAnimator.ResetStars();
	        _scoreAnimator.ResetAnimation();
	        _resultButtons.ResetButtons();
	    }

	    private IEnumerator WinAnimation(ILevelData levelData)
	    {
	        _scoreAnimator.ShowWinAnimation(_progressTracker.ReachedScore);
	        float animationDuration;
	        _objectivesAnimator.ShowAngryScoreAnimation(_progressTracker.AngryValue, _progressTracker.IsAngryTaskDone, out animationDuration);

	        yield return new WaitForSeconds(animationDuration);

	        _objectivesAnimator.ShowMoveObjectiveAnimation(levelData.ExtraStarMoveCount, _progressTracker.IsMoveTaskDone, out animationDuration);

	        yield return new WaitForSeconds(animationDuration);

	        _objectivesAnimator.ShowGoldAnimation(_progressTracker.ReachedGold, out animationDuration);
	        animationDuration /= _lastAnimationTimeReduction;

	        TryOpenRewardWindow(levelData);
	        _resultButtons.Activate();
	        ScoreShowed?.Invoke();
	    }

	    private IEnumerator PlayStarAnimations()
	    {
	        _starsAnimator.PlayNextStarAnimation();

	        yield return _starAnimationInterval;

	        if (_progressTracker.IsAngryTaskDone)
	        {
	            _starsAnimator.PlayNextStarAnimation();
	        }

	        yield return _starAnimationInterval;

	        if (_progressTracker.IsMoveTaskDone)
	        {
	            _starsAnimator.PlayNextStarAnimation();
	        }
	    }

	    private void TryOpenRewardWindow(ILevelData levelData)
	    {
	        int currentLevelId = levelData.LevelId;
	        LevelRewardData reward = _levelRewardSettings.LevelRewards.FirstOrDefault(reward => reward.LevelId == currentLevelId);
	        bool wasRewardReceived = _playerData.IsLevelRewardReceived(currentLevelId);

	        if (reward != null && wasRewardReceived == false)
	        {
	            _levelRewardWindow.Open(reward);
	        }
	    }
	}
}
