using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectButton : MonoBehaviour
{
    [SerializeField] private Paint _paint;
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _text;

    private Color _selectedStyleColor = Color.white;
    private Color _nonSelectedStyleColor = new Color(0.62f, 0.62f, 0.62f, 1f);

    public event Action<UnitSelectButton> ButtonClicked;

    public Paint Paint => _paint;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonPressed);
    }

    private void OnButtonPressed()
    {
        ButtonClicked?.Invoke(this);
    }

    public void SetSelectdStyle()
    {
        _button.interactable = false;
        _background.color = _selectedStyleColor;
        _text.fontStyle = FontStyles.Bold;
    }

    public void SetNonSelectedStyle()
    {
        _button.interactable = true;
        _background.color = _nonSelectedStyleColor;
        _text.fontStyle = FontStyles.Normal;
    }
}
