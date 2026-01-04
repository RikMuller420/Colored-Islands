using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

public class ToMainMenuButton : MonoBehaviour
{
    private const string SureQuestionStart = "Are you sure you want to";
    private const string SureQuestionEnd = "go to main menu?";

    [SerializeField] private Button _button;
    [SerializeField] private ConfirmationMenuWindow _confirmationWindow;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private bool _confirmationRequired = false;

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
        if (_confirmationRequired)
        {
            string confirmationMessage = LeanLocalization.GetTranslationText(SureQuestionStart) +
                                            LeanLocalization.GetTranslationText(SureQuestionEnd);
            _confirmationWindow.Open(confirmationMessage, _levelLoader.LoadMainMenu);
        }
        else
        {
            _levelLoader.LoadMainMenu();
        }
    }
}
