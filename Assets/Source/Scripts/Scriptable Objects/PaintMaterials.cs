using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PaintMaterials", menuName = "Custom/PaintMaterials")]
public class PaintMaterials : ScriptableObject
{
    [SerializeField] private PaintMaterialData[] _materials;

    public IReadOnlyCollection<PaintMaterialData> Materials => new ReadOnlyCollection<PaintMaterialData>(_materials);
}
