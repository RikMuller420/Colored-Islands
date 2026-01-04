public class UnitHighlighter
{
    public void HighlightUnits(BaseIsland island, Paint paint)
    {
        foreach (Unit unit in island.GetUnits(paint))
        {
            unit.ActivateOutline();
        }
    }

    public void UnhighlightUnits(BaseIsland island, Paint paint)
    {
        foreach (Unit unit in island.GetUnits(paint))
        {
            unit.DeactivateOutline();
        }
    }
}
