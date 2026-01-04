using System;

public class SelectHandler
{
    private SelectState _currentSelection;
    private BaseIsland _selectedIsland;
    private Paint _selectedPaint;

    private UnitMover _unitMover;
    private UnitHighlighter _unitHighlighter;

    public event Action UnitsSelected;

    public SelectHandler(UnitMover unitMover, BuferIslandsHolder buferIslands, LevelObjectsHolder levelDataHolder)
    {
        _unitHighlighter = new UnitHighlighter();
        _unitMover = unitMover;
    }

    public SelectState CurrentSelection => _currentSelection;
    public BaseIsland SelectedIsland => _selectedIsland;
    public Paint SelectedPaint => _selectedPaint;

    public void Select(ISelectable selectable)
    {
        switch (selectable)
        {
            case UnitCollider unitCollider:
                SelectUnit(unitCollider);
                break;

            case BaseIsland island:
                SelectIsland(island);
                break;
        }
    }

    public void ResetSelection()
    {
        if (CurrentSelection == SelectState.Units)
        {
            _unitHighlighter.UnhighlightUnits(SelectedIsland, SelectedPaint);
        }

        _currentSelection = SelectState.None;
    }

    private void SelectUnit(UnitCollider unitCollider)
    {
        ResetSelection();

        _currentSelection = SelectState.Units;
        _selectedIsland = unitCollider.Unit.Island;
        _selectedPaint = unitCollider.Unit.Paint;
        _unitHighlighter.HighlightUnits(_selectedIsland, _selectedPaint);
        UnitsSelected?.Invoke();
    }

    private void SelectIsland(BaseIsland island)
    {
        if (CurrentSelection == SelectState.None || island == SelectedIsland ||
            island.FreePointsCount == 0)
        {
            return;
        }

        _unitHighlighter.UnhighlightUnits(SelectedIsland, SelectedPaint);
        _unitMover.MoveAllPossibleUnits(SelectedIsland, SelectedPaint, island);
        ResetSelection();
    }
}
