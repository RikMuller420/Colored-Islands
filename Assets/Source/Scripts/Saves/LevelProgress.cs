using System;
using UnityEngine;

[Serializable]
public class LevelProgress
{
    [SerializeField] private int _id;
    [SerializeField] private bool _isDone;
    [SerializeField] private bool _isMoveTaskDone;
    [SerializeField] private bool _isTimeTaskDone;

    public LevelProgress(int id)
    {
        _id = id;
    }

    public LevelProgress(int id, bool isDone, bool isMoveTaskDone, bool isTimeTaskDone)
    {
        _id = id;
        _isDone = isDone;
        _isMoveTaskDone = isMoveTaskDone;
        _isTimeTaskDone = isTimeTaskDone;
    }

    public int Id => _id;
    public bool IsDone => _isDone;
    public bool IsMoveTaskDone => _isMoveTaskDone;
    public bool IsTimeTaskDone => _isTimeTaskDone;
}
