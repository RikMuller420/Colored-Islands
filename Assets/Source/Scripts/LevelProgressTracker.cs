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

    public event Action LevelFinished;
    public event Action LevelFailed;
    public event Action<float> TimeChanged;
    public event Action<int> MovesChanged;
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
        }
    }

    public void Initialize(GameProgressStorage progressStorage, LevelObjectsHolder levelDataHolder,
                            UnitMover unitMover)
    {
        _levelDataHolder = levelDataHolder;
        _unitMover = unitMover;
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

    private void OnUnitsMoved()
    {
        if (_isTracking == false)
        {
            return;
        }

        _levelMoves++;
        MovesChanged?.Invoke(_levelMoves);

        if (_levelMoves == LevelData.LevelMoveLimit)
        {
            StopTracking();
            CalculateRewardsAmount();
            LevelFailed?.Invoke();
        }
    }

    private void OnIslandFinished()
    {
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
