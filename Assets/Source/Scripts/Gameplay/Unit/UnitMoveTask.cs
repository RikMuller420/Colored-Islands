using DG.Tweening;
using SlimeGround.Gameplay.Islands;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class UnitMoveTask
	{
		private const float DeactivateMoveAnimationPercent = 0.5f;
		private const float MaxMoveSpeed = 10f;
		private const float MinMoveTime = 0.25f;
		private const float MinArcPosition = 0.3f;
		private const float MaxArcPosition = 0.7f;
		private const float MinArcOffset = 0f;
		private const float MaxArcOffset = 0.1f;

		private readonly Transform _unitsLookAtTarget;
		private readonly IslandPoint _targetPoint;
		private readonly Unit _unit;
		private readonly Tween _pathTween;

		private bool _isMoveAnimationActive;

	    public UnitMoveTask(Unit unit, IslandPoint targetPoint, Transform unitsLookAtTarget)
	    {
	        _unit = unit;
	        _targetPoint = targetPoint;
	        _unitsLookAtTarget = unitsLookAtTarget;
	        Vector3 intermediatePoint = CalculateIntermediatePoint(CurrentPosition, TargetPosition);
	        Vector3[] path = { CurrentPosition, intermediatePoint, TargetPosition };

	        float moveTime = (CurrentPosition - TargetPosition).magnitude / MaxMoveSpeed;

	        if (moveTime < MinMoveTime)
	        {
	            moveTime = MinMoveTime;
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

	        if (_pathTween.ElapsedPercentage() >= DeactivateMoveAnimationPercent && _isMoveAnimationActive)
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
	        Vector3 randomPoint = startPoint + (direction * (length * Random.Range(MinArcPosition, MaxArcPosition)));

	        Vector3 perpendicular = new Vector3(-direction.z, 0, direction.x).normalized;
	        float arcOffset = (endPoint - startPoint).magnitude * Random.Range(MinArcOffset, MaxArcOffset);
	        Vector3 intermediatePoint = randomPoint + (perpendicular * arcOffset);

	        return intermediatePoint;
	    }
	}
}
