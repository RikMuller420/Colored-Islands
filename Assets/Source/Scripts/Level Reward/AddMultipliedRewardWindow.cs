using UnityEngine;

public class AddMultipliedRewardWindow : MenuWindow
{
    [SerializeField] private LevelRewardView _levelRewardView;

    public void Open(LevelRewardData levelRewardData, int adsMultiplier)
    {
        _levelRewardView.SetIcons(levelRewardData, adsMultiplier);

        base.Open();
    }
}
