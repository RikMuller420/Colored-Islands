using UnityEngine;
using UnityEngine.UI;

public class ExitButtonStyleChanger : MonoBehaviour, IDeviceStyleChanger
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _desktopSprite;
    [SerializeField] private Sprite _mobileSprite;

    private Vector2 _desctopAnchor = new Vector2(0.95f, 0.82f);
    private Vector2 _mobileAnchor = new Vector2(0.05f, -0.02f);
    private float _desctopScale = 1f;
    private float _mobileScale = 1.2f;

    public void SetStyle(DeviceType deviceType)
    {
        if (deviceType == DeviceType.Desktop)
        {
            _image.sprite = _desktopSprite;
            SetButtonRectInfo(_desctopAnchor, _desctopScale);
        }
        else
        {
            _image.sprite = _mobileSprite;
            SetButtonRectInfo(_mobileAnchor, _mobileScale);
        }
    }

    private void SetButtonRectInfo(Vector2 anchor, float size)
    {
        _rectTransform.anchorMin = anchor;
        _rectTransform.anchorMax = anchor;
        _rectTransform.localScale = Vector3.one * size;
    }
}
