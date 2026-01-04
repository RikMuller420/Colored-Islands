using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitsFaceSettings", menuName = "Custom/UnitsFaceSettings")]
public class UnitsFaceSettings : ScriptableObject
{
    [SerializeField] private UnitFaceData[] _faces;

    public IReadOnlyCollection<UnitFaceData> Faces => _faces;
}
