using System.Collections.Generic;
using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.LevelNavigation;
using UnityEngine;

namespace SlimeGround.Menu.Windows.LevelMap
{
	public class LevelTabInitializer : MonoBehaviour
	{
	    [SerializeField] private int _startLevelIndex = 1;
	    [SerializeField] private List<LevelButton> _levelButtons = new();

	    public void InitializeButtons(IPlayerData playerData, LevelLoader levelLoader)
	    {
	        int levelIndex = _startLevelIndex;

	        foreach (LevelButton levelButton in _levelButtons)
	        {
	            levelButton.Initialize(levelIndex, playerData, levelLoader);
	            levelIndex++;
	        }
	    }
	}
}
