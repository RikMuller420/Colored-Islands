using System.Collections;
using Cinemachine;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.OrientationChanger;
using UnityEngine;

namespace SlimeGround.Core.CameraSystem
{
	public class CameraPositionChanger : MonoBehaviour
	{
		private const float MinVerticalAspectRatio = 0.8f;
		private const float MaxHorizontalAspectRatio = 1.3f;
		private const float RefreshRate = 0.1f;
		private const float FoVUpdateDelay = 0.2f;
		private const float CameraSqrTreshold = 3f;

		[SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
	    [SerializeField] private BuferIslands _buferIslands;
	    [SerializeField] private UIOrientationChanger _uIOrientationChanger;
	    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;

	    [SerializeField] private Transform _cameraFollowTarget;
	    [SerializeField] private Transform _cameraLookAtTarget;
	    [SerializeField] private Camera _mainCamera;
	    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
	    [SerializeField] private CameraTargets _menuVerticalTargets;
	    [SerializeField] private CameraTargets _menuHorizontalTargets;

		private ILevelData _currentLevelData;
	    private CameraFoVChanger _cameraFoVChanger;

	    private Coroutine _updateFoVInDelayCorutine;
	    private WaitForSeconds _waitUpdatePosition;
	    private WaitForSeconds _waitFoVUpdate;


	    private void OnEnable()
	    {
	        _levelChangeEventTracker.LevelChanged += UpdateCameraPosition;
	        _screenSizeChangeTracker.ScreenSizeChanged += OnScreenSizeChanged;
	    }

	    private void OnDisable()
	    {
	        _levelChangeEventTracker.LevelChanged -= UpdateCameraPosition;
	        _screenSizeChangeTracker.ScreenSizeChanged -= OnScreenSizeChanged;
	    }

	    public void Initialize(ILevelData currentLevelData)
	    {
	        _currentLevelData = currentLevelData;
	        _cameraFoVChanger = new CameraFoVChanger(_currentLevelData, _uIOrientationChanger, 
													 _mainCamera, _virtualCamera);
	        _waitUpdatePosition = new WaitForSeconds(RefreshRate);
	        _waitFoVUpdate = new WaitForSeconds(FoVUpdateDelay);
	        UpdateCameraPosition();

	        enabled = true;
	    }

	    private void OnScreenSizeChanged(Vector2 vector) => UpdateCameraPosition();

	    private void UpdateCameraPosition() => UpdateCameraPosition(_currentLevelData);

	    private void UpdateCameraPosition(ILevelData levelData)
	    {
	        if (levelData.IsMenuLevel)
	        {
	            SetMenuCameraPosition();

	            return;
	        }

	        if (levelData.VerticalCameraTargets == null || levelData.HorizontalCameraTargets == null)
	        {
	            return;
	        }

	        float aspectRatio = (float)Screen.width / Screen.height;

	        Vector3 lookAtPosition;
	        Vector3 followTargetPosition;

	        if (aspectRatio <= MinVerticalAspectRatio)
	        {
	            lookAtPosition = levelData.VerticalCameraTargets.LookAtPoint.position;
	            followTargetPosition = levelData.VerticalCameraTargets.FollowPoint.position;
	        }
	        else if (aspectRatio >= MaxHorizontalAspectRatio)
	        {
	            lookAtPosition = levelData.HorizontalCameraTargets.LookAtPoint.position;
	            followTargetPosition = levelData.HorizontalCameraTargets.FollowPoint.position;
	        }
	        else
	        {
	            float scale = (aspectRatio - MinVerticalAspectRatio) / (MaxHorizontalAspectRatio - MinVerticalAspectRatio);

	            lookAtPosition = Vector3.Lerp
	            (
	                levelData.VerticalCameraTargets.LookAtPoint.position,
	                levelData.HorizontalCameraTargets.LookAtPoint.position,
	                scale
	            );

	            followTargetPosition = Vector3.Lerp
	            (
	                levelData.VerticalCameraTargets.FollowPoint.position,
	                levelData.HorizontalCameraTargets.FollowPoint.position,
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

	            if (cameraSqrOffset < CameraSqrTreshold)
	            {
	                break;
	            }

	            yield return _waitUpdatePosition;
	        }

	        yield return _waitFoVUpdate;

	        _cameraFoVChanger.AdjustFOVToFitObjects();
	    }
	}
}
