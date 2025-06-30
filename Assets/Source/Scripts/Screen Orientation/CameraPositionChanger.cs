using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraPositionChanger : MonoBehaviour
{
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
    [SerializeField] private Transform _cameraFollowTarget;
    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private Vector3 _defaultVerticalCameraPosition = new Vector3(0, 12.2f, -11.2f);
    [SerializeField] private Vector3 _defaultHorizontalCameraPosition = new Vector3(0, 9f, -7.5f);
    [SerializeField] private float _defaultCameraFoV = 50;

    private bool _isVertical = true;
    private float _fovChangeDuration = 1f;

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

        _cameraFollowTarget.position = defaultPosition + offset;
        float fieldOfView = _defaultCameraFoV + _levelLoader.CurrentLevelData.CameraFoVOffset;

        DOTween.To(
            () => _camera.m_Lens.FieldOfView,
            value => _camera.m_Lens.FieldOfView = value,
            fieldOfView,
            _fovChangeDuration
        ).SetEase(Ease.InOutSine);
    }
}
