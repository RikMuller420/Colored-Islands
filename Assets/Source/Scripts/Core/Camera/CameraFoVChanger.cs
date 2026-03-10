using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.OrientationChanger;
using UnityEngine;

namespace SlimeGround.Core.CameraSystem
{
	public class CameraFoVChanger
	{
	    private Camera _mainCamera;
	    private CinemachineVirtualCamera _virtualCamera;

	    private float _menuVerticalCameraFoV = 60f;
	    private float _menuHorizontalCameraFoV = 40f;

	    private float _minFOV = 20f;
	    private float _maxFOV = 100f;
	    private float _fovChangeDuration = 1.2f;

	    private float _verticalOrientationPadding = 0.2f;
	    private float _horizontalOrientationPadding = 0;

	    private float _minCornerDistance = 0.01f;
	    private ILevelData _currentLevelData;
	    private UIOrientationChanger _uIOrientationChanger;
	    private List<MeshRenderer> _objectsToFitInCamera = new List<MeshRenderer>();

	    public CameraFoVChanger(ILevelData currentLevelData, UIOrientationChanger uIOrientationChanger,
								Camera mainCamera, CinemachineVirtualCamera virtualCamera)
	    {
	        _currentLevelData = currentLevelData;
	        _uIOrientationChanger = uIOrientationChanger;
	        _mainCamera = mainCamera;
	        _virtualCamera = virtualCamera;
	    }

	    public void SetMenuCameraFoV()
	    {
	        float targetFoV = _uIOrientationChanger.IsVertical ? _menuVerticalCameraFoV : _menuHorizontalCameraFoV;

	        DOTween.To(
	            () => _virtualCamera.m_Lens.FieldOfView,
	            value => _virtualCamera.m_Lens.FieldOfView = value,
	            targetFoV,
	            _fovChangeDuration
	        ).SetEase(Ease.InOutSine);
	    }

	    public void AdjustFOVToFitObjects()
	    {
	        UpdateObjectsList();

	        if (_objectsToFitInCamera.Count == 0)
	        {
	            return;
	        }

	        Bounds combinedBounds = GetCombinedBounds(_objectsToFitInCamera);
	        float requiredFOV = CalculateRequiredFOV(combinedBounds);
	        float fieldOfView = Mathf.Clamp(requiredFOV, _minFOV, _maxFOV);

	        DOTween.To
			(
	            () => _virtualCamera.m_Lens.FieldOfView,
	            value => _virtualCamera.m_Lens.FieldOfView = value,
	            fieldOfView,
	            _fovChangeDuration
	        )
			.SetEase(Ease.InOutSine);
	    }

	    private void UpdateObjectsList()
	    {
	        _objectsToFitInCamera.Clear();

	        if (_currentLevelData.LevelBounds != null)
	        {
	            _objectsToFitInCamera.Add(_currentLevelData.LevelBounds);        
	        }
	    }

	    private Bounds GetCombinedBounds(List<MeshRenderer> objects)
	    {
	        Bounds bounds = new Bounds();
	        bool first = true;

	        foreach (MeshRenderer renderer in objects)
	        {
	            if (renderer != null)
	            {
	                if (first)
	                {
	                    bounds = renderer.bounds;
	                    first = false;
	                }
	                else
	                {
	                    bounds.Encapsulate(renderer.bounds);
	                }
	            }
	        }

	        return bounds;
	    }

	    private float CalculateRequiredFOV(Bounds bounds)
	    {
	        Vector3 cameraPos = _mainCamera.transform.position;
	        float aspect = _mainCamera.aspect;

	        Vector3[] corners = GetBoundsCorners(bounds);
	        float maxVerticalAngle = 0f;
	        float maxHorizontalAngle = 0f;

	        foreach (Vector3 corner in corners)
	        {
	            Vector3 toCorner = corner - cameraPos;
	            float distance = toCorner.magnitude;

	            if (distance < _minCornerDistance)
	            {
	                continue;
	            }

	            Vector3 forward = _mainCamera.transform.forward;
	            Vector3 right = _mainCamera.transform.right;
	            Vector3 up = _mainCamera.transform.up;

	            Vector3 toCornerNormalized = toCorner.normalized;
	            Vector3 horizontalProjection = Vector3.ProjectOnPlane(toCornerNormalized, up);
	            Vector3 verticalProjection = Vector3.ProjectOnPlane(toCornerNormalized, right);

	            float horizontalAngle = Mathf.Acos(Vector3.Dot(horizontalProjection.normalized, forward)) * Mathf.Rad2Deg;
	            float verticalAngle = Mathf.Acos(Vector3.Dot(verticalProjection.normalized, forward)) * Mathf.Rad2Deg;

	            float verticalFOVHalf = verticalAngle;
	            float verticalPadding = _uIOrientationChanger.IsVertical ? _verticalOrientationPadding : _horizontalOrientationPadding;
	            float verticalPaddingFactor = 1f - verticalPadding;

	            if (verticalPaddingFactor > 0)
	            {
	                verticalAngle /= verticalPaddingFactor;
	            }

	            maxHorizontalAngle = Mathf.Max(maxHorizontalAngle, horizontalAngle);
	            maxVerticalAngle = Mathf.Max(maxVerticalAngle, verticalAngle);
	        }

	        float verticalFOV = maxVerticalAngle * 2f;
	        float horizontalFOV = maxHorizontalAngle * 2f;

	        float verticalFOVFromHorizontal = 2f * Mathf.Atan(Mathf.Tan(horizontalFOV * Mathf.Deg2Rad / 2f) / aspect) * Mathf.Rad2Deg;

	        return Mathf.Max(verticalFOV, verticalFOVFromHorizontal);
	    }

	    private Vector3[] GetBoundsCorners(Bounds bounds)
	    {
	        Vector3[] corners = new Vector3[8];
	        corners[0] = bounds.min;
	        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
	        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
	        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
	        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
	        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
	        corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
	        corners[7] = bounds.max;

	        return corners;
	    }
	}
}
