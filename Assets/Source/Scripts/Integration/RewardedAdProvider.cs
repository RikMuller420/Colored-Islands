using System;
using YG;

public class RewardedAdProvider
{
    public void ShowAdvReward(string id, Action receiveRewerd)
    {
        YG2.RewardedAdvShow(id, receiveRewerd);
    }
}
