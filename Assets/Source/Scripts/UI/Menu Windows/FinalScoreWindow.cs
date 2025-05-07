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
    [SerializeField] private NumberTextGrowAnimator _scoreAnimator;
    [SerializeField] private ObjectivesAnimator _objectivesAnimator;

    [SerializeField] private LevelProgressTracker _progressTracker;

    private LevelSettingsData _levelData;

    private new void OnEnable()
    {
        _progressTracker.LevelFinished += ShowWinAnimation;
        base.OnEnable();
    }

    private new void OnDisable()
    {
        _progressTracker.LevelFinished -= ShowWinAnimation;
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
    }

    private void PreparePanel()
    {
        UpdateLevelData();
        _objectivesAnimator.ResetObjectives();
        _starsAnimator.ResetStars();
        _scoreAnimator.ResetAnimation();
        UpdateLevelNumber();
    }

    private IEnumerator WinAnimation()
    {
        _scoreAnimator.ShowGrowAnimation(_progressTracker.ReachedScore);
        _starsAnimator.PlayNextStarAnimation();

        yield return new WaitForSeconds(_starsAnimator.AnmationDuration);

        _objectivesAnimator.ShowTimeObjectiveAnimation(_progressTracker, out float animationDuration);

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

        _objectivesAnimator.ShowGoldAnimation(_progressTracker);
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
