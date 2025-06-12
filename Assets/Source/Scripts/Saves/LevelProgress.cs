using System;
using Newtonsoft.Json;

[Serializable]
public class LevelProgress
{
    [JsonProperty] private int _id;
    [JsonProperty] private bool _isDone;
    [JsonProperty] private bool _isMoveTaskDone;
    [JsonProperty] private bool _isAngryTaskDone;
    [JsonProperty] private int _bestScore;

    public LevelProgress(int id)
    {
        _id = id;
    }

    [JsonConstructor]
    public LevelProgress(int id, bool isDone, bool isMoveTaskDone,
                         bool isAngryTaskDone, int bestScore)
    {
        _id = id;
        _isDone = isDone;
        _isMoveTaskDone = isMoveTaskDone;
        _isAngryTaskDone = isAngryTaskDone;
        _bestScore = bestScore;
    }

    [JsonIgnore] public int Id => _id;
    [JsonIgnore] public bool IsDone => _isDone;
    [JsonIgnore] public bool IsMoveTaskDone => _isMoveTaskDone;
    [JsonIgnore] public bool IsAngryTaskDone => _isAngryTaskDone;
    [JsonIgnore] public int BestScore => _bestScore;
}
