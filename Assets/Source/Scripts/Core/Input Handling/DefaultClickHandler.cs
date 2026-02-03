using System;
using SlimeGround.Data;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Core.InputHandling
{
	public class DefaultClickHandler : ClickBehaviour, IUnitsSelectedEvent
	{
		private bool _isUnitsSelected;
	    private BaseIsland _selectedUnitsIsland;
	    private UnitSlotType _selectedUnitType;

	    private UnitMover _unitMover;
	    private UnitHighlighter _unitHighlighter;

	    public DefaultClickHandler(UnitMover unitMover, LayerMask layerMask) : base(layerMask)
	    {
	        _unitMover = unitMover;
	        _unitHighlighter = new UnitHighlighter();
	    }

		public event Action UnitsSelected;

		public bool IsUnitsSelected => _isUnitsSelected;
	    public BaseIsland SelectedUnitsIsland => _selectedUnitsIsland;
	    public UnitSlotType SelectedUnitType => _selectedUnitType;

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
	        if (IsUnitsSelected)
	        {
	            _unitHighlighter.UnhighlightUnits(SelectedUnitsIsland, SelectedUnitType);
	        }

			_isUnitsSelected = false;
	    }

	    private void SelectUnit(UnitCollider unitCollider)
	    {
	        ResetSelection();

			_isUnitsSelected = true;
	        _selectedUnitsIsland = unitCollider.Unit.Island;
	        _selectedUnitType = unitCollider.Unit.Slot;
	        _unitHighlighter.HighlightUnits(_selectedUnitsIsland, _selectedUnitType);
	        UnitsSelected?.Invoke();
	    }

	    private void SelectIsland(BaseIsland island)
	    {
	        if (_isUnitsSelected == false || island == SelectedUnitsIsland ||
	            island.FreePointsCount == 0)
	        {
	            return;
	        }

	        _unitHighlighter.UnhighlightUnits(SelectedUnitsIsland, SelectedUnitType);
	        _unitMover.MoveAllPossibleUnits(SelectedUnitsIsland, SelectedUnitType, island);
	        ResetSelection();
	    }
	}
}
