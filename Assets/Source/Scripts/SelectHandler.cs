public class SelectHandler
{
    private bool _isSelected;
    private Island _selectedIsland;
    private Paint _selectedPaint;

    private UnitHighlighter _unitHighlighter;

    public SelectHandler(UnitHighlighter unitHighlighter)
    {
        _unitHighlighter = unitHighlighter;
    }

    public void Select(ISelectable selectable)
    {
        switch (selectable)
        {
            case Unit unit:
                SelectUnit(unit);
                break;

            case Island island:
                SelectIsland(island);
                break;
        }
    }

    private void SelectUnit(Unit unit)
    {
        _isSelected = true;
        _selectedIsland = unit.Island;
        _selectedPaint = unit.Paint;

        _unitHighlighter.HighlightUnits(_selectedIsland, _selectedPaint);
    }

    private void SelectIsland(Island island)
    {
        if (_isSelected == false || island == _selectedIsland)
        {
            return;
        }

        _unitHighlighter.UnhighlightUnits(_selectedIsland, _selectedPaint);
        //SendUnitsToIsland(island);
    }
}
