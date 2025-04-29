using System.Collections.Generic;

public class Island
{
    private readonly List<PlacementPoint> _placementPoints;

    public Island(List<PlacementPoint> placementPoints)
    {
        _placementPoints = placementPoints;
    }
}