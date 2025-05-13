using System;
using Lean.Localization;
using UnityEngine;

public class InGameSettingsWindow : MenuWindow
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private LeanToken _movesToken;
    [SerializeField] private LeanToken _minutesToken;

    private new void OnEnable()
    {
        base.OnEnable();
        _levelLoader.LevelChanged += OnLevelChanged;

    }

    private new void OnDisable()
    {
        base.OnDisable();
        _levelLoader.LevelChanged -= OnLevelChanged;
    }

    private void OnLevelChanged()
    {
        _movesToken.SetValue(_levelLoader.CurrentLevelData.ExtraStarMoveLimit);

        TimeSpan time = TimeSpan.FromSeconds(_levelLoader.CurrentLevelData.ExtraStarTimeLimit);
        string timeString = $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
        _minutesToken.SetValue(timeString);
    }

    public override void Open()
    {
        if (IsOpened)
        {
            return;
        }

        Time.timeScale = 0f;
        base.Open();
    }

    public override void Close()
    {
        if (IsOpened == false)
        {
            return;
        }

        Time.timeScale = 1f;
        base.Close();
    }
}
