using System;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Customization
{
	public class FaceSelectButton : SelectButton
	{
	    [SerializeField] private Image _faceImage;

	    public event Action<FaceSelectButton> ButtonClicked;

	    public int FaceId { get; private set; }

	    public void Initialize(int faceId, Sprite faceSprite, bool isAviable, bool wasUsed)
	    {
	        _faceImage.sprite = faceSprite;
	        FaceId = faceId;
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
