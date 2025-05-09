using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private LevelLoader _levelLoader;

    private GameProgressStorage _gameProgressStorage;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadNextLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadNextLevel);
    }

    public void Initialize(GameProgressStorage gameProgressStorage)
    {
        _gameProgressStorage = gameProgressStorage;
    }

    private void LoadNextLevel()
    {
        int nextLevelId = _gameProgressStorage.FirstUnfinishedLevel.Id;
        _levelLoader.LoadLevel(nextLevelId);
    }
}
