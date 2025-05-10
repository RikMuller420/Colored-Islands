using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FirstUnfinishedLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private GameProgressStorage _progressStorage;
    private LevelLoader _levelLoader;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadFirstUnfinishedLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadFirstUnfinishedLevel);
    }

    public void Initialize(GameProgressStorage gameProgressStorage, LevelLoader levelLoader)
    {
        _progressStorage = gameProgressStorage;
        _levelLoader = levelLoader;
        enabled = true;
    }

    private void LoadFirstUnfinishedLevel()
    {
        LevelProgress level = _progressStorage.FirstUnfinishedLevel;

        if (level == null)
        {
            level = _progressStorage.Levels.Last();
        }

        _levelLoader.LoadLevel(level.Id);
    }
}
