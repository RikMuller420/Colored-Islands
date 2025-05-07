using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimeObjectiveView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelTimeText;
    [SerializeField] private TextMeshProUGUI _timeForExtraStarText;
    [SerializeField] private PanelAnimator _timePanelAnimator;

    private bool _isTimePanelDropped = false;
    private float _fadeDuration = 1f;
    private LevelSettingsData _levelData;

    public void ResetPanel(LevelSettingsData levelData)
    {
        _levelData = levelData;
        _timeForExtraStarText.text = SecondsToString(_levelData.ExtraStarTimeLimit);
        _timePanelAnimator.ResetAnimator();
        ResetLevelTimeText();
        _isTimePanelDropped = false;
    }

    public void OnTimeChanged(float time)
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

    public void StopShaking()
    {
        _timePanelAnimator.StopShaking();
    }

    private void ResetLevelTimeText()
    {
        _levelTimeText.text = (SecondsToString(0));
        _levelTimeText
            .DOFade(1f, _fadeDuration)
            .SetEase(Ease.InOutQuad);
    }

    private string SecondsToString(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        return $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
    }
}
