using System;
using UnityEngine;

namespace SlimeGround.Gameplay.Levels
{
	public class LevelChangeEventTracker : MonoBehaviour
	{
	    [SerializeField] private LevelLoader _levelLoader;

	    public event Action LevelStartChanging;
	    public event Action<ILevelData> LevelChanged;

	    private void OnEnable()
	    {
	        _levelLoader.LevelChanged += OnLevelChanged;
	        _levelLoader.LevelStartChanging += OnLevelStartChanging;
	    }

	    private void OnDisable()
	    {
	        _levelLoader.LevelChanged -= OnLevelChanged;
	        _levelLoader.LevelStartChanging -= OnLevelStartChanging;
	    }

	    private void OnLevelStartChanging()
	    {
	        LevelStartChanging?.Invoke();
	    }

	    private void OnLevelChanged(ILevelData levelData)
	    {
	        LevelChanged.Invoke(levelData);
	    }
	}
}
