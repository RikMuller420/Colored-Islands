public class IslandFinishBoost : Boost
{
    private SelectHandler _selectHandler;
    private ClickHandler _gameClickHandler;
    private IslandFinishBehaviour _islandFinishBehaviour;
    private LevelLoader _levelLoader;

    private bool _isBoostApplying = false;

    public IslandFinishBoost(SelectHandler selectHandler, ClickHandler gameClickHandler,
                             IslandFinishBehaviour islandInstantFinisher,
                             LevelLoader levelLoader)
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
    }

    private void StopBoostApplying()
    {
        _gameClickHandler.ResetClickHandler();
        _isBoostApplying = false;
    }

    private void OnBoostApplyed()
    {
        StopBoostApplying();
        InvokeBoostApplyedEvent();
    }

    private void OnLevelChanged()
    {
        if (_isBoostApplying)
        {
            StopBoostApplying();
        }
    }
}
