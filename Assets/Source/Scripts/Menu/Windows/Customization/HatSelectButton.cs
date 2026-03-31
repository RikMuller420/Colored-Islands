using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Customization
{
	public class HatSelectButton : SelectButton
	{
	    [SerializeField] private Image _hatImage;
	    [SerializeField] private TextMeshProUGUI _requredLevelText;

	    public event Action<HatSelectButton> ButtonClicked;

	    public int HatId { get; private set; }
	    public int RequredLevel { get; private set; }

	    public void Initialize(int hatId, Sprite hatSprite, int requredLevel, bool isAviable, bool wasUsed)
	    {
	        _hatImage.sprite = hatSprite;
	        HatId = hatId;
	        RequredLevel = requredLevel;
	        _requredLevelText.text = requredLevel.ToString();
	        Initialize(isAviable, wasUsed);
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