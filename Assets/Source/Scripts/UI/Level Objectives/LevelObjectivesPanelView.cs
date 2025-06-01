using UnityEngine;

public class LevelObjectivesPanelView : MonoBehaviour
{
    [SerializeField] private TimeObjectiveView _timeObjective;
    [SerializeField] private LevelLoader _levelLoader;

    [SerializeField] private LevelProgressTracker _progressTracker;

    private void OnEnable()
    {
        _levelLoader.LevelChanged += OnLevelChanged;
        _progressTracker.TrackStopped += OnTrackStopped;
        _progressTracker.TimeChanged += _timeObjective.OnTimeChanged;
    }

    private void OnDisable()
    {
        _levelLoader.LevelChanged -= OnLevelChanged;
        _progressTracker.TrackStopped -= OnTrackStopped;
        _progressTracker.TimeChanged -= _timeObjective.OnTimeChanged;
    }

    private void OnLevelChanged()
    {
        _timeObjective.ResetPanel(_progressTracker.LevelData);
    }

    private void OnTrackStopped()
    {
        _timeObjective.StopShaking();
    }
}
