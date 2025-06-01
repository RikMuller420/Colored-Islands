using System;
using UnityEngine;

public class AngryBar : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private LevelProgressTracker _levelProgressTracker;
    private LevelLoader _levelLoader;

    public event Action Changed;

    public float Value { get; private set; }
    public float MaxValue { get; } = 1f;

    public void Initialize(LevelProgressTracker levelProgressTracker, LevelLoader levelLoader)
    {
        _levelProgressTracker = levelProgressTracker;
        _levelLoader = levelLoader;

        _levelProgressTracker.AngryChanged += OnAngyValueChanged;
        _levelProgressTracker.LevelFinished += OnLevelEnded;
        _levelProgressTracker.LevelFailed += OnLevelEnded;
        _levelLoader.LevelChanged += OnLevelStarted;

        enabled = true;
    }

    private void OnAngyValueChanged(float value)
    {
        Value = value;
        Changed?.Invoke();
    }

    private void OnLevelEnded()
    {
        _canvas.overrideSorting = true;
    }

    private void OnLevelStarted()
    {
        _canvas.overrideSorting = false;
    }
}
