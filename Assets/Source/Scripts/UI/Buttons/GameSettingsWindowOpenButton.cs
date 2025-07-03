using UnityEngine;
using UnityEngine.UI;

public class GameSettingsWindowOpenButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private InGameSettingsWindow _window;

    private void OnEnable()
    {
        _button.onClick.AddListener(Open);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Open);
    }

    protected virtual void Open()
    {
        _window.Open();
    }
}
