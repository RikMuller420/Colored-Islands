using System;
using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data.Saves;
using SlimeGround.Gameplay.Levels;

namespace SlimeGround.Menu.Windows.Customization
{
	public class CustomizationButtonAviabiltyUpdater
	{
	    private LevelProgressTracker _levelProgressTracker;
	    private IPlayerData _playerData;
	    private List<HatSelectButton> _hatSelectButtons;
	    private List<FaceSelectButton> _faceSelectButtons;

	    public CustomizationButtonAviabiltyUpdater(LevelProgressTracker levelProgressTracker, IPlayerData playerData,
	                                List<HatSelectButton> hatSelectButtons, List<FaceSelectButton> faceSelectButtons)
	    {
	        _levelProgressTracker = levelProgressTracker;
	        _playerData = playerData;
	        _hatSelectButtons = hatSelectButtons;
	        _faceSelectButtons = faceSelectButtons;

	        _levelProgressTracker.LevelFinished += UpdateHatAviability;
	        _playerData.FaceUnlocked += UpdateFaceAviability;
	    }

		public event Action HatButtonUnlocked;
		public event Action FaceButtonUnlocked;

		private void UpdateFaceAviability(int faceId)
	    {
	        FaceSelectButton faceButton = _faceSelectButtons.FirstOrDefault(face => face.FaceId == faceId);
	        faceButton.SetUnlockedStyle();
	        faceButton.ActivateUnusedMark();
	        FaceButtonUnlocked?.Invoke();
	    }

	    private void UpdateHatAviability(ILevelData _)
	    {
	        foreach (HatSelectButton hatButton in _hatSelectButtons)
	        {
	            if (hatButton.RequredLevel < _playerData.LastAvailableLevelId)
	            {
	                hatButton.SetUnlockedStyle();

	                if (_playerData.IsHatUsed(hatButton.HatId) == false)
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
}
