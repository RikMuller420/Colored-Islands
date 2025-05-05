using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private LevelRecords _levelRecords;
    [SerializeField] private LevelLoader _levelLoader;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadNextLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadNextLevel);
    }

    private void LoadNextLevel()
    {
        int nextLevelId = _levelRecords.LastUnfinishedLevel;
        _levelLoader.LoadLevel(nextLevelId);
    }
}
