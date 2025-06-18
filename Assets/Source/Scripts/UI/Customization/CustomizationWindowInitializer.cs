using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomizationWindowInitializer : MonoBehaviour
{
    [SerializeField] private UnitsFaceSettings _unitsFaceSettings;
    [SerializeField] private PaintMaterials _paintMaterials;

    [SerializeField] private Transform _facesParent;
    [SerializeField] private FaceSelectButton _facePrefab;
    [SerializeField] private UnitCustomizationView _unitCustomizationView;

    [SerializeField] private List<UnitSelectButton> _unitSelectButtons = new();

    public void Initialize(GameProgressStorage progressStorage)
    {
        _unitCustomizationView.Initialize(_paintMaterials, _unitsFaceSettings);
        List<FaceSelectButton> faceSelectButtons = CreateFaceButtons(progressStorage);

        UnitCustomizator unitCustomizator = new UnitCustomizator(_unitCustomizationView, progressStorage,
                                                                _unitSelectButtons, faceSelectButtons);
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
