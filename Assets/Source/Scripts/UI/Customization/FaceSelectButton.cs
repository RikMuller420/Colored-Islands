using System;
using UnityEngine;
using UnityEngine.UI;

public class FaceSelectButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _faceImage;
    [SerializeField] private GameObject _lockIcon;
    [SerializeField] private GameObject _selectedFrame;

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
        _button.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonPressed);
    }

    public void SetLockedStyle()
    {
        _button.interactable = false;
        _lockIcon.SetActive(true);
    }

    public void SetUnlockedStyle()
    {
        _button.interactable = true;
        _lockIcon.SetActive(false);
    }

    public void SetSelectedStyle()
    {
        _button.interactable = false;
        _selectedFrame.SetActive(true);
    }

    public void SetNonSelectedStyle()
    {
        _button.interactable = true;
        _selectedFrame.SetActive(false);
    }

    private void OnButtonPressed()
    {
        ButtonClicked?.Invoke(this);
    }
}
