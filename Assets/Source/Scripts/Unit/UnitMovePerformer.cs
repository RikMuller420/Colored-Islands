using System.Collections.Generic;
using UnityEngine;

public class UnitMovePerformer : MonoBehaviour
{
    private Transform _unitsLookAtPoint;

    private float _moveSpeed = 25f;
    private float _reachThreshold = 0.01f;
    private List<UnitMoveTask> _tasks = new();

    private void Update()
    {
        float stepDistance = _moveSpeed * Time.deltaTime;

        foreach (UnitMoveTask task in _tasks)
        {
            PreformTask(task, stepDistance);
        }

        RemoveFinishedTasks();
    }

    public void Initialize(Transform unitsLookAtPoint)
    {
        _unitsLookAtPoint = unitsLookAtPoint;

        enabled = true;
    }

    public void AddTask(UnitMoveTask unitMoveTask)
    {
        _tasks.RemoveAll(task => task.Unit == unitMoveTask.Unit);
        _tasks.Add(unitMoveTask);
    }

    private void PreformTask(UnitMoveTask task, float stepDistance)
    {
        Vector3 unitPosition = Vector3.MoveTowards
        (
            task.CurrentPosition,
            task.TargetPosition,
            stepDistance
        );

        task.Unit.transform.position = unitPosition;
    }

    private void RemoveFinishedTasks()
    {
        for (int i = _tasks.Count - 1; i >= 0; i--)
        {
            if (_tasks[i].SqrDistToTarget < _reachThreshold)
            {
                _tasks[i].Unit.MeshTransform.LookAt(_unitsLookAtPoint);
                _tasks.RemoveAt(i);
            }
        }
    }
}
