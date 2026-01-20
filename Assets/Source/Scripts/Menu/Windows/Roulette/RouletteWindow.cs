using UnityEngine;

public class RouletteWindow : MenuWindow
{
    [SerializeField] private Roulette _roulette;
    [SerializeField] private RouletteWheel _rouletteWheel;
    [SerializeField] private RouletteRewardWindow _rouletteRewardWindow;

    private IPlayerData _playerData;

    public void Initialize(IPlayerData playerData)
    {
        _playerData = playerData;
        enabled = true;
    }

    protected override void OnEnable()
    {
        _rouletteWheel.SpinStarted += DisableCloseButtons;
        _rouletteWheel.SpinFinished += EnableCloseButtons;
        _rouletteRewardWindow.MenuClosed += OnRewardReciewed;
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        _rouletteWheel.SpinStarted -= DisableCloseButtons;
        _rouletteWheel.SpinFinished -= EnableCloseButtons;
        _rouletteRewardWindow.MenuClosed -= OnRewardReciewed;
        base.OnDisable();
    }

    public override void Open()
    {
        _roulette.PrepareRoulette();
        base.Open();
    }

    private void EnableCloseButtons(Slot _) => EnableCloseButtons();

    private void OnRewardReciewed()
    {
        if (_playerData.AviableSpinCount == 0)
        {
            Close();
        }
    }
}
