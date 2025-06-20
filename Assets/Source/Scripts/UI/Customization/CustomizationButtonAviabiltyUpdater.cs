using System.Collections.Generic;

public class CustomizationButtonAviabiltyUpdater
{
    private LevelProgressTracker _levelProgressTracker;
    private GameProgressStorage _gameProgressStorage;
    private List<HatSelectButton> _hatSelectButtons;
    private List<FaceSelectButton> _faceSelectButtons;

    public CustomizationButtonAviabiltyUpdater(LevelProgressTracker levelProgressTracker, GameProgressStorage gameProgressStorage,
                                List<HatSelectButton> hatSelectButtons, List<FaceSelectButton> faceSelectButtons)
    {
        _levelProgressTracker = levelProgressTracker;
        _gameProgressStorage = gameProgressStorage;
        _hatSelectButtons = hatSelectButtons;
        _faceSelectButtons = faceSelectButtons;

        _levelProgressTracker.LevelFinished += UpdateHatAviability;
    }

    private void UpdateHatAviability()
    {
        foreach (HatSelectButton hatButton in _hatSelectButtons)
        {
            if (hatButton.RequredLevel < _gameProgressStorage.FirstUnfinishedLevel.Id)
            {
                hatButton.SetUnlockedStyle();
            }
            else
            {
                hatButton.SetLockedStyle();
            }
        }
    }
}
