using UnityEngine;
using UnityEngine.UI;

public class ButtonUISoundPlayer : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private UISoundPlayer _uiSoundPlayer;
    [SerializeField] private UiSoundType _uiSoundType;

    private void OnValidate()
    {
        if (_button == null)
        {
            _button = GetComponent<Button>();
        }

        if (_uiSoundPlayer == null)
        {
            _uiSoundPlayer = FindObjectOfType<UISoundPlayer>();
        }
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(PlaySound);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PlaySound);
    }

    private void PlaySound()
    {
        _uiSoundPlayer.PlaySound(_uiSoundType);
    }
}
