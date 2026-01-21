using SlimeGround.Gameplay.Levels;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.LevelNavigation
{
	public class NextLevelButton : MonoBehaviour
	{
	    [SerializeField] private LevelLoader _levelLoader;
	    [SerializeField] private Button _button;

	    private int _nextLevelId = 1;

	    private void OnEnable()
	    {
	        _button.onClick.AddListener(LoadNextLevel);
	    }

	    private void OnDisable()
	    {
	        _button.onClick.RemoveListener(LoadNextLevel);
	    }

	    public void SetNextLevelId(int id)
	    {
	        _nextLevelId = id;
	    }

	    private void LoadNextLevel()
	    {
	        _levelLoader.LoadLevel(_nextLevelId);
	    }
	}
}
