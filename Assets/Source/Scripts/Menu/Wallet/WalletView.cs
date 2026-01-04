using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private NumberTextGrowAnimator _goldTextAnimator;
    [SerializeField] private TextGrowAnimatorSettings _animationSettings;

    private WalletProvider _wallet;
    private int _currentGoldViewAmount = 0;

    private void OnEnable()
    {
        _wallet.GoldAmountChanged += OnGoldAmountChanged;
    }

    private void OnDisable()
    {
        _wallet.GoldAmountChanged -= OnGoldAmountChanged;
    }

    public void Initialize(WalletProvider walletProvider)
    {
        _wallet = walletProvider;
        _currentGoldViewAmount = _wallet.GoldAmount;
        _goldTextAnimator.SetValueImediatly(_currentGoldViewAmount);
        enabled = true;
    }

    public void OnGoldAmountChanged(int amount)
    {
        _goldTextAnimator.ShowGrowAnimation(_animationSettings, amount, _currentGoldViewAmount);
        _currentGoldViewAmount = amount;
    }
}
