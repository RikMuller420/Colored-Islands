using System;
using System.Collections.Generic;
using SlimeGround.Data;
using SlimeGround.Data.Saves;

namespace SlimeGround.Menu.Windows.Customization
{
	public class UnitCustomizator
	{
	    private List<UnitSelectButton> _unitSelectButtons;
	    private List<HatSelectButton> _hatSelectButtons;
	    private List<FaceSelectButton> _faceSelectButtons;
	    private List<ColorSelectButton> _colorSelectButtons;
	    private UnitCustomizationView _unitCustomizationView;
	    private PlayerDataProvider _playerData;

	    private UnitSelectButton _currentUnitButton;
	    private FaceSelectButton _currentFaceButton;
	    private HatSelectButton _currentHatButton;
	    private ColorSelectButton _currentColorButton;

	    private UnitSlotType _currentSlot;

	    public UnitCustomizator(UnitCustomizationView unitCustomizationView, PlayerDataProvider playerData,
	                            List<UnitSelectButton> unitSelectButtons, List<HatSelectButton> hatSelectButtons,
	                            List<FaceSelectButton> faceSelectButtons, List<ColorSelectButton> colorSelectButtons)
	    {
	        _unitCustomizationView = unitCustomizationView;
	        _playerData = playerData;
	        _unitSelectButtons = unitSelectButtons;
	        _faceSelectButtons = faceSelectButtons;
	        _hatSelectButtons = hatSelectButtons;
	        _colorSelectButtons = colorSelectButtons;

	        foreach (UnitSelectButton unitSelectButton in _unitSelectButtons)
	        {
	            unitSelectButton.ButtonClicked += ChangeCurrentPaint;
	            CustomizationPreferences preferences = _playerData.GetCustomizationPreference(unitSelectButton.Slot);
	            unitSelectButton.SetColor(preferences.ColorSample);
	        }

	        foreach (FaceSelectButton faceSelectButton in _faceSelectButtons)
	        {
	            faceSelectButton.ButtonClicked += ChangeCurrentFace;
	        }

	        foreach (HatSelectButton hatSelectButton in _hatSelectButtons)
	        {
	            hatSelectButton.ButtonClicked += ChangeCurrentHat;
	        }

	        foreach (ColorSelectButton colorSelectButton in _colorSelectButtons)
	        {
	            colorSelectButton.ButtonClicked += ChangeCurrentColor;
	        }

	        ChangeCurrentPaint(_unitSelectButtons[0]);
	    }

		public event Action FaceUsed;
		public event Action HatUsed;

		public IEnumerable<FaceSelectButton> FaceSelectButtons => _faceSelectButtons;
		public IEnumerable<HatSelectButton> HatSelectButtons => _hatSelectButtons;

		public void Dispose()
		{
			foreach (UnitSelectButton unitSelectButton in _unitSelectButtons)
			{
				unitSelectButton.ButtonClicked -= ChangeCurrentPaint;
			}

			foreach (FaceSelectButton faceSelectButton in _faceSelectButtons)
			{
				faceSelectButton.ButtonClicked -= ChangeCurrentFace;
			}

			foreach (HatSelectButton hatSelectButton in _hatSelectButtons)
			{
				hatSelectButton.ButtonClicked -= ChangeCurrentHat;
			}

			foreach (ColorSelectButton colorSelectButton in _colorSelectButtons)
			{
				colorSelectButton.ButtonClicked -= ChangeCurrentColor;
			}
		}

		private void ChangeCurrentPaint(UnitSelectButton button)
	    {
	        if (_currentUnitButton != null)
	        {
	            _currentUnitButton.SetNonSelectedStyle();
	        }

	        _currentSlot = button.Slot;
	        _currentUnitButton = button;
	        _currentUnitButton.SetSelectdStyle();

	        CustomizationPreferences preferences = _playerData.GetCustomizationPreference(_currentSlot);
	        _unitCustomizationView.SetColor(preferences.ColorSample);

	        FaceSelectButton faceSelectButton = _faceSelectButtons.Find(button => button.FaceId == preferences.FaceId);
	        ApplyNewFace(faceSelectButton);

	        HatSelectButton hatSelectButton = _hatSelectButtons.Find(button => button.HatId == preferences.HatId);
	        ApplyNewHat(hatSelectButton);

	        foreach (ColorSelectButton colorButton in _colorSelectButtons)
	        {
	            if (colorButton.ColorSample == preferences.ColorSample)
	            {
	                colorButton.SetSelectedStyle();
	                colorButton.SetUnlockedStyle();
	            }
	            else
	            {
	                colorButton.SetNonSelectedStyle();
	                bool isColorSampleFree = IsColofSampleFree(colorButton.ColorSample);

	                if (isColorSampleFree)
	                {
	                    colorButton.SetUnlockedStyle();
	                }
	                else
	                {
	                    colorButton.SetLockedStyle();
	                }
	            }
	        }

	        ColorSelectButton colorSelectButton = _colorSelectButtons.Find(button => button.ColorSample == preferences.ColorSample);
	        ApplyNewColor(colorSelectButton);
	    }

	    private bool IsColofSampleFree(ColorSample colorSample)
	    {
	        foreach (UnitSlotType slot in (UnitSlotType[])Enum.GetValues(typeof(UnitSlotType)))
	        {
	            CustomizationPreferences preferences = _playerData.GetCustomizationPreference(slot);

	            if (preferences.ColorSample == colorSample)
	            {
	                return false;
	            }
	        }

	        return true;
	    }

	    private void ChangeCurrentFace(FaceSelectButton faceButton)
	    {
	        ApplyNewFace(faceButton);
	        faceButton.DeactivateUnusedMark();
	        _playerData.ChangeCustomizationPreferenceFace(_currentSlot, faceButton.FaceId);
	        _playerData.MarkFaceUsed(faceButton.FaceId);
	        _playerData.Save();
	        FaceUsed?.Invoke();
	    }

	    private void ChangeCurrentHat(HatSelectButton hatButton)
	    {
	        ApplyNewHat(hatButton);
	        hatButton.DeactivateUnusedMark();
	        _playerData.ChangeCustomizationPreferenceHat(_currentSlot, hatButton.HatId);
	        _playerData.MarkHatUsed(hatButton.HatId);
	        _playerData.Save();
	        HatUsed?.Invoke();
	    }

	    private void ChangeCurrentColor(ColorSelectButton colorButton)
	    {
	        ApplyNewColor(colorButton);
	        _playerData.ChangeCustomizationPreferenceColor(_currentSlot, colorButton.ColorSample);
	        _playerData.Save();
	    }

	    private void ApplyNewColor(ColorSelectButton colorButton)
	    {
	        if (_currentColorButton != null)
	        {
	            _currentColorButton.SetNonSelectedStyle();
	        }

	        if (_currentUnitButton != null)
	        {
	            _currentUnitButton.SetColor(colorButton.ColorSample);
	        }

	        _currentColorButton = colorButton;
	        _currentColorButton.SetSelectedStyle();
	        _unitCustomizationView.SetColor(colorButton.ColorSample);
	    }

	    private void ApplyNewFace(FaceSelectButton faceButton)
	    {
	        if (_currentFaceButton != null)
	        {
	            _currentFaceButton.SetNonSelectedStyle();
	        }

	        _currentFaceButton = faceButton;
	        _currentFaceButton.SetSelectedStyle();
	        _unitCustomizationView.SetFace(faceButton.FaceId);
	    }

	    private void ApplyNewHat(HatSelectButton hatButton)
	    {
	        if (_currentHatButton != null)
	        {
	            _currentHatButton.SetNonSelectedStyle();
	        }

	        _currentHatButton = hatButton;
	        _currentHatButton.SetSelectedStyle();
	        _unitCustomizationView.SetHat(hatButton.HatId);
	    }
	}
}
