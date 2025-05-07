using UnityEngine;

public class LevelObjectivesPanelView : MonoBehaviour
{
    [SerializeField] private MovesObjectiveView _movesObjective;
    [SerializeField] private TimeObjectiveView _timeObjective;

    [SerializeField] private LevelProgressTracker _progressTracker;

    private void OnEnable()
    {
        _progressTracker.TrackStarted += OnTrackStarted;
        _progressTracker.TrackStopped += OnTrackStopped;
        _progressTracker.TimeChanged += _timeObjective.OnTimeChanged;
        _progressTracker.MovesChanged += _movesObjective.ChangeMoves;
    }

    private void OnDisable()
    {
        _progressTracker.TrackStarted -= OnTrackStarted;
        _progressTracker.TrackStopped -= OnTrackStopped;
        _progressTracker.TimeChanged -= _timeObjective.OnTimeChanged;
        _progressTracker.MovesChanged -= _movesObjective.ChangeMoves;
    }

    private void OnTrackStarted()
    {
        _timeObjective.ResetPanel(_progressTracker.LevelData);
        _movesObjective.ResetPanel(_progressTracker.LevelData);
    }

    private void OnTrackStopped()
    {
        _movesObjective.StopShaking();
        _timeObjective.StopShaking();
    }
}
