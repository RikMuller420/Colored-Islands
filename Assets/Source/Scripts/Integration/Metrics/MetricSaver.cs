using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data;
using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using SlimeGround.Menu.Windows.InAppPurchase;
using YG;

namespace SlimeGround.Integration.Metrics
{
	public class MetricSaver
	{
		private static MetricSaver s_instance;

		private readonly ILevelData _levelData;
		private readonly IPlayerData _playerData;

	    public MetricSaver(ILevelData currentLevelData, IPlayerData playerData)
	    {
			s_instance = this;
	        _levelData = currentLevelData;
	        _playerData = playerData;
	    }

		public static void TrackSpendBoost(BoostType type)
	    {
	        int levelId = s_instance._levelData.LevelId;
			YG2.MetricaSend(MetricKeys.BoostSpended, type.ToString(), s_instance._levelData.LevelId.ToString());
		}

		public static void TrackLeaderboardWindowOpened()
	    {
			YG2.MetricaSend(MetricKeys.OpenLeaderboard);
		}

		public static void TrackCustomizationWindowClosed(float spendedSeconds)
	    {
			YG2.MetricaSend(MetricKeys.TimeSpentInWindow, MetricKeys.Customization, spendedSeconds.ToString());
		}

		public static void TrackCustomizationPreferencesChanged()
		{
			IEnumerable<UnitSlotType> slotCollection = Enum.GetValues(typeof(UnitSlotType)).Cast<UnitSlotType>();
			Dictionary<string, object> slimeSlots = new Dictionary<string, object>();

			foreach (UnitSlotType slot in slotCollection)
			{
				CustomizationPreferences slimePreference = s_instance._playerData.Customization.GetCustomizationPreference(slot);

				string slotName = $"{MetricKeys.Slime}_{(int)slot}";

				var slotPrefrence = new Dictionary<string, object>
				{
					{ MetricKeys.Color, slimePreference.ColorSample.ToString() },
					{ MetricKeys.Face, slimePreference.FaceId.ToString() },
					{ MetricKeys.Hat, slimePreference.HatId.ToString() }
				};

				slimeSlots.Add(slotName, slotPrefrence);
			}

			YG2.MetricaSend(MetricKeys.CustomizationChanged, slimeSlots);
		}

		public static void TrackLevelStarted()
	    {
			YG2.MetricaSend(MetricKeys.LevelStarted, MetricKeys.Level, s_instance._levelData.LevelId.ToString());
		}

		public static void TrackLevelFinish()
	    {
			YG2.MetricaSend(MetricKeys.LevelFinished, MetricKeys.Level, s_instance._levelData.LevelId.ToString());
		}

	    public static void TrackRouleteSpinned()
	    {
			YG2.MetricaSend(MetricKeys.RouletteSpinned);
		}

		public static void TrackUpgradePurchased(UpgradeType type)
	    {
			YG2.MetricaSend(MetricKeys.InGamePurchase, MetricKeys.Upgrade, type.ToString());
		}

		public static void TrackBoostPurchased(BoostType type)
	    {
			YG2.MetricaSend(MetricKeys.InGamePurchase, MetricKeys.Boost, type.ToString());
		}

		public static void TrackRouletteSpinPurchased()
		{
			YG2.MetricaSend(MetricKeys.InGamePurchase, MetricKeys.RouletteSpin, MetricKeys.Blank);
		}

		public static void TrackAngryBarFailed()
	    {
			YG2.MetricaSend(MetricKeys.LevelTaskFailed, MetricKeys.AngryBar, s_instance._levelData.LevelId.ToString());
		}

		public static void TrackMoveLimitFailed()
	    {
			YG2.MetricaSend(MetricKeys.LevelTaskFailed, MetricKeys.MoveLimit, s_instance._levelData.LevelId.ToString());
		}

		public static void TrackInAppViaAddWatched(InAppType inAppType)
	    {
			YG2.MetricaSend(MetricKeys.AdShowed, MetricKeys.InAppViaWathAdd, inAppType.ToString());
		}

		public static void TrackFreeGoldAddWatched()
	    {
			YG2.MetricaSend(MetricKeys.AdShowed, MetricKeys.FreeGold, MetricKeys.Blank);
		}

		public static void TrackStandartLevelRewardReceived()
	    {
			YG2.MetricaSend(MetricKeys.StandartLevelRewardReceived);
	    }

	    public static void TrackMultiplayedLevelRewardWithAddReceived()
	    {
			YG2.MetricaSend(MetricKeys.MultipliedLevelRewardReceived);
	    }

	    public static void TrackGetFreeBoostAddWatched(BoostType type)
	    {
			YG2.MetricaSend(MetricKeys.AdShowed, MetricKeys.Boost, type.ToString());
		}	
	}
}
