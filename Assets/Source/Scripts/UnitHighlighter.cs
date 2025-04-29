public class UnitHighlighter
{
    public void HighlightUnits(Island island, Paint paint)
    {
        foreach (Unit unit in island.GetUnits(paint))
        {
            unit.ActivateOutline();
        }
    }

    public void UnhighlightUnits(Island island, Paint paint)
    {
        foreach (Unit unit in island.GetUnits(paint))
        {
            unit.DeactivateOutline();
        }
    }
}
