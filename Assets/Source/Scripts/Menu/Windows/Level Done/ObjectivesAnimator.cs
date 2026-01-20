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

    public void ShowAngryScoreAnimation(float angryValue, bool isCompleted, out float animationDuration)
    {
        animationDuration = _angryScoreObjective.AnimationDuration;
        int revercedAngryPercent = _percentMultiplier - (int)(angryValue * _percentMultiplier);
        _angryScoreObjective.ShowAppearAnimation($"{revercedAngryPercent}%", isCompleted);
    }

    public void ShowMoveObjectiveAnimation(int moveCount, bool isCompleted, out float animationDuration)
    {
        animationDuration = _movesObjective.AnimationDuration;
        string moves = moveCount.ToString();
        _movesObjective.ShowAppearAnimation(moves, isCompleted);
    }

    public void ShowGoldAnimation(int earndeGold, out float animationDuration)
    {
        animationDuration = _goldObjective.AnimationDuration;
        int reachedGold = 0;
        _goldObjective.ShowAppearAnimation(reachedGold.ToString(), true);

        if (earndeGold > 0)
        {
            _goldTextAnimator.ShowGrowAnimation(_goldGrowSettings, earndeGold);
        }
    }
}
