using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    [SerializeField] private UnitMover _unitMover;
    [SerializeField] private FinalScoreWindow _finalScoreWindow;

    private bool _isTracking = false;
    private IReadOnlyCollection<Island> _islands = new List<Island>();
    private float _levelTime = 0f;
    private int _levelMoves = 0;

    public event Action LevelFinished;
    public event Action LevelFailed;
    public event Action<float> TimeChanged;
    public event Action<int> MovesChanged;
    public event Action TrackStarted;
    public event Action TrackStopped;

    public bool IsTimeTaskDone { get => _levelTime <= LevelData.ExtraStarTimeLimit; }
    public bool IsMoveTaskDone { get => _levelMoves <= LevelData.ExtraStarMoveLimit; }
    public int ReachedGold { get => 10; }
    public int ReachedScore { get => 15000; }

    public LevelSettingsData LevelData { get; private set; }

    private void OnEnable()
    {
        _unitMover.UnitsMoved += OnUnitsMoved;
    }

    private void OnDisable()
    {
        _unitMover.UnitsMoved -= OnUnitsMoved;
    }

    private void Update()
    {
        if (_isTracking)
        {
            _levelTime += Time.deltaTime;
            TimeChanged?.Invoke(_levelTime);
        }
    }

    public void StartTrack(IslandsGroupInitializer islandsGroup, LevelSettingsData levelData)
    {
        StopTrack();

        _islands = islandsGroup.Islands;

        foreach (Island island in _islands)
        {
            island.IslandFinished += OnIslandFinished;
        }

        LevelData = levelData;
        _levelMoves = 0;
        _levelTime = 0f;
        _isTracking = true;
        TrackStarted?.Invoke();
    }

    public void StopTrack()
    {
        foreach (Island island in _islands)
        {
            island.IslandFinished -= OnIslandFinished;
        }

        _isTracking = false;
        TrackStopped?.Invoke();
    }

    private void OnUnitsMoved()
    {
        _levelMoves++;
        MovesChanged?.Invoke(_levelMoves);

        if (_levelMoves == LevelData.LevelMoveLimit)
        {
            StopTrack();
            LevelFailed?.Invoke();
        }
    }

    private void OnIslandFinished()
    {
        /*foreach (Island island in _islands)
        {
            if (island.IsDone == false)
            {
                return;
            }
        }*/

        StopTrack();
        LevelFinished?.Invoke();
    }
}
