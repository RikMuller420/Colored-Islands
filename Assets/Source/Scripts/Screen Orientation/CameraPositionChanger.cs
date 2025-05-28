using UnityEngine;

public class CameraPositionChanger : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
    [SerializeField] private Camera _camera;

    [SerializeField] private Vector3 _defaultVerticalCameraPosition = new Vector3(0, 12.2f, -11.2f);
    [SerializeField] private Vector3 _defaultHorizontalCameraPosition = new Vector3(0, 23f, -9.3f);
    [SerializeField] private float _defaultCameraFoV = 50;

    private bool _isVertical = true;

    private void OnEnable()
    {
        _screenSizeChangeTracker.ScreenSizeChanged += TryUpdateCameraPosition;
        _levelLoader.LevelChanged += UpdateCamraPosition;
    }

    private void OnDisable()
    {
        _screenSizeChangeTracker.ScreenSizeChanged -= TryUpdateCameraPosition;
        _levelLoader.LevelChanged -= UpdateCamraPosition;
    }

    private void TryUpdateCameraPosition(Vector2 screenSize)
    {
        bool isNewOrientationVertical = screenSize.y > screenSize.x;

        if (isNewOrientationVertical != _isVertical)
        {
            _isVertical = isNewOrientationVertical;
            UpdateCamraPosition();
        }
    }

    private void UpdateCamraPosition()
    {
        Vector3 defaultPosition = _isVertical ? _defaultVerticalCameraPosition : _defaultHorizontalCameraPosition;
        Vector3 offset = _isVertical ? _levelLoader.CurrentLevelData.CameraVerticalOrientationOffset :
                                        _levelLoader.CurrentLevelData.CameraHorizontalOrientationOffset;

        _camera.transform.position = defaultPosition + offset;
        _camera.fieldOfView = _defaultCameraFoV + _levelLoader.CurrentLevelData.CameraFoVOffset;
    }
}
