using UnityEngine;

public class ObjectivesAnimator : MonoBehaviour
{
    [SerializeField] private ObjectiveAnimator _angryScoreObjective;
    [SerializeField] private ObjectiveAnimator _movesObjective;
    [SerializeField] private ObjectiveAnimator _goldObjective;
    [SerializeField] private NumberTextGrowAnimator _goldTextAnimator;
    [SerializeField] private TextGrowAnimatorSettings _goldGrowSettings;

    private int _percentMultiplier = 100;

    public void ResetObjectives()
    {
        _angryScoreObjective.ResetObjective();
        _movesObjective.ResetObjective();
        _goldObjective.ResetObjective();
        _goldTextAnimator.ResetAnimation();
    }

    public void ShowAngryScoreAnimation(LevelProgressTracker progressTracker, out float animationDuration)
    {
        animationDuration = _angryScoreObjective.AnimationDuration;
        int revercedAngryPercent = _percentMultiplier - (int)(progressTracker.AngryValue * _percentMultiplier);
        _angryScoreObjective.ShowAppearAnimation($"{revercedAngryPercent}%", progressTracker.IsAngryTaskDone);
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
            _goldTextAnimator.ShowGrowAnimation(_goldGrowSettings, progressTracker.ReachedGold);
        }
    }
}
