using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelObjectivesPanelView : MonoBehaviour
{
    [SerializeField] private Image _moveFiller;
    [SerializeField] private TextMeshProUGUI _restAviableMovesText;
    [SerializeField] private TextMeshProUGUI _levelTimeText;
    [SerializeField] private TextMeshProUGUI _movesForExtraStarText;
    [SerializeField] private TextMeshProUGUI _timeForExtraStarText;
    [SerializeField] private RectTransform _movesForExtraStarPanel;
    [SerializeField] private PanelAnimator _movesPanelAnimator;
    [SerializeField] private PanelAnimator _timePanelAnimator;

    [SerializeField] private LevelProgressTracker _progressTracker;

    private LevelSettingsData _levelData;
    private bool _isMovePanelDropped = false;
    private bool _isTimePanelDropped = false;
    private float _fadeDuration = 1f;

    private void OnEnable()
    {
        _progressTracker.TrackStarted += OnTrackStarted;
        _progressTracker.TrackStopped += OnTrackStopped;
        _progressTracker.TimeChanged += OnTimeChanged;
        _progressTracker.MovesChanged += OnMovesChanged;
    }

    private void OnDisable()
    {
        _progressTracker.TrackStarted -= OnTrackStarted;
        _progressTracker.TrackStopped -= OnTrackStopped;
        _progressTracker.TimeChanged -= OnTimeChanged;
        _progressTracker.MovesChanged -= OnMovesChanged;
    }

    private void OnTrackStarted()
    {
        _levelData = _progressTracker.LevelData;
        ResetLevelTimeText();
        ResetMovesPanel();
        ResetTimePanel();
    }

    private void OnTrackStopped()
    {
        _movesPanelAnimator.StopShaking();
        _timePanelAnimator.StopShaking();
    }

    private void OnTimeChanged(float time)
    {
        if (_isTimePanelDropped)
        {
            return;
        }

        _levelTimeText.text = SecondsToString(time);

        if (time > _levelData.ExtraStarTimeLimit)
        {
            _isTimePanelDropped = true;
            _timePanelAnimator.DropPanel();
            _levelTimeText
                .DOFade(0f, _fadeDuration)
                .SetEase(Ease.InOutQuad);
        }
    }

    private void OnMovesChanged(int moves)
    {
        _restAviableMovesText.text = (_levelData.LevelMoveLimit - moves).ToString();

        float restMovesFillAmount = 1f - (float)moves / _levelData.LevelMoveLimit;
        _moveFiller.fillAmount = restMovesFillAmount;

        if (_isMovePanelDropped == false && moves > _levelData.ExtraStarMoveLimit)
        {
            _isMovePanelDropped = true;
            _movesPanelAnimator.DropPanel();
        }
    }

    private void ResetLevelTimeText()
    {
        _levelTimeText.text = "0:00";
        _levelTimeText
            .DOFade(1f, _fadeDuration)
            .SetEase(Ease.InOutQuad);
    }

    private void ResetMovesPanel()
    {
        _moveFiller.fillAmount = 1f;
        _restAviableMovesText.text = _levelData.LevelMoveLimit.ToString();
        _movesForExtraStarText.text = _levelData.ExtraStarMoveLimit.ToString();

        PlaceMovesPanel();

        _movesPanelAnimator.ResetAnimator();
        _isMovePanelDropped = false;
    }

    private void ResetTimePanel()
    {
        _timeForExtraStarText.text = SecondsToString(_levelData.ExtraStarTimeLimit);
        _timePanelAnimator.ResetAnimator();
        _isTimePanelDropped = false;
    }

    private string SecondsToString(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        return $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
    }

    private void PlaceMovesPanel()
    {
        float anchorPositionX = 1f - (float)_levelData.ExtraStarMoveLimit / _levelData.LevelMoveLimit;

        Vector2 anchorMin = _movesForExtraStarPanel.anchorMin;
        Vector2 anchorMax = _movesForExtraStarPanel.anchorMax;

        anchorMin.x = anchorPositionX;
        anchorMax.x = anchorPositionX;

        _movesForExtraStarPanel.anchorMin = anchorMin;
        _movesForExtraStarPanel.anchorMax = anchorMax;
    }
}
