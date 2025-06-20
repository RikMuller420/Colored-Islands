using System;
using UnityEngine;
using UnityEngine.UI;

public class FaceSelectButton : SelectButton
{
    [SerializeField] private Image _faceImage;

    public int FaceId { get; private set; }

    public event Action<FaceSelectButton> ButtonClicked;


    public void Initialize(int faceId, Sprite faceSprite, bool isAviable)
    {
        _faceImage.sprite = faceSprite;
        FaceId = faceId;

        if (isAviable)
        {
            SetUnlockedStyle();
        }
        else
        {
            SetLockedStyle();
        }
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
