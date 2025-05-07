using System;
using UnityEngine;

public class ObjectivesAnimator : MonoBehaviour
{
    [SerializeField] private ObjectiveAnimator _timeObjective;
    [SerializeField] private ObjectiveAnimator _movesObjective;
    [SerializeField] private ObjectiveAnimator _goldObjective;
    [SerializeField] private NumberTextGrowAnimator _goldTextAnimator;

    public void ResetObjectives()
    {
        _timeObjective.ResetObjective();
        _movesObjective.ResetObjective();
        _goldObjective.ResetObjective();
        _goldTextAnimator.ResetAnimation();
    }

    public void ShowTimeObjectiveAnimation(LevelProgressTracker progressTracker, out float animationDuration)
    {
        animationDuration = _timeObjective.AnimationDuration;
        string time = SecondsToString(progressTracker.LevelData.ExtraStarTimeLimit);
        _timeObjective.ShowAppearAnimation(time, progressTracker.IsTimeTaskDone);
    }

    public void ShowMoveObjectiveAnimation(LevelProgressTracker progressTracker, out float animationDuration)
    {
        animationDuration = _movesObjective.AnimationDuration;
        string moves = progressTracker.LevelData.ExtraStarMoveLimit.ToString();
        _movesObjective.ShowAppearAnimation(moves, progressTracker.IsMoveTaskDone);
    }

    public void ShowGoldAnimation(LevelProgressTracker progressTracker, out float animationDuration)
    {
        animationDuration = _goldObjective.AnimationDuration;
        int reachedGold = 0;
        _goldObjective.ShowAppearAnimation(reachedGold.ToString(), true);

        if (progressTracker.ReachedGold > 0)
        {
            _goldTextAnimator.ShowGrowAnimation(progressTracker.ReachedGold);
        }
    }

    private string SecondsToString(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        return $"{(int)time.TotalMinutes}:{time.Seconds:D2}";
    }
}
