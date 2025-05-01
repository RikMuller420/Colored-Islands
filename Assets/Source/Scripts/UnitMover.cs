public class UnitMover
{
    public void SendUnitsToIsland(BaseIsland homeIsland, Paint paint, BaseIsland targetIsland)
    {
        foreach (Unit unit in homeIsland.GetUnits(paint))
        {
            if (targetIsland.FreePointsCount == 0)
            {
                break;
            }

            homeIsland.RemoveUnit(unit);
            targetIsland.AddUnit(unit, out PlacementPoint placementPoint);

            unit.SetIsland(targetIsland);
            unit.transform.position = placementPoint.Point.position;
        }
    }
}
