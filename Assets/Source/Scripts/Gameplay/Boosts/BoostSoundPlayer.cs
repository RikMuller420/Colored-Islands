using System.Collections.Generic;

public class BoostSoundPlayer
{
    private GameplaySoundPlayer _gameplaySoundPlayer;
    private MenuWindow _outOfBoostWindow;
    private IEnumerable<Boost> _boosts;

    public BoostSoundPlayer(GameplaySoundPlayer gameplaySoundPlayer, MenuWindow outOfBoostWindow,
                            IEnumerable<Boost> boosts)
    {
        _boosts = boosts;
        _outOfBoostWindow = outOfBoostWindow;
        _gameplaySoundPlayer = gameplaySoundPlayer;

        foreach (Boost boost in _boosts)
        {
            boost.BoostApplyed += OnBoostApplyed;
        }

        _outOfBoostWindow.MenuOpened += OutOfBoostWindowOpened;
    }

    private void OutOfBoostWindowOpened()
    {
        _gameplaySoundPlayer.PlaySound(GameplaySoundType.OutOfBoost);
    }

    private void OnBoostApplyed(Boost boost)
    {
        _gameplaySoundPlayer.PlaySound(GameplaySoundType.ApplyBoost);
    }
}
