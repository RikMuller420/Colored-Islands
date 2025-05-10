using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private int _nextLevelId = 1;
    private LevelLoader _levelLoader;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadNextLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadNextLevel);
    }

    public void Initialize(LevelLoader levelLoader)
    {
        _levelLoader = levelLoader;
        enabled = true;
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
