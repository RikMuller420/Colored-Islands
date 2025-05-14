public class MoveUnitsBehaviour : SelectIslandBehaviour
{
    private SelectHandler _selectHandler;
    private UnitHighlighter _unitHighlighter;
    private UnitMover _unitMover;

    public MoveUnitsBehaviour(SelectHandler selectHandler, UnitHighlighter unitHighlighter, UnitMover unitMover)
    {
        _selectHandler = selectHandler;
        _unitHighlighter = unitHighlighter;
        _unitMover = unitMover;
    }

    public void SelectIsland(BaseIsland island)
    {
        if (_selectHandler.CurrentSelection == SelectState.None || island == _selectHandler.SelectedIsland ||
            island.FreePointsCount == 0)
        {
            return;
        }

        _unitHighlighter.UnhighlightUnits(_selectHandler.SelectedIsland, _selectHandler.SelectedPaint);
        _unitMover.MoveAllPossibleUnits(_selectHandler.SelectedIsland, _selectHandler.SelectedPaint, island);
        _selectHandler.ResetSelection();
    }
}
