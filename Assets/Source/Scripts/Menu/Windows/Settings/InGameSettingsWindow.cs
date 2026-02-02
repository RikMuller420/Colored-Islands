using System;
using Lean.Localization;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Settings
{
	public class InGameSettingsWindow : MenuWindow
	{
	    [SerializeField] private LevelSettings _levelSettings;
	    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
	    [SerializeField] private LeanToken _movesToken;
	    [SerializeField] private LeanToken _minutesToken;

	    private new void OnEnable()
	    {
	        base.OnEnable();
	        _levelChangeEventTracker.LevelChanged += OnLevelChanged;
	    }

	    private new void OnDisable()
	    {
	        base.OnDisable();
	        _levelChangeEventTracker.LevelChanged -= OnLevelChanged;
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

		private void OnLevelChanged(ILevelData levelData)
	    {
	        if (levelData.LevelId == _levelSettings.MainMenuSettings.Id)
	        {
	            return;
	        }

	        _movesToken.SetValue(levelData.ExtraStarMoveCount);

	        TimeSpan time = TimeSpan.FromSeconds(levelData.ExtraScoreTime);
	        string timeString = $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
	        _minutesToken.SetValue(timeString);
	    }
	}
}
