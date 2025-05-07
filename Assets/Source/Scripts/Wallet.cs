using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private int _goldAmount;

    public event Action<int> GoldAmountChanged;

    public int GoldAmount => _goldAmount;

    public void AddGold(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(nameof(amount));
        }

        _goldAmount += amount;
        GoldAmountChanged?.Invoke(_goldAmount);
    }

    public void SpendGold(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException(nameof(amount));
        }

        if (_goldAmount - amount < 0)
        {
            throw new InvalidOperationException("not enough gold");
        }

        _goldAmount -= amount;
        GoldAmountChanged?.Invoke(_goldAmount);
    }
}
