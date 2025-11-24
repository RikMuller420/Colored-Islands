using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationWindowInitializer : MonoBehaviour
{
    [SerializeField] private UnitsFaceSettings _unitsFaceSettings;
    [SerializeField] private UnitsHatSettings _unitsHatSettings;
    [SerializeField] private PaintMaterials _paintMaterials;

    [SerializeField] private Sprite _noHatSprite;

    [SerializeField] private Transform _facesParent;
    [SerializeField] private FaceSelectButton _facePrefab;
    [SerializeField] private Transform _colorButtonParent;
    [SerializeField] private ColorSelectButton _colorButtonPrefab;
    [SerializeField] private Transform _hatParent;
    [SerializeField] private HatSelectButton _hatPrefab;
    [SerializeField] private UnitCustomizationView _unitCustomizationView;
    [SerializeField] private CustomizationWindowOpenerButton _customizationWindowOpenerButton;

    [SerializeField] private List<UnitSelectButton> _unitSelectButtons = new();

    public void Initialize(GameProgressStorage progressStorage, LevelProgressTracker levelProgressTracker)
    {
        _unitCustomizationView.Initialize(_paintMaterials, _unitsFaceSettings, _unitsHatSettings);
        List<ColorSelectButton> colorSelectButtons = CreateColorButtons(progressStorage);
        List<HatSelectButton> hatSelectButtons = CreateHatButtons(progressStorage);
        List<FaceSelectButton> faceSelectButtons = CreateFaceButtons(progressStorage);

        UnitCustomizator unitCustomizator = new UnitCustomizator(_unitCustomizationView, progressStorage,
                                                _unitSelectButtons, hatSelectButtons, faceSelectButtons, colorSelectButtons);

        CustomizationButtonAviabiltyUpdater buttonAviabiltyUpdater = new CustomizationButtonAviabiltyUpdater(
                                                levelProgressTracker, progressStorage, hatSelectButtons, faceSelectButtons);

        _customizationWindowOpenerButton.Initialize(unitCustomizator, buttonAviabiltyUpdater);
    }

    private List<ColorSelectButton> CreateColorButtons(GameProgressStorage progressStorage)
    {
        List<ColorSelectButton> colorSelectButtons = new List<ColorSelectButton>();

        foreach (PaintMaterialData material in _paintMaterials.Materials)
        {
            ColorSelectButton colorButton = Instantiate(_colorButtonPrefab, _colorButtonParent);
            colorButton.Initialize(material);
            colorSelectButtons.Add(colorButton);
        }

        return colorSelectButtons;
    }

    private List<HatSelectButton> CreateHatButtons(GameProgressStorage progressStorage)
    {
        List<HatSelectButton> hatSelectButtons = new List<HatSelectButton>();

        HatSelectButton noHatButton = Instantiate(_hatPrefab, _hatParent);
        noHatButton.Initialize(_unitsHatSettings.NoHatId, _noHatSprite, 0, true, true);
        hatSelectButtons.Add(noHatButton);

        foreach (UnitHatData hatData in _unitsHatSettings.Hats)
        {
            HatSelectButton hatButton = Instantiate(_hatPrefab, _hatParent);
            bool isHatAviable = progressStorage.LastAvailableLevelId > hatData.RequredLevel;
            bool wasHatUsed = progressStorage.WasHatUsed(hatData.Id);
            hatButton.Initialize(hatData.Id, hatData.SelectSprite, hatData.RequredLevel, isHatAviable, wasHatUsed);
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
            FaceAvailabilitie face = progressStorage.FaceAvailabilities.FirstOrDefault(face => face.FaceId == faceData.Id);
            faceButton.Initialize(faceData.Id, faceData.Sprite, face.IsAviable, face.WasUsed);
            faceSelectButtons.Add(faceButton);
        }

        return faceSelectButtons;
    }
}
