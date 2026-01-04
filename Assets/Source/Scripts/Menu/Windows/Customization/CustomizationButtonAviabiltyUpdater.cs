using System;
using System.Collections.Generic;
using System.Linq;

public class CustomizationButtonAviabiltyUpdater
{
    private LevelProgressTracker _levelProgressTracker;
    private GameProgressStorage _progressStorage;
    private List<HatSelectButton> _hatSelectButtons;
    private List<FaceSelectButton> _faceSelectButtons;

    public event Action HatButtonUnlocked;
    public event Action FaceButtonUnlocked;

    public CustomizationButtonAviabiltyUpdater(LevelProgressTracker levelProgressTracker, GameProgressStorage gameProgressStorage,
                                List<HatSelectButton> hatSelectButtons, List<FaceSelectButton> faceSelectButtons)
    {
        _levelProgressTracker = levelProgressTracker;
        _progressStorage = gameProgressStorage;
        _hatSelectButtons = hatSelectButtons;
        _faceSelectButtons = faceSelectButtons;

        _levelProgressTracker.LevelFinished += UpdateHatAviability;
        _progressStorage.FaceUnlocked += UpdateFaceAviability;
    }

    private void UpdateFaceAviability(int faceId)
    {
        FaceSelectButton faceButton = _faceSelectButtons.FirstOrDefault(face => face.FaceId == faceId);
        faceButton.SetUnlockedStyle();
        faceButton.ActivateUnusedMark();
        FaceButtonUnlocked?.Invoke();
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
