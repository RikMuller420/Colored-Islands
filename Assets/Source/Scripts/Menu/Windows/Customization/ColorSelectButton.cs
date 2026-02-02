using System;
using SlimeGround.Data;
using SlimeGround.Data.ScriptableObjects.Paints;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Customization
{
	public class ColorSelectButton : SelectButton
	{
	    [SerializeField] private Image _faceImage;

	    public event Action<ColorSelectButton> ButtonClicked;
	    public ColorSample ColorSample { get; private set; }

	    public void Initialize(ColorSampleMaterialData material)
	    {
	        ColorSample = material.ColorSample;
	        ColorBlock colorBlock = Button.colors;
	        colorBlock.normalColor = material.UnitUiColor;
	        colorBlock.highlightedColor = material.UnitUiColor;
	        colorBlock.pressedColor = material.UnitUiHatColor;
	        colorBlock.selectedColor = material.UnitUiHatColor;
	        colorBlock.disabledColor = material.UnitUiHatColor;

	        Button.colors = colorBlock;

	        Initialize(true, true);
	    }

	    public void SetFaceImage(Sprite sprite)
	    {
	        _faceImage.sprite = sprite;
	    }

	    private void OnEnable()
	    {
	        Button.onClick.AddListener(OnButtonPressed);
	    }

	    private void OnDisable()
	    {
	        Button.onClick.RemoveListener(OnButtonPressed);
	    }

	    private void OnButtonPressed()
	    {
	        ButtonClicked?.Invoke(this);
	    }
	}
}
