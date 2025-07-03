using System;
using System.Collections.Generic;

public class CustomizationButtonAviabiltyUpdater
{
    private LevelProgressTracker _levelProgressTracker;
    private GameProgressStorage _progressStorage;
    private List<HatSelectButton> _hatSelectButtons;
    private List<FaceSelectButton> _faceSelectButtons;

    public event Action HatButtonUnlocked;

    public CustomizationButtonAviabiltyUpdater(LevelProgressTracker levelProgressTracker, GameProgressStorage gameProgressStorage,
                                List<HatSelectButton> hatSelectButtons, List<FaceSelectButton> faceSelectButtons)
    {
        _levelProgressTracker = levelProgressTracker;
        _progressStorage = gameProgressStorage;
        _hatSelectButtons = hatSelectButtons;
        _faceSelectButtons = faceSelectButtons;

        _levelProgressTracker.LevelFinished += UpdateHatAviability;
    }

    private void UpdateHatAviability()
    {
        foreach (HatSelectButton hatButton in _hatSelectButtons)
        {
            if (hatButton.RequredLevel < _progressStorage.LastAvailableLevelId)
            {
                hatButton.SetUnlockedStyle();

                if (_progressStorage.WasHatUsed(hatButton.HatId) == false)
                {
                    hatButton.ActivateUnusedMark();
                }

                HatButtonUnlocked?.Invoke();
            }
            else
            {
                hatButton.SetLockedStyle();
            }
        }
    }
}
