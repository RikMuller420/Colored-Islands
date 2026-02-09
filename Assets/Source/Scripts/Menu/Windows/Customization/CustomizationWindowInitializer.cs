using System.Collections.Generic;
using System.Linq;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.Paints;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using SlimeGround.Gameplay.Levels;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Customization
{
	public class CustomizationWindowInitializer : MonoBehaviour
	{
	    [SerializeField] private PlayerDataProvider _playerData;
	    [SerializeField] private LevelProgressTracker _levelProgressTracker;

	    [SerializeField] private UnitsFaceSettings _unitsFaceSettings;
	    [SerializeField] private UnitsHatSettings _unitsHatSettings;
	    [SerializeField] private ColorSampleMaterials _paintMaterials;

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

		private UnitCustomizator _unitCustomizator;
		private CustomizationButtonAviabiltyUpdater _buttonAviabiltyUpdater;

		public void Initialize()
	    {
	        _unitCustomizationView.Initialize(_paintMaterials, _unitsFaceSettings, _unitsHatSettings);
	        List<ColorSelectButton> colorSelectButtons = CreateColorButtons(_playerData);
	        List<HatSelectButton> hatSelectButtons = CreateHatButtons(_playerData);
	        List<FaceSelectButton> faceSelectButtons = CreateFaceButtons(_playerData);

			_unitCustomizator = new UnitCustomizator(_unitCustomizationView, _playerData,
	                                                 _unitSelectButtons, hatSelectButtons,
													 faceSelectButtons, colorSelectButtons);

			_buttonAviabiltyUpdater = new CustomizationButtonAviabiltyUpdater(_levelProgressTracker,
													_playerData, hatSelectButtons, faceSelectButtons);

	        _customizationWindowOpenerButton.Initialize(_unitCustomizator, _buttonAviabiltyUpdater);
	    }

		public void Dispose()
		{
			_unitCustomizator.Dispose();
			_buttonAviabiltyUpdater.Dispose();
			_customizationWindowOpenerButton.Dispose();
		}

	    private List<ColorSelectButton> CreateColorButtons(IPlayerData playerData)
	    {
	        List<ColorSelectButton> colorSelectButtons = new List<ColorSelectButton>();

	        foreach (ColorSampleMaterialData material in _paintMaterials.Materials)
	        {
	            ColorSelectButton colorButton = Instantiate(_colorButtonPrefab, _colorButtonParent);
	            colorButton.Initialize(material);
	            colorSelectButtons.Add(colorButton);
	        }

	        return colorSelectButtons;
	    }

	    private List<HatSelectButton> CreateHatButtons(IPlayerData playerData)
	    {
	        List<HatSelectButton> hatSelectButtons = new List<HatSelectButton>();

	        HatSelectButton noHatButton = Instantiate(_hatPrefab, _hatParent);
	        noHatButton.Initialize(_unitsHatSettings.NoHatId, _noHatSprite, 0, true, true);
	        hatSelectButtons.Add(noHatButton);

	        foreach (UnitHatData hatData in _unitsHatSettings.Hats)
	        {
	            HatSelectButton hatButton = Instantiate(_hatPrefab, _hatParent);
	            bool isHatAviable = playerData.LastAvailableLevelId > hatData.RequredLevel;
	            bool wasHatUsed = playerData.IsHatUsed(hatData.Id);
	            hatButton.Initialize(hatData.Id, hatData.SelectSprite, hatData.RequredLevel, isHatAviable, wasHatUsed);
	            hatSelectButtons.Add(hatButton);
	        }

	        return hatSelectButtons;
	    }

	    private List<FaceSelectButton> CreateFaceButtons(IPlayerData playerData)
	    {
	        List<FaceSelectButton> faceSelectButtons = new List<FaceSelectButton>();

	        foreach (UnitFaceData faceData in _unitsFaceSettings.Faces)
	        {
	            FaceSelectButton faceButton = Instantiate(_facePrefab, _facesParent);
	            FaceAvailabilitie face = playerData.FaceAvailabilities.FirstOrDefault(face => face.FaceId == faceData.Id);
	            faceButton.Initialize(faceData.Id, faceData.Sprite, face.IsAviable, face.WasUsed);
	            faceSelectButtons.Add(faceButton);
	        }

	        return faceSelectButtons;
	    }
	}
}
