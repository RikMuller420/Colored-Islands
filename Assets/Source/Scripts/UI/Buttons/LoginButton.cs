using UnityEngine;
using UnityEngine.UI;
using YG;

public class LoginButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private YandexGame _yandexGame;

    private void Awake()
    {
        UpdateButtonActivity();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(AskForLogin);
        _yandexGame.ResolvedAuthorization.AddListener(UpdateButtonActivity);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(AskForLogin);
        _yandexGame.ResolvedAuthorization.RemoveListener(UpdateButtonActivity);
    }

    private void AskForLogin()
    {
        _yandexGame._OpenAuthDialog();
    }

    private void UpdateButtonActivity()
    {
        gameObject.SetActive(YandexGame.auth == false);
    }
}
