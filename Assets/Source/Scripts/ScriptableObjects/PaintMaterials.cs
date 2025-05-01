using UnityEngine;

[CreateAssetMenu(fileName = "PaintMaterials", menuName = "Custom/PaintMaterials")]
public class PaintMaterials : ScriptableObject
{
    [SerializeField] private PaintMaterialData[] materials;

    public PaintMaterialData[] Materials => materials;
}
