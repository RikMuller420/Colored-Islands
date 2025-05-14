using System;
using UnityEngine;

public class LevelProgressTracker : MonoBehaviour
{
    [SerializeField] private UnitMover _unitMover;
    [SerializeField] private FinalScoreWindow _finalScoreWindow;

    private bool _isTracking = false;
    private float _levelTime = 0f;
    private int _levelMoves = 0;
    private LevelDataHolder _levelDataHolder;
    private GoldCalculator _goldCalculator;
    private ScoreCalculator _scoreCalculator;

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

    public void Initialize(GameProgressStorage progressStorage, LevelDataHolder levelDataHolder)
    {
        _levelDataHolder = levelDataHolder;
        _goldCalculator = new GoldCalculator(this, progressStorage);
        _scoreCalculator = new ScoreCalculator(this, levelDataHolder);
        enabled = true;
    }

    public void StartTrack()
    {
        StopTrack();

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

    public void StopTrack()
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
        _levelMoves++;
        MovesChanged?.Invoke(_levelMoves);

        if (_levelMoves == LevelData.LevelMoveLimit)
        {
            StopTrack();
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
        StopTrack();
        CalculateRewardsAmount();
        LevelFinished?.Invoke();
    }

    //Under TEST UI only
    public void FailLevel()
    {
        StopTrack();
        CalculateRewardsAmount();
        LevelFailed?.Invoke();
    }

    //Under TEST UI only
    public void FinishLevel()
    {
        IsLevelFinished = true;
        StopTrack();
        CalculateRewardsAmount();
        LevelFinished?.Invoke();
    }

    private void CalculateRewardsAmount()
    {
        ReachedGold = _goldCalculator.CalculateGold();
        ReachedScore = _scoreCalculator.CalculateScore(_levelTime, _levelMoves);
    }
}
