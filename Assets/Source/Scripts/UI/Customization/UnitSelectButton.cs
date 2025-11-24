using System;
using System.Linq;
using Lean.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class UnitSelectButton : MonoBehaviour
{
    [SerializeField] private Paint _paint;
    [SerializeField] private Button _button;
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private GameObject _frame;
    [SerializeField] private PaintMaterials _paintMaterials;

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

    public void SetColor(ColorSample colorSample)
    {
        PaintMaterialData material = _paintMaterials.Materials.FirstOrDefault(material => material.ColorSample == colorSample);

        ColorBlock colorBlock = _button.colors;
        colorBlock.normalColor = material.UnitUiColor;
        colorBlock.highlightedColor = material.UnitUiHatColor;
        colorBlock.pressedColor = material.UnitUiHatColor;
        colorBlock.selectedColor = material.UnitUiColor;
        colorBlock.disabledColor = material.UnitUiColor;

        _button.colors = colorBlock;
        _text.text = LeanLocalization.GetTranslationText(material.LocalizationKey);
    }

    public void SetSelectdStyle()
    {
        _button.interactable = false;
        _frame.SetActive(true);
        _background.color = _selectedStyleColor;
        _text.fontStyle = FontStyles.Bold;
    }

    public void SetNonSelectedStyle()
    {
        _button.interactable = true;
        _frame.SetActive(false);
        _background.color = _nonSelectedStyleColor;
        _text.fontStyle = FontStyles.Normal;
    }

    private void OnButtonPressed()
    {
        ButtonClicked?.Invoke(this);
    }
}
