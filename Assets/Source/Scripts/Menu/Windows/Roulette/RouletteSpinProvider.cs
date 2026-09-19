using System;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Roulette;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class RouletteSpinProvider
	{
		private readonly PlayerDataProvider _playerData;

		public RouletteSpinProvider(PlayerDataProvider playerData, RouletteSettings rouletteSettings)
		{
			_playerData = playerData;
			RouletteSettings = rouletteSettings;
			_playerData.Resources.SpinCountChanged += OnSpinAmountInSavedProgressChanged;
		}

		public event Action SpinAmountChanged;

		public RouletteSettings RouletteSettings { get; private set; }

		public int SpinAmount => _playerData.Resources.AviableSpinCount;

		public void Dispose()
		{
			_playerData.Resources.SpinCountChanged -= OnSpinAmountInSavedProgressChanged;
		}

		public void SpendSpin()
		{
			int spinAmount = SpinAmount;

			if (spinAmount == 0)
			{
				throw new InvalidOperationException("not enough Spins");
			}

			spinAmount--;
			_playerData.Resources.SetSpinCount(spinAmount);
			_playerData.Save();
		}

		public void AddSpin()
		{
			int spinAmount = SpinAmount;
			spinAmount++;
			_playerData.Resources.SetSpinCount(spinAmount);
			_playerData.Save();
		}

		private void OnSpinAmountInSavedProgressChanged()
		{
			SpinAmountChanged?.Invoke();
		}
	}
}
