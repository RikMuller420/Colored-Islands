using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private Button _button;

    private IPlayerData _playerData;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadLastAvailableLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadLastAvailableLevel);
    }

    public void Initialize(IPlayerData playerData)
    {
        _playerData = playerData;
        enabled = true;
    }

    private void LoadLastAvailableLevel()
    {
        _levelLoader.LoadLevel(_playerData.LastAvailableLevelId);
    }
}
