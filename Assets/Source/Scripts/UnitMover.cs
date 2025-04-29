public class UnitMover
{
    public void SendUnitsToIsland(Island homeIsland, Paint paint, Island target)
    {
        foreach (Unit unit in homeIsland.GetUnits(paint))
        {
            //Send to target
        }
    }
}
