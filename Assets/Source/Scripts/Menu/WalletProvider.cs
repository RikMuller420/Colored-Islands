using System;
using SlimeGround.Data.Saves;

namespace SlimeGround.Menu
{
	public class WalletProvider
	{
	    private PlayerDataProvider _playerData;

	    public WalletProvider(PlayerDataProvider playerData)
	    {
	        _playerData = playerData;
	        _playerData.GoldAmountChanged += OnGoldAmountChanged;
	    }

	    public event Action<int> GoldAmountChanged;
	    
	    public int GoldAmount => _playerData.GoldAmount;

		public void Dispose()
		{
			_playerData.GoldAmountChanged -= OnGoldAmountChanged;
		}

	    public void AddGold(int amount)
	    {
	        if (amount < 0)
	        {
	            throw new ArgumentException(nameof(amount));
	        }

	        int newGoldAmount = GoldAmount + amount;
	        _playerData.SetGoldAmount(newGoldAmount);
	        _playerData.Save();
	    }

	    public void SpendGold(int amount)
	    {
	        if (amount < 0)
	        {
	            throw new ArgumentException(nameof(amount));
	        }

	        if (GoldAmount - amount < 0)
	        {
	            throw new InvalidOperationException("not enough gold");
	        }

	        int newGoldAmount = GoldAmount - amount;
	        _playerData.SetGoldAmount(newGoldAmount);
	        _playerData.Save();
	    }

	    private void OnGoldAmountChanged()
	    {
	        GoldAmountChanged?.Invoke(GoldAmount);
	    }
	}
}
