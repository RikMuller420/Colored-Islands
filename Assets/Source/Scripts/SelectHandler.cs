public class SelectHandler
{
    private bool _isUnitsSelected;
    private Island _selectedIsland;
    private Paint _selectedPaint;

    private UnitHighlighter _unitHighlighter;
    private UnitMover _unitMover;

    public SelectHandler(UnitHighlighter unitHighlighter, UnitMover unitMover)
    {
        _unitHighlighter = unitHighlighter;
        _unitMover = unitMover;
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
        if (_isUnitsSelected)
        {
            _unitHighlighter.UnhighlightUnits(_selectedIsland, _selectedPaint);
        }

        _isUnitsSelected = true;
        _selectedIsland = unit.Island;
        _selectedPaint = unit.Paint;

        _unitHighlighter.HighlightUnits(_selectedIsland, _selectedPaint);
    }

    private void SelectIsland(Island island)
    {
        if (_isUnitsSelected == false || island == _selectedIsland ||
            island.FreePointsCount == 0)
        {
            return;
        }

        _unitHighlighter.UnhighlightUnits(_selectedIsland, _selectedPaint);
        _unitMover.SendUnitsToIsland(_selectedIsland, _selectedPaint, island);
        _isUnitsSelected = false;
    }
}
