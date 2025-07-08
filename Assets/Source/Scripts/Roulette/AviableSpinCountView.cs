using TMPro;
using UnityEngine;

public class AviableSpinCountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private GameProgressStorage _progressStorage;

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;
        _progressStorage.SpinCountChanged += UpdateViewText;
        UpdateViewText();
    }

    private void UpdateViewText()
    {
        _text.text = _progressStorage.AviableSpinCount.ToString();
    }
}
