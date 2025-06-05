using DG.Tweening;
using UnityEngine;

public class UnitMoveTask
{
    private float _maxMoveSpeed = 10f;
    private float _minMoveTime = 0.25f;
    private float _minArcPosition = 0.3f;
    private float _maxArcPosition = 0.7f;
    private float _minArcOffset = 0f;
    private float _maxArcOffset = 0.1f;

    public UnitMoveTask(Unit unit, IslandPoint targetPoint)
    {
        _unit = unit;
        _targetPoint = targetPoint;
        Vector3 intermediatePoint = CalculateIntermediatePoint(_currentPosition, _targetPosition);
        Vector3[] path = { _currentPosition, intermediatePoint, _targetPosition };

        float moveTime = (_currentPosition - _targetPosition).magnitude / _maxMoveSpeed;

        if (moveTime < _minMoveTime)
        {
            moveTime = _minMoveTime;
        }

        unit.transform.DOKill();
        unit.transform.DOPath(path, moveTime, PathType.CatmullRom).SetEase(Ease.OutQuad);
    }

    private IslandPoint _targetPoint;
    private Unit _unit;

    private Vector3 _currentPosition => _unit.transform.position;
    private Vector3 _targetPosition => _targetPoint.Point.transform.position;


    private Vector3 CalculateIntermediatePoint(Vector3 startPoint, Vector3 endPoint)
    {
        float length = (endPoint - startPoint).magnitude;
        Vector3 direction = (endPoint - startPoint).normalized;
        Vector3 randomPoint = startPoint + direction * (length * Random.Range(_minArcPosition, _maxArcPosition));

        Vector3 perpendicular = new Vector3(-direction.z, 0, direction.x).normalized;
        float arcOffset = (endPoint - startPoint).magnitude * Random.Range(_minArcOffset, _maxArcOffset);
        Vector3 intermediatePoint = randomPoint + perpendicular * arcOffset;

        return intermediatePoint;
    }
}
