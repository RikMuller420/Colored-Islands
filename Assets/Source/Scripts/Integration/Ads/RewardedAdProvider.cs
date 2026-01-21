using System;
using YG;

namespace SlimeGround.Integration.Ads
{

	public class RewardedAdProvider
	{
	    public event Action RewardedAdClosed;

	    public RewardedAdProvider()
	    {
	        YG2.onCloseRewardedAdv += OnCloseRewardedAdv;
	    }

	    public void ShowAdvReward(string id, Action receiveRewerd)
	    {
	        YG2.RewardedAdvShow(id, receiveRewerd);
	    }

	    public void OnCloseRewardedAdv()
	    {
	        RewardedAdClosed?.Invoke();
	    }
	}

}
