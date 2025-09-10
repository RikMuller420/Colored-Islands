using System;
using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    private bool _isTracking = false;
    private bool _isAngryTracking = false;
    private float _levelTime = 0f;
    private int _levelMoves = 0;
    private LevelObjectsHolder _levelDataHolder;
    private GoldCalculator _goldCalculator;
    private ScoreCalculator _scoreCalculator;
    private UnitMover _unitMover;
    private AngryTracker _angryTracker;

    public event Action LevelFinished;
    public event Action<float> TimeChanged;
    public event Action<float> AngryChanged;
    public event Action AngryTaskFailed;
    public event Action TrackStopped;

    public bool IsLevelFinished { get; private set; }
    public bool IsAngryTaskDone => _angryTracker.AngryValue < 1f;
    public bool IsMoveTaskDone  => _levelMoves <= LevelData.ExtraStarMoveLimit;
    public int ReachedGold { get; private set; }
    public int ReachedScore { get; private set; }
    public float AngryValue => _angryTracker.AngryValue;
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

            if (_isAngryTracking)
            {
                _angryTracker.AddAngryTick();
                AngryChanged?.Invoke(_angryTracker.AngryValue);

                if (_angryTracker.AngryValue >= 1f)
                {
                    AngryTaskFailed?.Invoke();
                    _isAngryTracking = false;
                }      
            }
        }
    }

    public void Initialize(GameProgressStorage progressStorage, LevelObjectsHolder levelDataHolder,
                            UnitMover unitMover, AngryTracker angryTracker, UpgradesProvider upgradesProvider)
    {
        _levelDataHolder = levelDataHolder;
        _unitMover = unitMover;
        _angryTracker = angryTracker;
        _goldCalculator = new GoldCalculator(this, progressStorage, upgradesProvider);
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
        _isAngryTracking = true;
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
        if (_levelDataHolder.Islands != null)
        {
            foreach (Island island in _levelDataHolder.Islands)
            {
                island.IslandFinished -= OnIslandFinished;
            }
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

        if (_isAngryTracking)
        {
            _angryTracker.AddUnitsMovedTick(unitsMoveInfo);
            AngryChanged?.Invoke(_angryTracker.AngryValue);
        }
    }

    private void OnIslandFinished(Island finishedIsland)
    {
        if (_isTracking && _isAngryTracking)
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
