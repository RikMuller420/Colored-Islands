using System.Collections.Generic;

public class UnitCustomizator
{
    private List<UnitSelectButton> _unitSelectButtons;
    private List<HatSelectButton> _hatSelectButtons;
    private List<FaceSelectButton> _faceSelectButtons;
    private UnitCustomizationView _unitCustomizationView;
    private GameProgressStorage _progressStorage;

    private UnitSelectButton _currentUnitButton;
    private FaceSelectButton _currentFaceButton;
    private HatSelectButton _currentHatButton;

    private Paint _currentPaint;

    public UnitCustomizator(UnitCustomizationView unitCustomizationView, GameProgressStorage progressStorage,
                            List<UnitSelectButton> unitSelectButtons, List<HatSelectButton> hatSelectButtons,
                            List<FaceSelectButton> faceSelectButtons)
    {
        _unitCustomizationView = unitCustomizationView;
        _progressStorage = progressStorage;
        _unitSelectButtons = unitSelectButtons;
        _faceSelectButtons = faceSelectButtons;
        _hatSelectButtons = hatSelectButtons;

        foreach (UnitSelectButton unitSelectButton in _unitSelectButtons)
        {
            unitSelectButton.ButtonClicked += ChangeCurrentPaint;
        }

        foreach (FaceSelectButton faceSelectButton in _faceSelectButtons)
        {
            faceSelectButton.ButtonClicked += ChangeCurrentFace;
        }

        foreach (HatSelectButton hatSelectButton in _hatSelectButtons)
        {
            hatSelectButton.ButtonClicked += ChangeCurrentHat;
        }

        ChangeCurrentPaint(_unitSelectButtons[0]);
    }

    private void ChangeCurrentPaint(UnitSelectButton button)
    {
        if (_currentUnitButton != null)
        {
            _currentUnitButton.SetNonSelectedStyle();
        }

        _currentPaint = button.Paint;
        _currentUnitButton = button;
        _currentUnitButton.SetSelectdStyle();

        _unitCustomizationView.SetPaint(_currentPaint);
        CustomizationPreferences customizationPreferences = _progressStorage.GetCustomizationPreference(_currentPaint);

        FaceSelectButton faceSelectButton = _faceSelectButtons.Find(button => button.FaceId == customizationPreferences.FaceId);
        ApplyNewFace(faceSelectButton);

        HatSelectButton hatSelectButton = _hatSelectButtons.Find(button => button.HatId == customizationPreferences.HatId);
        AplyNewHat(hatSelectButton);
    }

    private void ChangeCurrentFace(FaceSelectButton faceButton)
    {
        ApplyNewFace(faceButton);
        _progressStorage.ChangeCustomizationPreferenceFace(_currentPaint, faceButton.FaceId);
        _progressStorage.Save();
    }

    private void ChangeCurrentHat(HatSelectButton hatButton)
    {
        AplyNewHat(hatButton);
        _progressStorage.ChangeCustomizationPreferenceHat(_currentPaint, hatButton.HatId);
        _progressStorage.Save();
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

    private void AplyNewHat(HatSelectButton hatButton)
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
