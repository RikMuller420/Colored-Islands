using System.Collections;
using TMPro;
using UnityEngine;

public class FinalScoreWindow : MenuWindow
{
    [SerializeField] private GameObject _winTitle;
    [SerializeField] private GameObject _looseTitle;
    [SerializeField] private NextLevelButton _nextLevelButton;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private StarsAnimator _starsAnimator;
    [SerializeField] private ScoreAnimator _scoreAnimator;
    [SerializeField] private ObjectivesAnimator _objectivesAnimator;
    [SerializeField] private ResultButtons _resultButtons;

    [SerializeField] private LevelProgressTracker _progressTracker;

    private LevelSettingsData _levelData;
    private float _lastAnimationTimeReduction = 2f;

    private new void OnEnable()
    {
        _progressTracker.LevelFinished += ShowWinAnimation;
        _progressTracker.LevelFailed += ShowFailAnimation;
        base.OnEnable();
    }

    private new void OnDisable()
    {
        _progressTracker.LevelFinished -= ShowWinAnimation;
        _progressTracker.LevelFailed += ShowFailAnimation;
        base.OnDisable();
    }

    public void ShowWinAnimation()
    {
        _winTitle.SetActive(true);
        _looseTitle.SetActive(false);
        _nextLevelButton.gameObject.SetActive(true);

        PreparePanel();
        Open();
        StartCoroutine(WinAnimation());
    }

    public void ShowFailAnimation()
    {
        _winTitle.SetActive(false);
        _looseTitle.SetActive(true);
        _nextLevelButton.gameObject.SetActive(false);

        PreparePanel();
        Open();
        StartCoroutine(FailAnimation());
    }

    private void PreparePanel()
    {
        UpdateLevelData();
        UpdateLevelNumber();
        _objectivesAnimator.ResetObjectives();
        _starsAnimator.ResetStars();
        _scoreAnimator.ResetAnimation();
        _resultButtons.ResetButtons();
    }

    private IEnumerator FailAnimation()
    {
        _scoreAnimator.ShowFailAnimation(_progressTracker.ReachedScore);
        _objectivesAnimator.ShowTimeObjectiveAnimation(_progressTracker, out float animationDuration);

        yield return new WaitForSeconds(animationDuration);

        _objectivesAnimator.ShowMoveObjectiveAnimation(_progressTracker, out animationDuration);

        yield return new WaitForSeconds(animationDuration);

        _objectivesAnimator.ShowGoldAnimation(_progressTracker, out animationDuration);
        animationDuration /= _lastAnimationTimeReduction;

        yield return new WaitForSeconds(animationDuration);

        _resultButtons.Activate();
    }

    private IEnumerator WinAnimation()
    {
        _scoreAnimator.ShowWinAnimation(_progressTracker.ReachedScore);
        _starsAnimator.PlayNextStarAnimation();
        float animationDuration = _starsAnimator.AnmationDuration;

        yield return new WaitForSeconds(animationDuration);

        _objectivesAnimator.ShowTimeObjectiveAnimation(_progressTracker, out animationDuration);

        if (_progressTracker.IsTimeTaskDone)
        {
            _starsAnimator.PlayNextStarAnimation();
        }

        yield return new WaitForSeconds(animationDuration);

        _objectivesAnimator.ShowMoveObjectiveAnimation(_progressTracker, out animationDuration);

        if (_progressTracker.IsMoveTaskDone)
        {
            _starsAnimator.PlayNextStarAnimation();
        }

        yield return new WaitForSeconds(animationDuration);

        _objectivesAnimator.ShowGoldAnimation(_progressTracker, out animationDuration);
        animationDuration /= _lastAnimationTimeReduction;

        yield return new WaitForSeconds(animationDuration);

        _resultButtons.Activate();
    }

    private void UpdateLevelData()
    {
        _levelData = _progressTracker.LevelData;
    }

    private void UpdateLevelNumber()
    {
        _levelNumberText.text = "Level " + _levelData.Id.ToString();
    }
}
