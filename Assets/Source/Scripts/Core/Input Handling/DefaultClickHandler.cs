using System;
using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Core.InputHandling
{
	public class DefaultClickHandler : ClickBehaviour, IUnitsSelectedEvent
	{
	    private SelectState _currentSelection;
	    private BaseIsland _selectedIsland;
	    private UnitSlotType _selectedUnitSlot;

	    private UnitMover _unitMover;
	    private UnitHighlighter _unitHighlighter;

	    public DefaultClickHandler(UnitMover unitMover, LayerMask layerMask) : base(layerMask)
	    {
	        _unitMover = unitMover;
	        _unitHighlighter = new UnitHighlighter();
	    }

		public event Action UnitsSelected;

		public SelectState CurrentSelection => _currentSelection;
	    public BaseIsland SelectedIsland => _selectedIsland;
	    public UnitSlotType SelectedUnitSlot => _selectedUnitSlot;

	    public override void HandleClick(RaycastHit hit)
	    {
	        if (hit.collider.TryGetComponent(out ISelectable selectable))
	        {
	            Select(selectable);
	        }
	    }

	    public override void ResetBehaviour() => ResetSelection();

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

	    private void ResetSelection()
	    {
	        if (CurrentSelection == SelectState.Units)
	        {
	            _unitHighlighter.UnhighlightUnits(SelectedIsland, SelectedUnitSlot);
	        }

	        _currentSelection = SelectState.None;
	    }

	    private void SelectUnit(UnitCollider unitCollider)
	    {
	        ResetSelection();

	        _currentSelection = SelectState.Units;
	        _selectedIsland = unitCollider.Unit.Island;
	        _selectedUnitSlot = unitCollider.Unit.Slot;
	        _unitHighlighter.HighlightUnits(_selectedIsland, _selectedUnitSlot);
	        UnitsSelected?.Invoke();
	    }

	    private void SelectIsland(BaseIsland island)
	    {
	        if (CurrentSelection == SelectState.None || island == SelectedIsland ||
	            island.FreePointsCount == 0)
	        {
	            return;
	        }

	        _unitHighlighter.UnhighlightUnits(SelectedIsland, SelectedUnitSlot);
	        _unitMover.MoveAllPossibleUnits(SelectedIsland, SelectedUnitSlot, island);
	        ResetSelection();
	    }
	}
}
