using UnityEngine;

public class UIOrientationChanger : MonoBehaviour
{
    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
    [SerializeField] private GameObject _boostsZoneVertical;
    [SerializeField] private GameObject _boostsZoneHorizontal;

    private bool _isVertical = true;

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
        _boostsZoneVertical.SetActive(_isVertical);
        _boostsZoneHorizontal.SetActive(_isVertical == false);
    }
}
