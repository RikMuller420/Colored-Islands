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

    public void Initialize(PaintMaterials paintMaterials, UnitsFaceSettings unitsFaceSettings)
    {
        _paintMaterials = paintMaterials;
        _unitsFaceSettings = unitsFaceSettings;
    }

    public void SetPaint(Paint paint)
    {
        PaintMaterialData paintData = _paintMaterials.Materials.FirstOrDefault(paintData => paintData.Paint == paint);
        _body.color = paintData.UnitUiColor;
    }

    public void SetFace(int faceId)
    {
        UnitFaceData faceData = _unitsFaceSettings.Faces.FirstOrDefault(face => face.Id == faceId);
        _face.sprite = faceData.Sprite;
    }

    public void SetHat()
    {
        
    }
}
