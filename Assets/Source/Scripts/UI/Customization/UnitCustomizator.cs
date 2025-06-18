using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCustomizator
{
    private List<UnitSelectButton> _unitSelectButtons;
    private List<FaceSelectButton> _faceSelectButtons;
    private UnitCustomizationView _unitCustomizationView;
    private GameProgressStorage _progressStorage;

    private UnitSelectButton _currentUnitButton;
    private FaceSelectButton _currentFaceButton;

    private Paint _currentPaint;
    private int _currentFaceId;

    public UnitCustomizator(UnitCustomizationView unitCustomizationView, GameProgressStorage progressStorage,
                            List<UnitSelectButton> unitSelectButtons, List<FaceSelectButton> faceSelectButtons)
    {
        _unitCustomizationView = unitCustomizationView;
        _progressStorage = progressStorage;
        _unitSelectButtons = unitSelectButtons;
        _faceSelectButtons = faceSelectButtons;

        foreach (UnitSelectButton unitSelectButton in _unitSelectButtons)
        {
            unitSelectButton.ButtonClicked += ChangeCurrentPaint;
        }

        foreach (FaceSelectButton faceSelectButton in _faceSelectButtons)
        {
            faceSelectButton.ButtonClicked += ChangeCurrentFace;
        }

        //Подписатся на смену шапки

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

        //Загрузить из осхранения шапку
    }

    private void ChangeCurrentFace(FaceSelectButton faceButton)
    {
        ApplyNewFace(faceButton);
        _progressStorage.ChangeCustomizationPreferenceFace(_currentPaint, faceButton.FaceId);
        _progressStorage.Save();

        CustomizationPreferences customizationPreferences = _progressStorage.GetCustomizationPreference(_currentPaint);
    }

    private void ApplyNewFace(FaceSelectButton faceButton)
    {
        if (_currentFaceButton != null)
        {
            _currentFaceButton.SetNonSelectedStyle();
        }

        _currentFaceButton = faceButton;
        _currentFaceButton.SetSelectedStyle();
        _currentFaceId = faceButton.FaceId;
        _unitCustomizationView.SetFace(_currentFaceId);
    }
}
