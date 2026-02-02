using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class UnitLookAtRotator
	{
	    private Transform _body;
	    private Vector2 _lookAtDurationInterval = new Vector2(0.2f, 0.35f);
	    private Vector2 _lookBackDurationInterval = new Vector2(0.4f, 0.7f);
	    private Vector2 _awaitInterval = new Vector2(0.1f, 0.2f);
	    private Vector2 _lookAtAngleFirstInterval = new Vector2(10f, 20f);
	    private Vector2 _lookAtAngleSecondInterval = new Vector2(5f, 10f);
	    private Quaternion _initialLocalRotation;

	    private Sequence _lookAtSequenceStart;
	    private Sequence _lookAtSequenceEnd;

		public UnitLookAtRotator(Transform body)
	    {
	        _body = body;
	        _initialLocalRotation = _body.localRotation;
	    }

		private float RandomLookAtTime => Random.Range(_lookAtDurationInterval.x, _lookAtDurationInterval.y);
		private float RandomAwaitTime => Random.Range(_awaitInterval.x, _awaitInterval.y);
		private float RandomLookBackTime => Random.Range(_lookBackDurationInterval.x, _lookBackDurationInterval.y);

		public void ResetRotation()
	    {
	        StopRotationSequences();
	        _body.localRotation = _initialLocalRotation;
	    }

	    public void LookToTarget(Transform target, UnitsMoveInfo unitsMoveInfo)
	    {
	        StopRotationSequences();

	        _lookAtSequenceStart = DOTween.Sequence()
	                .Append(_body.DORotateQuaternion(CalculateRotation(target.position, _lookAtAngleFirstInterval), RandomLookAtTime)
	                .SetEase(Ease.InOutQuad))
	                .OnComplete(() => LookAtMovedUnits(unitsMoveInfo));
	    }

		private void StopRotationSequences()
	    {
	        _lookAtSequenceStart?.Kill();
	        _lookAtSequenceEnd?.Kill();
	    }

	    private void LookAtMovedUnits(UnitsMoveInfo unitsMoveInfo)
	    {
	        _lookAtSequenceEnd = DOTween.Sequence()
	            .Append(_body.DORotateQuaternion(CalculateLookToUnitsRotation(unitsMoveInfo), RandomLookAtTime))
	            .SetEase(Ease.InQuad)
	            .AppendInterval(RandomAwaitTime)
	            .Append(_body.DOLocalRotateQuaternion(_initialLocalRotation, RandomLookBackTime)
	            .SetEase(Ease.InOutQuad));
	    }

	    private Quaternion CalculateLookToUnitsRotation(UnitsMoveInfo unitsMoveInfo)
	    {
	        Vector3 target = unitsMoveInfo.EndIsland.Points
	                            .Where(point => point.IsFree == false && unitsMoveInfo.Units.Contains(point.OccupiedUnit))
	                            .Aggregate(Vector3.zero, (sum, point) => sum + point.Transform.position)
	                            / unitsMoveInfo.Units.Count;

	        return CalculateRotation(target, _lookAtAngleSecondInterval);
	    }

	    private Quaternion CalculateRotation(Vector3 target, Vector2 angleInterval)
	    {
	        Vector3 direction = target - _body.position;
	        direction.y = 0;
	        Quaternion targetRotation = Quaternion.LookRotation(direction);

	        Vector3 eulerRotation = targetRotation.eulerAngles;
	        Quaternion yOnlyRotation = Quaternion.Euler(0, eulerRotation.y, 0);

	        float angle = Quaternion.Angle(_body.rotation, yOnlyRotation);
	        float maxAngle = Random.Range(angleInterval.x, angleInterval.y);

	        if (angle > maxAngle)
	        {
	            yOnlyRotation = Quaternion.Slerp(_body.rotation, yOnlyRotation, maxAngle / angle);
	        }

	        return yOnlyRotation;
	    }
	}
}
