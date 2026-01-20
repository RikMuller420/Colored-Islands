using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public class PaintAmountReduceBoost : Boost
{
    private ILevelData _currentLevelData;
    private BuferIslandsHolder _buferIslands;
    private UnitMover _unitMover;
    private IPlayerData _playerData;

    private int _bestNewColorIndex = 2;

    public PaintAmountReduceBoost(ILevelData currentLevelData, BuferIslandsHolder buferIslands,
                                    BoostAmountProvider boostAmountProvider, IPlayerData playerData,
                                    UnitMover unitMover) : base(boostAmountProvider)
    {
        _currentLevelData = currentLevelData;
        _buferIslands = buferIslands;
        _unitMover = unitMover;
        _playerData = playerData;
    }
    public override BoostType Type => BoostType.ReducePaints;

    public override void TryApplyBoost()
    {
        ReadOnlyCollection<UnitSlotType> unitSlots = CalculateSortedUnitSlotsAmounts();

        UnitSlotType oldUnitSlot = unitSlots[0];
        UnitSlotType newUnitSlot = unitSlots[unitSlots.Count - 1];

        if (unitSlots.Count > _bestNewColorIndex)
        {
            newUnitSlot = unitSlots[unitSlots.Count - _bestNewColorIndex];
        }

        foreach (Island island in _currentLevelData.Islands)
        {
            if (island.IsDone)
            {
                continue;
            }

            if (island.RequredUnitSlot == oldUnitSlot)
            {
                CustomizationPreferences preference = _playerData.GetCustomizationPreference(newUnitSlot);
                island.SetRequredUnitSlot(newUnitSlot, preference.ColorSample);
            }

            SwapUnitsPaint(island, oldUnitSlot, newUnitSlot);
            island.TryFinish();
        }

        SwapUnitsPaint(_buferIslands.CurrentIsland, oldUnitSlot, newUnitSlot);

        foreach (Island island in _currentLevelData.Islands)
        {
            _unitMover.OptimizeUnitsPosition(island);
        }

        SpendBoost(Type);
    }

    private void SwapUnitsPaint(BaseIsland island, UnitSlotType oldUnitSlot, UnitSlotType newUnitSlot)
    {
        foreach (IslandPoint point in island.Points)
        {
            if (point.IsFree == false && point.OccupiedUnit.Slot == oldUnitSlot)
            {
                point.OccupiedUnit.SetUnitSlot(newUnitSlot);
            }
        }
    }

    private ReadOnlyCollection<UnitSlotType> CalculateSortedUnitSlotsAmounts()
    {
        Dictionary<UnitSlotType, int> unitSlotAmouts = new Dictionary<UnitSlotType, int>();

        foreach (Island island in _currentLevelData.Islands)
        {
            if (island.IsDone == false)
            {
                AddUnitSlotAmount(unitSlotAmouts, island);
            }
        }

        AddUnitSlotAmount(unitSlotAmouts, _buferIslands.CurrentIsland);

        return unitSlotAmouts
                .OrderBy(unitSlotAmout => unitSlotAmout.Value)
                .Select(unitSlotAmout => unitSlotAmout.Key)
                .ToList()
                .AsReadOnly();
    }

    private void AddUnitSlotAmount(Dictionary<UnitSlotType, int> unitSlotsAmouts, BaseIsland island)
    {
        foreach (IslandPoint point in island.Points)
        {
            if (point.IsFree == false)
            {
                AddUnitSlotAmount(unitSlotsAmouts, point.OccupiedUnit.Slot);
            }
        }
    }

    private void AddUnitSlotAmount(Dictionary<UnitSlotType, int> unitSlotsAmouts, UnitSlotType unitSlot)
    {
        if (unitSlotsAmouts.ContainsKey(unitSlot))
        {
            unitSlotsAmouts[unitSlot]++;
        }
        else
        {
            unitSlotsAmouts.Add(unitSlot, 1);
        }
    }
}
