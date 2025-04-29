using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Island : MonoBehaviour, ISelectable
{
    private List<PlacementPoint> _placementPoints;

    public void Initialize(List<PlacementPoint> placementPoints, Paint paint)
    {
        _placementPoints = placementPoints;
        Paint = paint;
    }

    public Paint Paint { get; private set; }

    public IEnumerable<Unit> GetUnits(Paint paint)
    {
        foreach (PlacementPoint point in _placementPoints)
        {
            if (point.IsOccupied && point.OccupiedUnit.Paint == paint)
            {
                yield return point.OccupiedUnit;
            }
        }
    }
}