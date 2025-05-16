using UnityEngine;

public class IslandFinishBoost
{
    private SelectHandler _selectHandler;
    private GameClickHandler _gameClickHandler;
    private IslandInstantFinisher _islandInstantFinisher;
    private ClickHandlerData _islandFinisher;
    private BoostButtonAnimator _buttonAnimator;
    private LevelLoader _levelLoader;

    private bool _isBoostApplying = false;

    public IslandFinishBoost(SelectHandler selectHandler, GameClickHandler gameClickHandler,
                             IslandInstantFinisher islandInstantFinisher, LayerMask paintedIslands,
                             BoostButtonAnimator buttonAnimators, LevelLoader levelLoader)
    {
        _selectHandler = selectHandler;
        _gameClickHandler = gameClickHandler;
        _islandInstantFinisher = islandInstantFinisher;
        _islandFinisher = new ClickHandlerData(islandInstantFinisher, paintedIslands);
        _buttonAnimator = buttonAnimators;
        _levelLoader = levelLoader;

        _islandInstantFinisher.IslandFinished += OnBoostApplyed;
        _levelLoader.LevelChanged += OnLevelChanged;
    }

    public void StartBoostApplying()
    {
        _selectHandler.ResetSelection();
        _gameClickHandler.SetClickHandler(_islandFinisher);
        _buttonAnimator.ShowFinishIslandAnimation();
        _isBoostApplying = true;
    }

    private void OnBoostApplyed()
    {
        StopBoostApplying();
    }

    private void OnLevelChanged()
    {
        if (_isBoostApplying)
        {
            StopBoostApplying();
        }
    }

    private void StopBoostApplying()
    {
        _gameClickHandler.ResetClickHandler();
        _buttonAnimator.StopAnimation();
        _isBoostApplying = false;
    }
}
