using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Extensions.DeviceStyle
{
	public class ExitButtonStyleChanger : MonoBehaviour, IDeviceStyleChanger
	{
		private const float DesctopScale = 1f;
		private const float MobileScale = 1.2f;
		private readonly Vector2 _desctopAnchor = new Vector2(0.95f, 0.82f);
		private readonly Vector2 _mobileAnchor = new Vector2(0.05f, -0.02f);

		[SerializeField] private RectTransform _rectTransform;
	    [SerializeField] private Image _image;
	    [SerializeField] private Sprite _desktopSprite;
	    [SerializeField] private Sprite _mobileSprite;

	    public void SetStyle(Integration.DeviceInfo.DeviceType deviceType)
	    {
	        if (deviceType == Integration.DeviceInfo.DeviceType.Desktop)
	        {
	            _image.sprite = _desktopSprite;
	            SetButtonRectInfo(_desctopAnchor, DesctopScale);
	        }
	        else
	        {
	            _image.sprite = _mobileSprite;
	            SetButtonRectInfo(_mobileAnchor, MobileScale);
	        }
	    }

	    private void SetButtonRectInfo(Vector2 anchor, float size)
	    {
	        _rectTransform.anchorMin = anchor;
	        _rectTransform.anchorMax = anchor;
	        _rectTransform.localScale = Vector3.one * size;
	    }
	}
}
