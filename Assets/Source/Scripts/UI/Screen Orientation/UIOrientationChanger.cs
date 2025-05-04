using UnityEngine;

public class UIOrientationChanger : MonoBehaviour
{
    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
    [SerializeField] private RectTransform _gameZone;
    [SerializeField] private RectTransform _bannerZone;
    [SerializeField] private GameObject _boostsZoneVertical;
    [SerializeField] private GameObject _boostsZoneHorizontal;

    private bool _isVertical = true;

    private float _banerAnchorsSize = 0.1f;

    private void OnEnable()
    {
        _screenSizeChangeTracker.ScreenSizeChanged += TryUpdateOrientation;
    }

    private void OnDisable()
    {
        _screenSizeChangeTracker.ScreenSizeChanged -= TryUpdateOrientation;
    }

    private void TryUpdateOrientation(Vector2 screenSize)
    {
        bool isNewOrientationVertical = screenSize.y > screenSize.x;

        if (isNewOrientationVertical != _isVertical)
        {
            _isVertical = isNewOrientationVertical;
            UpdateOrientation();
        }
    }

    private void UpdateOrientation()
    {
        if (_isVertical)
        {
            _bannerZone.anchorMin = Vector2.zero;
            _bannerZone.anchorMax = new Vector2(1, _banerAnchorsSize);

            _gameZone.anchorMin = new Vector2(0, _banerAnchorsSize);
            _gameZone.anchorMax = Vector2.one;
        }
        else
        {
            _bannerZone.anchorMin = new Vector2(1f - _banerAnchorsSize, 0);
            _bannerZone.anchorMax = Vector2.one;

            _gameZone.anchorMin = Vector2.zero;
            _gameZone.anchorMax = new Vector2(1f - _banerAnchorsSize, 1);
        }

        _boostsZoneVertical.SetActive(_isVertical);
        _boostsZoneHorizontal.SetActive(_isVertical == false);
    }
}
