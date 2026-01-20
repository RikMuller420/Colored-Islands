using TMPro;
using UnityEngine;

public class AviableSpinCountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private IPlayerData _playerData;

    public void Initialize(IPlayerData playerData)
    {
        _playerData = playerData;
        _playerData.SpinCountChanged += UpdateViewText;
        UpdateViewText();
    }

    private void UpdateViewText()
    {
        _text.text = _playerData.AviableSpinCount.ToString();
    }
}
