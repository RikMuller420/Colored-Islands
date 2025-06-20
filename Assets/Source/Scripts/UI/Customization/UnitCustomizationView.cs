using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UnitCustomizationView : MonoBehaviour
{
    [SerializeField] private Image _body;
    [SerializeField] private Image _face;
    [SerializeField] private Image _hat;

    private PaintMaterials _paintMaterials;
    private UnitsFaceSettings _unitsFaceSettings;
    private UnitsHatSettings _unitsHatSettings;

    public void Initialize(PaintMaterials paintMaterials, UnitsFaceSettings unitsFaceSettings, UnitsHatSettings unitsHatSettings)
    {
        _paintMaterials = paintMaterials;
        _unitsFaceSettings = unitsFaceSettings;
        _unitsHatSettings = unitsHatSettings;
    }

    public void SetPaint(Paint paint)
    {
        PaintMaterialData paintData = _paintMaterials.Materials.FirstOrDefault(paintData => paintData.Paint == paint);
        _body.color = paintData.UnitUiColor;
        _hat.color = paintData.UnitUiHatColor;
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
            SetHatColor(0f);
        }
        else
        {
            UnitHatData hatData = _unitsHatSettings.Hats.FirstOrDefault(hat => hat.Id == hatId);
            _hat.sprite = hatData.PreviewSprite;
            SetHatColor(1f);
        }
    }

    private void SetHatColor(float alpha)
    {
        Color color = _hat.color;
        color.a = alpha;
        _hat.color = color;
    }
}
