using UnityEngine;

public class ScoreAnimator : MonoBehaviour
{
    [SerializeField] private NumberTextGrowAnimator _growAnimator;

    [SerializeField] private TextGrowAnimatorSettings _growSettingsWin;
    [SerializeField] private TextGrowAnimatorSettings _growSettingsFail;

    public void ResetAnimation()
    {
        _growAnimator.ResetAnimation();
    }

    public void ShowWinAnimation(int reachedScore)
    {
        _growAnimator.ShowGrowAnimation(_growSettingsWin, reachedScore);
    }

    public void ShowFailAnimation(int reachedScore)
    {
        _growAnimator.ShowGrowAnimation(_growSettingsFail, reachedScore);
    }
}
