using System;
using System.Collections.Generic;
using UI.TabSystem;
using UnityEngine;

public class UnitCustomizator
{
    private List<UnitSelectButton> _unitSelectButtons;
    private List<HatSelectButton> _hatSelectButtons;
    private List<FaceSelectButton> _faceSelectButtons;
    private List<ColorSelectButton> _colorSelectButtons;
    private UnitCustomizationView _unitCustomizationView;
    private GameProgressStorage _progressStorage;

    private UnitSelectButton _currentUnitButton;
    private FaceSelectButton _currentFaceButton;
    private HatSelectButton _currentHatButton;
    private ColorSelectButton _currentColorButton;

    private Paint _currentPaint;

    public IEnumerable<FaceSelectButton> FaceSelectButtons => _faceSelectButtons;
    public IEnumerable<HatSelectButton> HatSelectButtons => _hatSelectButtons;

    public event Action FaceUsed;
    public event Action HatUsed;

    public UnitCustomizator(UnitCustomizationView unitCustomizationView, GameProgressStorage progressStorage,
                            List<UnitSelectButton> unitSelectButtons, List<HatSelectButton> hatSelectButtons,
                            List<FaceSelectButton> faceSelectButtons, List<ColorSelectButton> colorSelectButtons)
    {
        _unitCustomizationView = unitCustomizationView;
        _progressStorage = progressStorage;
        _unitSelectButtons = unitSelectButtons;
        _faceSelectButtons = faceSelectButtons;
        _hatSelectButtons = hatSelectButtons;
        _colorSelectButtons = colorSelectButtons;

        foreach (UnitSelectButton unitSelectButton in _unitSelectButtons)
        {
            unitSelectButton.ButtonClicked += ChangeCurrentPaint;
            CustomizationPreferences preferences = _progressStorage.GetCustomizationPreference(unitSelectButton.Paint);
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

    private void ChangeCurrentPaint(UnitSelectButton button)
    {
        if (_currentUnitButton != null)
        {
            _currentUnitButton.SetNonSelectedStyle();
        }

        _currentPaint = button.Paint;
        _currentUnitButton = button;
        _currentUnitButton.SetSelectdStyle();

        CustomizationPreferences preferences = _progressStorage.GetCustomizationPreference(_currentPaint);
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
        foreach (Paint paint in (Paint[])Enum.GetValues(typeof(Paint)))
        {
            CustomizationPreferences preferences = _progressStorage.GetCustomizationPreference(paint);

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
        _progressStorage.ChangeCustomizationPreferenceFace(_currentPaint, faceButton.FaceId);
        _progressStorage.MarkFaceUsed(faceButton.FaceId);
        _progressStorage.Save();
        FaceUsed?.Invoke();
    }

    private void ChangeCurrentHat(HatSelectButton hatButton)
    {
        ApplyNewHat(hatButton);
        hatButton.DeactivateUnusedMark();
        _progressStorage.ChangeCustomizationPreferenceHat(_currentPaint, hatButton.HatId);
        _progressStorage.MarkHatUsed(hatButton.HatId);
        _progressStorage.Save();
        HatUsed?.Invoke();
    }

    private void ChangeCurrentColor(ColorSelectButton colorButton)
    {
        ApplyNewColor(colorButton);
        _progressStorage.ChangeCustomizationPreferenceColor(_currentPaint, colorButton.ColorSample);
        _progressStorage.Save();
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
