using DG.Tweening;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class UnitMoveTask
	{
	    private Tween _pathTween;
	    private Transform _unitsLookAtTarget;
		private IslandPoint _targetPoint;
		private Unit _unit;

		private bool _isMoveAnimationActive;
	    private float _deactivateMoveAnimationPercent = 0.5f;

	    private float _maxMoveSpeed = 10f;
	    private float _minMoveTime = 0.25f;
	    private float _minArcPosition = 0.3f;
	    private float _maxArcPosition = 0.7f;
	    private float _minArcOffset = 0f;
	    private float _maxArcOffset = 0.1f;

	    public UnitMoveTask(Unit unit, IslandPoint targetPoint, Transform unitsLookAtTarget)
	    {
	        _unit = unit;
	        _targetPoint = targetPoint;
	        _unitsLookAtTarget = unitsLookAtTarget;
	        Vector3 intermediatePoint = CalculateIntermediatePoint(CurrentPosition, TargetPosition);
	        Vector3[] path = { CurrentPosition, intermediatePoint, TargetPosition };

	        float moveTime = (CurrentPosition - TargetPosition).magnitude / _maxMoveSpeed;

	        if (moveTime < _minMoveTime)
	        {
	            moveTime = _minMoveTime;
	        }

	        unit.transform.DOKill();
	        _pathTween = unit.transform.DOPath(path, moveTime, PathType.CatmullRom)
	                      .SetEase(Ease.OutQuad)
	                      .OnUpdate(OnMoveUpdate)
	                      .OnComplete(OnMoveComplete);

	        unit.Animator.StartWalk();
	        _isMoveAnimationActive = true;
	    }

	    private Vector3 CurrentPosition => _unit.transform.position;
	    private Vector3 TargetPosition => _targetPoint.Transform.position;

	    private void OnMoveUpdate()
	    {
	        _unit.MeshTransform.LookAt(_unitsLookAtTarget);

	        if (_pathTween.ElapsedPercentage() >= _deactivateMoveAnimationPercent && _isMoveAnimationActive)
	        {
	            _unit.Animator.StopWalk();
	            _isMoveAnimationActive = false;
	        }
	    }

	    private void OnMoveComplete()
	    {
	        _unit.MeshTransform.LookAt(_unitsLookAtTarget);
	    }

	    private Vector3 CalculateIntermediatePoint(Vector3 startPoint, Vector3 endPoint)
	    {
	        float length = (endPoint - startPoint).magnitude;
	        Vector3 direction = (endPoint - startPoint).normalized;
	        Vector3 randomPoint = startPoint + (direction * (length * Random.Range(_minArcPosition, _maxArcPosition)));

	        Vector3 perpendicular = new Vector3(-direction.z, 0, direction.x).normalized;
	        float arcOffset = (endPoint - startPoint).magnitude * Random.Range(_minArcOffset, _maxArcOffset);
	        Vector3 intermediatePoint = randomPoint + (perpendicular * arcOffset);

	        return intermediatePoint;
	    }
	}
}
