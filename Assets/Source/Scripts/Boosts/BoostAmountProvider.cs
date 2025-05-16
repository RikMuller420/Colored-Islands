using System;

public class BoostAmountProvider 
{
    private GameProgressStorage _gameProgressStorage;

    public event Action BoostsAmountChanged;

    public BoostAmountProvider(GameProgressStorage gameProgressStorage)
    {
        _gameProgressStorage = gameProgressStorage;
        _gameProgressStorage.BoostsAmountChanged += OnBoostsAmountInSavedProgressChanged;
    }

    public int BufferIslandBoostAmount => _gameProgressStorage.GetBoostAmount<BufferIslandBoost>();

    public int BoostAmount<T>() where T : Boost => _gameProgressStorage.GetBoostAmount<T>();

    public void SpendBoost<T>() where T : Boost
    {
        int boostAmount = BoostAmount<T>();

        if (boostAmount == 0)
        {
            throw new InvalidOperationException("not enough Boosts");
        }

        boostAmount--;
        _gameProgressStorage.SetBoostAmount<T>(boostAmount);
    }

    private void OnBoostsAmountInSavedProgressChanged()
    {
        BoostsAmountChanged?.Invoke();
    }
}
