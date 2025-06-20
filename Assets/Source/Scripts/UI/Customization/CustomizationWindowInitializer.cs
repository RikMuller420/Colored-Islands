using System.Collections.Generic;
using UnityEngine;

public class CustomizationWindowInitializer : MonoBehaviour
{
    [SerializeField] private UnitsFaceSettings _unitsFaceSettings;
    [SerializeField] private UnitsHatSettings _unitsHatSettings;
    [SerializeField] private PaintMaterials _paintMaterials;

    [SerializeField] private Sprite _noHatSprite;

    [SerializeField] private Transform _facesParent;
    [SerializeField] private FaceSelectButton _facePrefab;
    [SerializeField] private Transform _hatParent;
    [SerializeField] private HatSelectButton _hatPrefab;
    [SerializeField] private UnitCustomizationView _unitCustomizationView;

    [SerializeField] private List<UnitSelectButton> _unitSelectButtons = new();

    public void Initialize(GameProgressStorage progressStorage, LevelProgressTracker levelProgressTracker)
    {
        _unitCustomizationView.Initialize(_paintMaterials, _unitsFaceSettings, _unitsHatSettings);
        List<FaceSelectButton> faceSelectButtons = CreateFaceButtons(progressStorage);
        List<HatSelectButton> hatSelectButtons = CreateHatButtons(progressStorage);

        UnitCustomizator unitCustomizator = new UnitCustomizator(_unitCustomizationView, progressStorage,
                                                _unitSelectButtons, hatSelectButtons, faceSelectButtons);

        CustomizationButtonAviabiltyUpdater buttonAviabiltyUpdater = new CustomizationButtonAviabiltyUpdater(
                                                levelProgressTracker, progressStorage, hatSelectButtons, faceSelectButtons);
    }

    private List<HatSelectButton> CreateHatButtons(GameProgressStorage progressStorage)
    {
        List<HatSelectButton> hatSelectButtons = new List<HatSelectButton>();

        HatSelectButton noHatButton = Instantiate(_hatPrefab, _hatParent);
        noHatButton.Initialize(_unitsHatSettings.NoHatId, _noHatSprite, 0, true);
        hatSelectButtons.Add(noHatButton);

        foreach (UnitHatData hatData in _unitsHatSettings.Hats)
        {
            HatSelectButton hatButton = Instantiate(_hatPrefab, _hatParent);
            bool isHatAviable = progressStorage.FirstUnfinishedLevel.Id > hatData.RequredLevel;

            Debug.Log(progressStorage.FirstUnfinishedLevel.Id + "   " + hatData.RequredLevel);

            hatButton.Initialize(hatData.Id, hatData.SelectSprite, hatData.RequredLevel, isHatAviable);
            hatSelectButtons.Add(hatButton);
        }

        return hatSelectButtons;
    }

    private List<FaceSelectButton> CreateFaceButtons(GameProgressStorage progressStorage)
    {
        List<FaceSelectButton> faceSelectButtons = new List<FaceSelectButton>();

        foreach (UnitFaceData faceData in _unitsFaceSettings.Faces)
        {
            FaceSelectButton faceButton = Instantiate(_facePrefab, _facesParent);
            bool isFaceAviable = progressStorage.FacesAvailabilities[faceData.Id];
            faceButton.Initialize(faceData.Id, faceData.Sprite, isFaceAviable);
            faceSelectButtons.Add(faceButton);
        }

        return faceSelectButtons;
    }
}
