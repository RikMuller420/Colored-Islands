using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Source.Scripts.Metrics;
using GameAnalyticsSDK;


public class MetricSaver
{
    private const string CustomizationWindowKey = "CustomizationWindow";
    private const string BoostCurrency = "Boost";
    private const string GoldCurrency = "Gold";
    private const string UpgradeCurrency = "Upgrade";
    private const string SpinCurrency = "Spin";

    private LevelProgressTracker _progressTracker { get; }
    private GameProgressStorage _progressStorage { get; }

    public static MetricSaver Instance { get; private set; }

    public MetricSaver(LevelProgressTracker progressTracker, GameProgressStorage progressStorage)
    {
        _progressTracker = progressTracker;
        _progressStorage = progressStorage;
        Instance = this;
    }

    public static void SpentBoost(BoostType type)
    {
        int levelId = Instance._progressTracker.LevelData.Id;

        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, BoostCurrency, 1, type.ToString(), levelId.ToString());
    }


    public static void OpenLeaderboardWindow()
    {
        GameAnalytics.NewDesignEvent("Leaderboard");
    }


    public static void OpenCustomizationWindow()
    {
        GameAnalytics.StartTimer(CustomizationWindowKey);
        GameAnalytics.NewDesignEvent(CustomizationWindowKey);
    }

    public static void CloseCustomizationWindow()
    {
        GameAnalytics.StopTimer(CustomizationWindowKey);

        IEnumerable<Paint> slimeTypes = Enum.GetValues(typeof(Paint)).Cast<Paint>();
        Dictionary<string, object> slimeSlots = new Dictionary<string, object>();

        foreach (Paint paint in slimeTypes)
        {
            CustomizationPreferences slimePreference = Instance._progressStorage.GetCustomizationPreference(paint);

            SlimeSlot slimeSlot = new SlimeSlot()
            {
                ColorId = slimePreference.ColorSample.ToString(),
                FaceId = slimePreference.FaceId.ToString(),
                HatId = slimePreference.HatId.ToString()
            };

            slimeSlots.Add(((int)paint).ToString(), slimeSlot);
        }

        GameAnalytics.NewDesignEvent("avatar:preferences:snapshot", slimeSlots.Count, slimeSlots);
    }

    public static void StartLevel()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Instance._progressTracker.LevelData.Id.ToString());
    }

    public static void FinishLevel()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Instance._progressTracker.LevelData.Id.ToString());
    }

    public static void GetRouleteSpin(int spinCount)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, SpinCurrency, spinCount, SpinCurrency, SpinCurrency);
    }

    public static void SpinRoulete()
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Sink, SpinCurrency, 1, SpinCurrency, SpinCurrency);
    }

    public static void BuyUpgrade(UpgradeType type)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, UpgradeCurrency, 1, type.ToString(), type.ToString());
    }

    public static void BuyBoost(BoostType type)
    {
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, BoostCurrency, 1, type.ToString(), type.ToString());
    }

    public static void TrackAngryBarFailed()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Angry Bar " + Instance._progressTracker.LevelData.Id);
    }

    public static void TrackMoveLimitFailed()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Move Limit " + Instance._progressTracker.LevelData.Id);
    }

    public static void GetInAppViaWathAdd(InAppType inAppType)
    {
        GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Video, inAppType.ToString(), "UI");
    }

    public static void ShowGetFreeGoldAdd()
    {
        GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Video, "Free Gold", "UI");
    }

    public static void ReceiveStandartLevelReward()
    {
        GameAnalytics.NewDesignEvent("Receive Standart Level Reward");
    }

    public static void ReceiveMultiplayedLevelRewardWithAdd()
    {
        GameAnalytics.NewDesignEvent("Receive Multiplied Level Reward");
        GameAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Video, "Multiply Level Rewards", "Level Reward");
    }

    public static void ShowGetFreeBoostAdd(BoostType type)
    {
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, "Boost", type.ToString());
    }
}
