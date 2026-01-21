using System;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Gameplay.AngryBar;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Score;
using SlimeGround.Gameplay.Units;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using UnityEngine;

namespace SlimeGround.Gameplay.Levels
{
	public class LevelProgressTracker : MonoBehaviour
	{
	    [SerializeField] private LevelSettings _levelSettings;
	    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
	    [SerializeField] private LevelProgressTracker _levelProgressTracker;

	    private bool _isTrackingLevel = false;
	    private bool _isTrackingAngryValue = false;
	    private bool _firstMoveDone = false;
	    private float _levelTime = 0f;
	    private int _levelMoves = 0;
	    private ILevelData _currentLevelData;
	    private GoldCalculator _goldCalculator;
	    private LevelScoreCalculator _scoreCalculator;
	    private UnitMover _unitMover;
	    private AngryTracker _angryTracker;

	    public event Action<Island> IslandFinished;
	    public event Action<ILevelData> LevelFinished;
	    public event Action<float> TimeChanged;
	    public event Action<float> AngryChanged;
	    public event Action AngryTaskFailed;
	    public event Action TrackStopped;
	    public event Action FirstMoveDone;

	    public bool IsAngryTaskDone => _angryTracker.AngryValue < 1f;
	    public bool IsMoveTaskDone => _levelMoves <= _currentLevelData.ExtraStarMoveCount;
	    public float AngryValue => _angryTracker.AngryValue;
	    public int ReachedGold { get; private set; }
	    public int ReachedScore { get; private set; }

	    private void OnEnable()
	    {
	        _unitMover.UnitsMoved += OnUnitsMoved;
	        _levelChangeEventTracker.LevelChanged += OnLevelChanged;
	    }

	    private void OnDisable()
	    {
	        _unitMover.UnitsMoved -= OnUnitsMoved;
	        _levelChangeEventTracker.LevelChanged -= OnLevelChanged;
	    }

	    private void Update()
	    {
	        if (_isTrackingLevel)
	        {
	            _levelTime += Time.deltaTime;
	            TimeChanged?.Invoke(_levelTime);

	            if (_isTrackingAngryValue)
	            {
	                _angryTracker.AddAngryTick();
	                AngryChanged?.Invoke(_angryTracker.AngryValue);

	                if (_angryTracker.AngryValue >= 1f)
	                {
	                    AngryTaskFailed?.Invoke();
	                    _isTrackingAngryValue = false;
	                    MetricSaver.TrackAngryBarFailed();
	                }
	            }
	        }
	    }

	    public void Initialize(ILevelData currentLevelData, UnitMover unitMover,
	                           IUpgradesData upgradesData, IPlayerData playerData)
	    {
	        _currentLevelData = currentLevelData;
	        _unitMover = unitMover;
	        _angryTracker = new AngryTracker(currentLevelData, _levelProgressTracker, upgradesData, _levelChangeEventTracker);
	        _goldCalculator = new GoldCalculator(playerData, upgradesData, currentLevelData);
	        _scoreCalculator = new LevelScoreCalculator(currentLevelData);

	        enabled = true;
	    }

	    public void PauseTracking()
	    {
	        _isTrackingLevel = false;
	    }

	    public void ContinueTracking()
	    {
	        _isTrackingLevel = true;
	    }

	    private void OnLevelChanged(ILevelData levelData)
	    {
	        if (levelData.LevelId == _levelSettings.MainMenuSettings.Id)
	        {
	            StopTracking(levelData);
	        }
	        else
	        {
	            StartTracking(levelData);
	        }
	    }

	    private void StartTracking(ILevelData levelData)
	    {
	        StopTracking(levelData);

	        foreach (Island island in levelData.Islands)
	        {
	            island.IslandFinished += OnIslandFinished;
	        }

	        _levelMoves = 0;
	        _levelTime = 0f;
	        ReachedGold = 0;
	        ReachedScore = 0;
	        _angryTracker.ResetAngryValue();
	        _isTrackingLevel = true;
	        _isTrackingAngryValue = false;
	        _firstMoveDone = false;
	        AngryChanged?.Invoke(_angryTracker.AngryValue);
	    }

	    private void StopTracking(ILevelData levelData)
	    {
	        if (levelData.Islands != null)
	        {
	            foreach (Island island in levelData.Islands)
	            {
	                island.IslandFinished -= OnIslandFinished;
	            }
	        }

	        _isTrackingLevel = false;
	        TrackStopped?.Invoke();
	    }

	    private void OnUnitsMoved(UnitsMoveInfo unitsMoveInfo)
	    {
	        if (_isTrackingLevel == false)
	        {
	            return;
	        }

	        _levelMoves++;

	        if (_firstMoveDone == false)
	        {
	            _firstMoveDone = true;
	            FirstMoveDone?.Invoke();
	            _isTrackingAngryValue = true;
	        }

	        if (_isTrackingAngryValue)
	        {
	            _angryTracker.AddUnitsMovedTick(unitsMoveInfo);
	            AngryChanged?.Invoke(_angryTracker.AngryValue);
	        }
	    }

	    private void OnIslandFinished(Island finishedIsland)
	    {
	        IslandFinished?.Invoke(finishedIsland);

	        if (_isTrackingLevel && _isTrackingAngryValue)
	        {
	            _angryTracker.AddIslandFinishedTick(finishedIsland);
	            AngryChanged?.Invoke(_angryTracker.AngryValue);
	        }

	        foreach (Island island in _currentLevelData.Islands)
	        {
	            if (island.IsDone == false)
	            {
	                return;
	            }
	        }

	        OnLevelFinished();
	    }

	    private void OnLevelFinished()
	    {
	        StopTracking(_currentLevelData);
	        ReachedGold = _goldCalculator.CalculateLevelGold(IsAngryTaskDone, IsMoveTaskDone);
	        ReachedScore = _scoreCalculator.CalculateScore(_levelTime, _levelMoves);
	        LevelFinished?.Invoke(_currentLevelData);

	        MetricSaver.FinishLevel();
	    }
	}
}
