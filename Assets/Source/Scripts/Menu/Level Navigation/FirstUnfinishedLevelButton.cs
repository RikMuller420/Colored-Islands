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
        _button.onClick.AddListener(LoadLastAvailableLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadLastAvailableLevel);
    }

    public void Initialize(GameProgressStorage gameProgressStorage, LevelLoader levelLoader)
    {
        _progressStorage = gameProgressStorage;
        _levelLoader = levelLoader;
        enabled = true;
    }

    private void LoadLastAvailableLevel()
    {
        _levelLoader.LoadLevel(_progressStorage.LastAvailableLevelId);
    }
}
