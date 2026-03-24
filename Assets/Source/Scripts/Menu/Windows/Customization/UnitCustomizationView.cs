using System.Linq;
using SlimeGround.Data;
using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.Paints;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Customization
{
	public class UnitCustomizationView : MonoBehaviour
	{
	    [SerializeField] private Image _body;
	    [SerializeField] private Image _face;
	    [SerializeField] private Image _hat;
		[SerializeField] private Image _hatOverlay;

		private ColorSampleMaterials _paintMaterials;
	    private UnitsFaceSettings _unitsFaceSettings;
	    private UnitsHatSettings _unitsHatSettings;

	    public void Initialize(ColorSampleMaterials paintMaterials, UnitsFaceSettings unitsFaceSettings,
	                           UnitsHatSettings unitsHatSettings)
	    {
	        _paintMaterials = paintMaterials;
	        _unitsFaceSettings = unitsFaceSettings;
	        _unitsHatSettings = unitsHatSettings;
	    }

	    public void SetColor(ColorSample colorSample)
	    {
	        ColorSampleMaterialData paintData = _paintMaterials.Materials.FirstOrDefault(paintData => paintData.ColorSample == colorSample);
	        _body.color = paintData.UnitUiColor;

	        float hatAlpha = _hat.color.a;
	        _hat.color = paintData.UnitUiHatColor;
	        SetHatAlpha(hatAlpha);
	    }

	    public void SetFace(int faceId)
	    {
	        UnitFaceData faceData = _unitsFaceSettings.Faces.FirstOrDefault(face => face.Id == faceId);
	        _face.sprite = faceData.Sprite;
	    }

	    public void SetHat(int hatId)
	    {
	        if (hatId == _unitsHatSettings.NoHatId)
	        {
	            _hat.sprite = null;
				_hatOverlay.sprite = null;
				SetHatAlpha(0f);
				SetHatOverlayAlpha(0f);
			}
			else
	        {
	            UnitHatData hatData = _unitsHatSettings.Hats.FirstOrDefault(hat => hat.Id == hatId);
	            _hat.sprite = hatData.PreviewSprite;
	            SetHatAlpha(1f);

				_hatOverlay.sprite = hatData.PreviewOverlaySprite;
				float hatOverlayAlpha = hatData.PreviewOverlaySprite == null ? 0 : 1;
				SetHatOverlayAlpha(hatOverlayAlpha);
			}
	    }

		private void SetHatAlpha(float alpha) => SetImageAlpha(_hat, alpha);

		private void SetHatOverlayAlpha(float alpha) => SetImageAlpha(_hatOverlay, alpha);

		private void SetImageAlpha(Image image, float alpha)
		{
			Color color = image.color;
			color.a = alpha;
			image.color = color;
		}
	}
}
