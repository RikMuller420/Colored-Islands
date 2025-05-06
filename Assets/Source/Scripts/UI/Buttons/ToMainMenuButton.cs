using UnityEngine;
using UnityEngine.UI;

public class ToMainMenuButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private ConfirmationMenuWindow _confirmationWindow;
    [SerializeField] private LevelLoader _levelLoader;

    private string _confirmationMessage = "Are you sure you wand to go to main menu?";

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
        _confirmationWindow.Open(_confirmationMessage, _levelLoader.LoadMainMenu);
    }
}
