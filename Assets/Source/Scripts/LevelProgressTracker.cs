using System;
using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    private bool _isTracking = false;
    private float _levelTime = 0f;
    private int _levelMoves = 0;
    private LevelObjectsHolder _levelDataHolder;
    private GoldCalculator _goldCalculator;
    private ScoreCalculator _scoreCalculator;
    private UnitMover _unitMover;
    private AngryTracker _angryTracker;

    public event Action LevelFinished;
    public event Action LevelFailed;
    public event Action<float> TimeChanged;
    public event Action<float> AngryChanged;
    public event Action TrackStopped;
    public bool IsLevelFinished { get; private set; }
    public bool IsTimeTaskDone { get => _levelTime <= LevelData.ExtraStarTimeLimit; }
    public bool IsMoveTaskDone { get => _levelMoves <= LevelData.ExtraStarMoveLimit; }
    public int ReachedGold { get; private set; }
    public int ReachedScore { get; private set; }
    public LevelSettingsData LevelData => _levelDataHolder.LevelSettings;

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

            _angryTracker.AddAngryTick();
            AngryChanged?.Invoke(_angryTracker.AngryValue);

            if (_angryTracker.AngryValue >= 1f)
            {
                FailLevel();
            }
        }
    }

    public void Initialize(GameProgressStorage progressStorage, LevelObjectsHolder levelDataHolder,
                            UnitMover unitMover, AngryTracker angryTracker)
    {
        _levelDataHolder = levelDataHolder;
        _unitMover = unitMover;
        _angryTracker = angryTracker;
        _goldCalculator = new GoldCalculator(this, progressStorage);
        _scoreCalculator = new ScoreCalculator(this, levelDataHolder);
        enabled = true;
    }

    public void StartTracking()
    {
        StopTracking();

        foreach (Island island in _levelDataHolder.Islands)
        {
            island.IslandFinished += OnIslandFinished;
        }

        _levelMoves = 0;
        _levelTime = 0f;
        ReachedGold = 0;
        ReachedScore = 0;
        _angryTracker.ResetAngryValue();
        IsLevelFinished = false;
        _isTracking = true;
    }

    public void PauseTracking()
    {
        _isTracking = false;
    }

    public void ContinueTracking()
    {
        _isTracking = true;
    }

    public void StopTracking()
    {
        foreach (Island island in _levelDataHolder.Islands)
        {
            island.IslandFinished -= OnIslandFinished;
        }

        _isTracking = false;
        TrackStopped?.Invoke();
    }

    private void OnUnitsMoved(UnitsMoveInfo unitsMoveInfo)
    {
        if (_isTracking == false)
        {
            return;
        }

        _levelMoves++;

        _angryTracker.AddUnitsMovedTick(unitsMoveInfo);
        AngryChanged?.Invoke(_angryTracker.AngryValue);
    }

    private void OnIslandFinished(Island finishedIsland)
    {
        if (_isTracking)
        {
            _angryTracker.AddIslandFinishedTick(finishedIsland);
            AngryChanged?.Invoke(_angryTracker.AngryValue);
        }

        foreach (Island island in _levelDataHolder.Islands)
        {
            if (island.IsDone == false)
            {
                return;
            }
        }

        IsLevelFinished = true;
        StopTracking();
        CalculateRewardsAmount();
        LevelFinished?.Invoke();
    }

    public void FailLevel()
    {
        StopTracking();
        CalculateRewardsAmount();
        LevelFailed?.Invoke();
    }

    //Under TEST UI only
    public void FinishLevel()
    {
        IsLevelFinished = true;
        StopTracking();
        CalculateRewardsAmount();
        LevelFinished?.Invoke();
    }

    private void CalculateRewardsAmount()
    {
        ReachedGold = _goldCalculator.CalculateGold();
        ReachedScore = _scoreCalculator.CalculateScore(_levelTime, _levelMoves);
    }
}
