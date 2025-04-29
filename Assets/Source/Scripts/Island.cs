using System.Collections.Generic;

public class Island
{
    private readonly List<PlacementPoint> _placementPoints;

    public Island(List<PlacementPoint> placementPoints, Paint paint)
    {
        _placementPoints = placementPoints;
        Paint = paint;
    }

    public Paint Paint { get; }

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