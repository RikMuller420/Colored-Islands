using System.Linq;
using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Menu.Windows.GameShop.Upgrades;

namespace SlimeGround.Gameplay.Score
{
	public class GoldCalculator
	{
	    private const int GoldPerNewStar = 50;
	    private const int GoldPerReEarnedStar = 5;

		private readonly IPlayerData _playerData;
		private readonly IUpgradesData _upgradesData;
		private readonly ILevelData _currentLevelData;

		public GoldCalculator(IPlayerData playerData, IUpgradesData upgradesData,
	                          ILevelData currentLevelData)
	    {
	        _playerData = playerData;
	        _upgradesData = upgradesData;
	        _currentLevelData = currentLevelData;
	    }

		public int CalculateLevelGold(bool isAngryTaskDone, bool isMoveTaskDone)
	    {
	        LevelProgress savedProgress = _playerData.Progress.Levels
	                                    .FirstOrDefault(level => level.Id == _currentLevelData.LevelId);
	        int gold = 0;
	        gold += savedProgress.IsDone ? GoldPerReEarnedStar : GoldPerNewStar;

	        if (isAngryTaskDone)
	        {
	            gold += savedProgress.IsAngryStarEarned ? GoldPerReEarnedStar : GoldPerNewStar;
	        }

	        if (isMoveTaskDone)
	        {
	            gold += savedProgress.IsMovesStarEarned ? GoldPerReEarnedStar : GoldPerNewStar;
	        }

	        return _upgradesData.CalculateUpgradedGoldAmount(gold);
	    }
	}
}
