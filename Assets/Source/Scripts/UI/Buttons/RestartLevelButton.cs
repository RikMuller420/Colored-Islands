using UnityEngine;
using UnityEngine.UI;

public class RestartLevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private LevelLoader _levelLoader;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadMainMenu);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        _levelLoader.ReloadLastLevel();
    }
}
