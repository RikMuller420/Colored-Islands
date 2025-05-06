using System;
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

    [SerializeField] private LevelProgressTracker _progressTracker;

    private LevelSettingsData _levelData;

    private void OnEnable()
    {
        _progressTracker.TrackStarted += OnTrackStarted;
        _progressTracker.TimeChanged += OnTimeChanged;
        _progressTracker.MovesChanged += OnMovesChanged;
    }

    private void OnDisable()
    {
        _progressTracker.TrackStarted -= OnTrackStarted;
        _progressTracker.TimeChanged -= OnTimeChanged;
        _progressTracker.MovesChanged -= OnMovesChanged;
    }

    private void OnTrackStarted()
    {
        _levelData = _progressTracker.LevelData;

        _moveFiller.fillAmount = 1f;
        _restAviableMovesText.text = _levelData.LevelMoveLimit.ToString();
        _levelTimeText.text = "0:00";
        _movesForExtraStarText.text = _levelData.ExtraStarMoveLimit.ToString();
        _timeForExtraStarText.text = SecondsToString(_levelData.ExtraStarTimeLimit);
        PlaceMovesPanel();
    }

    private void OnTimeChanged(float time)
    {
        _levelTimeText.text = SecondsToString(time);
    }

    private void OnMovesChanged(int moves)
    {
        _restAviableMovesText.text = (_levelData.LevelMoveLimit - moves).ToString();

        float restMovesFillAmount = 1f - (float)moves / _levelData.LevelMoveLimit;
        _moveFiller.fillAmount = restMovesFillAmount;
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
