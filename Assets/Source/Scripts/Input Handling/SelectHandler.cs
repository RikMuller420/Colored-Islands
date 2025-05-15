public class SelectHandler
{
    private SelectState _currentSelection;
    private BaseIsland _selectedIsland;
    private Paint _selectedPaint;

    private UnitHighlighter _unitHighlighter;

    private SelectIslandBehaviour _selectIslandBehaviour;
    private MoveUnitsBehaviour _moveUnitsBehaviour;
    private FinishIslandBehaviour _finishIslandBehaviour;

    public SelectHandler(UnitMover unitMover, BuferIslandsHolder buferIslands, LevelObjectsHolder levelDataHolder)
    {
        _unitHighlighter = new UnitHighlighter();
        _moveUnitsBehaviour = new MoveUnitsBehaviour(this, _unitHighlighter, unitMover);
        _finishIslandBehaviour = new FinishIslandBehaviour(this, levelDataHolder, buferIslands, unitMover);

        _selectIslandBehaviour = _moveUnitsBehaviour;
    }

    public SelectState CurrentSelection => _currentSelection;
    public BaseIsland SelectedIsland => _selectedIsland;
    public Paint SelectedPaint => _selectedPaint;

    public void Select(ISelectable selectable)
    {
        switch (selectable)
        {
            case Unit unit:
                SelectUnit(unit);
                break;

            case BaseIsland island:
                _selectIslandBehaviour.SelectIsland(island);
                break;
        }
    }

    public void ResetSelection()
    {
        _currentSelection = SelectState.None;
    }

    private void SelectUnit(Unit unit)
    {
        if (_currentSelection == SelectState.Units)
        {
            _unitHighlighter.UnhighlightUnits(_selectedIsland, _selectedPaint);
        }

        _currentSelection = SelectState.Units;
        _selectedIsland = unit.Island;
        _selectedPaint = unit.Paint;

        _unitHighlighter.HighlightUnits(_selectedIsland, _selectedPaint);
    }
}
