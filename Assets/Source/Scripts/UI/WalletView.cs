using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private NumberTextGrowAnimator _goldTextAnimator;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextGrowAnimatorSettings _animationSettings;

    private int _startGoldAmount = 0;

    private void OnEnable()
    {
        _wallet.GoldAmountChanged += OnGoldAmountChanged;
    }

    private void OnDisable()
    {
        _wallet.GoldAmountChanged -= OnGoldAmountChanged;
    }

    private void Start()
    {
        _startGoldAmount = _wallet.GoldAmount;
        _goldTextAnimator.SetValueImediatly(_startGoldAmount);
    }

    public void OnGoldAmountChanged(int amount)
    {
        _goldTextAnimator.ShowGrowAnimation(_animationSettings, amount, _startGoldAmount);
        _startGoldAmount = amount;
    }
}
