using UnityEngine;

public class RouletteWindow : MenuWindow
{
    [SerializeField] private Roulette _roulette;
    [SerializeField] private RouletteWheel _rouletteWheel;
    [SerializeField] private RouletteRewardWindow _rouletteRewardWindow;

    private GameProgressStorage _progressStorage;

    public void Initialize(GameProgressStorage progressStorage)
    {
        _progressStorage = progressStorage;
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
        if (_progressStorage.AviableSpinCount == 0)
        {
            Close();
        }
    }
}
