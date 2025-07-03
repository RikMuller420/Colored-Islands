using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class FinalScoreWindow : MenuWindow
{
    [SerializeField] private NextLevelButton _nextLevelButton;
    [SerializeField] private StarsAnimator _starsAnimator;
    [SerializeField] private ScoreAnimator _scoreAnimator;
    [SerializeField] private ObjectivesAnimator _objectivesAnimator;
    [SerializeField] private ResultButtons _resultButtons;
    [SerializeField] private ZoneUi _boostButtonZone;
    [SerializeField] private GameObject _confettiParticle;

    private GameProgressStorage _progressStorage;
    private LevelProgressTracker _progressTracker;
    private LevelLoader _levelLoader;

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
        _levelLoader.LevelChanged += OnLevelChanged;
        base.OnEnable();
    }

    private new void OnDisable()
    {
        _progressTracker.LevelFinished -= OnLevelFinished;
        _levelLoader.LevelChanged -= OnLevelChanged;
        base.OnDisable();
    }

    public void Initialize(GameProgressStorage progressStorage, LevelProgressTracker progressTracker,
                           LevelLoader levelLoader)
    {
        _openWindowAwait = new WaitForSeconds(_openWindowDelay);
        _starAnimationInterval = new WaitForSeconds(_starAnimationTime);
        _progressStorage = progressStorage;
        _progressTracker = progressTracker;
        _levelLoader = levelLoader;
        enabled = true;
    }

    private void OnLevelChanged()
    {
        if (_openWindowCorutine != null)
        {
            StopCoroutine(_openWindowCorutine);
            _openWindowCorutine = null;
        }
    }

    private void OnLevelFinished()
    {
        int currentLevelId = _progressTracker.LevelData.Id;
        LevelProgress nextLevel = _progressStorage.Levels.FirstOrDefault(level => level.Id > currentLevelId);
        bool isNextLevelExist = _progressStorage.FirstUnfinishedLevel != null;
        _nextLevelButton.gameObject.SetActive(nextLevel != null);
        _boostButtonZone.Close();

        if (nextLevel != null)
        {
            _nextLevelButton.SetNextLevelId(nextLevel.Id);
        }

        _openWindowCorutine = StartCoroutine(ShowWinAnimation());
    }

    private IEnumerator ShowWinAnimation()
    {
        yield return _openWindowAwait;

        _confettiParticle.SetActive(true);
        PreparePanel();
        OpenUnclosableWindow();
        StartCoroutine(PlayStarAnimations());
        StartCoroutine(WinAnimation());
    }

    private void PreparePanel()
    {
        _objectivesAnimator.ResetObjectives();
        _starsAnimator.ResetStars();
        _scoreAnimator.ResetAnimation();
        _resultButtons.ResetButtons();
    }

    private IEnumerator WinAnimation()
    {
        _scoreAnimator.ShowWinAnimation(_progressTracker.ReachedScore);
        float animationDuration;
        _objectivesAnimator.ShowAngryScoreAnimation(_progressTracker, out animationDuration);

        yield return new WaitForSeconds(animationDuration);

        _objectivesAnimator.ShowMoveObjectiveAnimation(_progressTracker, out animationDuration);

        yield return new WaitForSeconds(animationDuration);

        _objectivesAnimator.ShowGoldAnimation(_progressTracker, out animationDuration);
        animationDuration /= _lastAnimationTimeReduction;

        _resultButtons.Activate();
        ScoreShowed?.Invoke();
    }

    private IEnumerator PlayStarAnimations()
    {
        _starsAnimator.PlayNextStarAnimation();

        yield return _starAnimationInterval;

        if(_progressTracker.IsAngryTaskDone)
        {
            _starsAnimator.PlayNextStarAnimation();
        }

        yield return _starAnimationInterval;

        if (_progressTracker.IsMoveTaskDone)
        {
            _starsAnimator.PlayNextStarAnimation();
        }
    }
}
