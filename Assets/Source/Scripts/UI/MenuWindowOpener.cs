using UnityEngine;
using UnityEngine.UI;

public class MenuWindowOpener : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private MenuWindow _window;

    private void OnEnable()
    {
        _button.onClick.AddListener(Open);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Open);
    }

    private void Open()
    {
        _window.Open();
    }
}
