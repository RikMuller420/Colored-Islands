using System;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Gameplay.Units;
using UnityEngine;

namespace SlimeGround.Core.InputHandling
{
	public class GameplayClickHandler : ClickBehaviour, IUnitsSelectedEvent
	{
		private Unit _selectedUnit;

		private UnitMover _unitMover;
	    private UnitHighlighter _unitHighlighter;

	    public GameplayClickHandler(UnitMover unitMover, LayerMask layerMask) : base(layerMask)
	    {
	        _unitMover = unitMover;
	        _unitHighlighter = new UnitHighlighter();
	    }

		public event Action UnitsSelected;

	    public override void HandleClick(RaycastHit hit)
	    {
	        if (hit.collider.TryGetComponent(out ISelectable selectable))
	        {
	            Select(selectable);
	        }
	    }

	    public override void ResetBehaviour() => ResetSelection();

		private void Select(ISelectable selectable)
	    {
	        switch (selectable)
	        {
	            case UnitCollider unitCollider:
	                SelectUnit(unitCollider.Unit);
	                break;

	            case BaseIsland island:
	                SelectIsland(island);
	                break;
	        }
	    }

	    private void SelectUnit(Unit unit)
	    {
			if (TryRedirectToIsland(unit))
			{
				return;
			}

			if (IsSameAsSelected(unit))
			{
				ResetSelection();

				return;
			}

			ResetSelection();
			_selectedUnit = unit;
	        _unitHighlighter.HighlightUnits(_selectedUnit.Island, _selectedUnit.Slot);
	        UnitsSelected?.Invoke();
	    }

		private bool TryRedirectToIsland(Unit unit)
		{
			if (IsUnitsSelected &&
				unit.Island != _selectedUnit.Island &&
				unit.Island.FreePointsCount > 0)
			{
				SelectIsland(unit.Island);

				return true;
			}

			return false;
		}

	    private void SelectIsland(BaseIsland island)
	    {
	        if (IsUnitsSelected == false || island == _selectedUnit.Island ||
	            island.FreePointsCount == 0)
	        {
	            return;
	        }

	        _unitHighlighter.UnhighlightUnits(_selectedUnit.Island, _selectedUnit.Slot);
	        _unitMover.MoveAllPossibleUnits(_selectedUnit.Island, _selectedUnit.Slot, island);
	        ResetSelection();
	    }

		private void ResetSelection()
		{
			if (IsUnitsSelected)
			{
				_unitHighlighter.UnhighlightUnits(_selectedUnit.Island, _selectedUnit.Slot);
			}

			_selectedUnit = null;
		}

		private bool IsSameAsSelected(Unit unit)
		{
			return IsUnitsSelected &&
					unit.Island == _selectedUnit.Island &&
					unit.Slot == _selectedUnit.Slot;
		}

		private bool IsUnitsSelected => _selectedUnit != null;
	}
}
