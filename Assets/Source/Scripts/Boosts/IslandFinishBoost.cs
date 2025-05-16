using System;

public class IslandFinishBoost : Boost
{
    private SelectHandler _selectHandler;
    private ClickHandler _gameClickHandler;
    private IslandFinishBehaviour _islandFinishBehaviour;
    private LevelLoader _levelLoader;

    private bool _isBoostApplying = false;

    public event Action BoostStartApplyed;
    public event Action BoostStopApplyed;

    public IslandFinishBoost(SelectHandler selectHandler, ClickHandler gameClickHandler,
                             IslandFinishBehaviour islandInstantFinisher,
                             LevelLoader levelLoader, BoostAmountProvider boostAmountProvider) :
                             base(boostAmountProvider)
    {
        _selectHandler = selectHandler;
        _gameClickHandler = gameClickHandler;
        _islandFinishBehaviour = islandInstantFinisher;
        _levelLoader = levelLoader;

        _islandFinishBehaviour.IslandFinished += OnBoostApplyed;
        _levelLoader.LevelChanged += OnLevelChanged;
    }

    public override void TryApplyBoost()
    {
        if (_isBoostApplying)
        {
            StopBoostApplying();
        }
        else
        {
            StartBoostApplying();
        }
    }

    private void StartBoostApplying()
    {
        _selectHandler.ResetSelection();
        _gameClickHandler.SetClickBehaviour(_islandFinishBehaviour);
        _isBoostApplying = true;
        BoostStartApplyed?.Invoke();
    }

    private void StopBoostApplying()
    {
        _gameClickHandler.ResetClickHandler();
        _isBoostApplying = false;
        BoostStopApplyed?.Invoke();
    }

    private void OnBoostApplyed()
    {
        StopBoostApplying();
        SpendBoost<IslandFinishBoost>();
    }

    private void OnLevelChanged()
    {
        if (_isBoostApplying)
        {
            StopBoostApplying();
        }
    }
}
