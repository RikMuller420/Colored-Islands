using System;

public class WalletProvider
{
    private GameProgressStorage _gameProgressStorage;

    public event Action<int> GoldAmountChanged;

    public WalletProvider(GameProgressStorage gameProgressStorage)
    {
        _gameProgressStorage = gameProgressStorage;
        _gameProgressStorage.GoldAmountChanged += OnGoldAmountInSavedProgressChanged;
    }

    public int GoldAmount => _gameProgressStorage.GoldAmount;

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
        _gameProgressStorage.SetGoldAmount(newGoldAmount);
    }

    private void OnGoldAmountInSavedProgressChanged()
    {
        GoldAmountChanged?.Invoke(GoldAmount);
    }
}
