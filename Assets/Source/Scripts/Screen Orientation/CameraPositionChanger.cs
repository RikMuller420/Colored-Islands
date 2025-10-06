using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraPositionChanger : MonoBehaviour
{
    [SerializeField] private Transform _cameraFollowTarget;
    [SerializeField] private Transform _cameraLookAtTarget;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private CameraTargets _menuVerticalTargets;
    [SerializeField] private CameraTargets _menuHorizontalTargets;

    private float _minVerticalAspectRatio = 0.8f;
    private float _maxHorizontalAspectRatio = 1.3f;

    private LevelLoader _levelLoader;
    private LevelObjectsHolder _levelObjectsHolder;

    private CameraFoVChanger _cameraFoVChanger;
    private ScreenSizeChangeTracker _screenSizeChangeTracker;
    private UIOrientationChanger _uIOrientationChanger;
    private float _refreshRate = 0.1f;
    private float _foVUpdateDelay = 0.2f;
    private Coroutine _updateFoVInDelayCorutine;

    private WaitForSeconds _waitUpdatePosition;
    private WaitForSeconds _waitFoVUpdate;

    private float _cameraSqrTreshold = 0.05f;

    private void OnEnable()
    {
        _levelLoader.LevelChanged += UpdateCameraPosition;
        _screenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;
        UpdateCameraPosition();
    }

    private void OnDisable()
    {
        _levelLoader.LevelChanged -= UpdateCameraPosition;
        _screenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged;
    }

    public void Initialize(LevelLoader levelLoader, LevelObjectsHolder levelObjectsHolder,
                        BuferIslandsHolder buferIslandsHolder, UIOrientationChanger uIOrientationChanger,
                        ScreenSizeChangeTracker screenSizeChangeTracker)
    {
        _levelLoader = levelLoader;
        _levelObjectsHolder = levelObjectsHolder;
        _screenSizeChangeTracker = screenSizeChangeTracker;
        _uIOrientationChanger = uIOrientationChanger;
        _cameraFoVChanger = new CameraFoVChanger(_levelObjectsHolder, buferIslandsHolder,
                                                uIOrientationChanger, _mainCamera, _virtualCamera);
        _waitUpdatePosition = new WaitForSeconds(_refreshRate);
        _waitFoVUpdate = new WaitForSeconds(_foVUpdateDelay);
        enabled = true;
    }

    private void OnScreenSizeChanged(Vector2 vector) => UpdateCameraPosition();

    private void UpdateCameraPosition()
    {
        if (_levelLoader.CurrentLevelData.Id <= 0)
        {
            SetMenuCameraPosition();

            return;
        }

        if (_levelObjectsHolder.VerticalCameraTargets == null || _levelObjectsHolder.HorizontalCameraTargets == null)
        {
            return;
        }

        float aspectRatio = (float)Screen.width / Screen.height;

        Vector3 lookAtPosition;
        Vector3 followTargetPosition;

        if (aspectRatio <= _minVerticalAspectRatio)
        {
            lookAtPosition = _levelObjectsHolder.VerticalCameraTargets.LookAtPoint.position;
            followTargetPosition = _levelObjectsHolder.VerticalCameraTargets.FollowPoint.position;
        }
        else if (aspectRatio >= _maxHorizontalAspectRatio)
        {
            lookAtPosition = _levelObjectsHolder.HorizontalCameraTargets.LookAtPoint.position;
            followTargetPosition = _levelObjectsHolder.HorizontalCameraTargets.FollowPoint.position;
        }
        else
        {
            float scale = (aspectRatio - _minVerticalAspectRatio) / (_maxHorizontalAspectRatio - _minVerticalAspectRatio);

            lookAtPosition = Vector3.Lerp
            (
                _levelObjectsHolder.VerticalCameraTargets.LookAtPoint.position,
                _levelObjectsHolder.HorizontalCameraTargets.LookAtPoint.position,
                scale
            );

            followTargetPosition = Vector3.Lerp
            (
                _levelObjectsHolder.VerticalCameraTargets.FollowPoint.position,
                _levelObjectsHolder.HorizontalCameraTargets.FollowPoint.position,
                scale
            );
        }

        _cameraLookAtTarget.position = lookAtPosition;
        _cameraFollowTarget.position = followTargetPosition;

        TryStopUpdateFoVCoroutine();
        _updateFoVInDelayCorutine = StartCoroutine(AdjustFoVInDelay());
    }

    private void TryStopUpdateFoVCoroutine()
    {
        if (_updateFoVInDelayCorutine != null)
        {
            StopCoroutine(_updateFoVInDelayCorutine);
            _updateFoVInDelayCorutine = null;
        }
    }

    private void SetMenuCameraPosition()
    {
        TryStopUpdateFoVCoroutine();

        _cameraLookAtTarget.position = _uIOrientationChanger.IsVertical ?
                                            _menuVerticalTargets.LookAtPoint.position :
                                            _menuHorizontalTargets.LookAtPoint.position;

        _cameraFollowTarget.position = _uIOrientationChanger.IsVertical ?
                                            _menuVerticalTargets.FollowPoint.position :
                                            _menuHorizontalTargets.FollowPoint.position;

        _cameraFoVChanger.SetMenuCameraFoV();
    }

    private IEnumerator AdjustFoVInDelay()
    {
        while (enabled)
        {
            float cameraSqrOffset = (_cameraFollowTarget.position - _mainCamera.transform.position).sqrMagnitude;

            if (cameraSqrOffset < _cameraSqrTreshold)
            {
                break;
            }

            yield return _waitUpdatePosition;
        }

        yield return _waitFoVUpdate;

        _cameraFoVChanger.AdjustFOVToFitObjects();
    }
}
