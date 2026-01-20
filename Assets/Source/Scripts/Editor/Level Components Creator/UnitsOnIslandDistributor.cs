using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class UnitsOnIslandDistributor
{
    public void DistributeUnits(IReadOnlyCollection<IslandInitializer> islands, 
                                           Dictionary<UnitSlotType, int> unitsAmount, UnitsVisualizator unitsVisualizator,
                                           Unit unitPrefab, PaintMaterials paintMaterials)
    {
        Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits = new Dictionary<IslandInitializer, List<IslandStartUnits>>();

        AddFirstUnitToIslandsDictionary(islands, unitsAmount, islandsUnits);
        AddLackingUnitsToIslandsDictionary(islands, unitsAmount, islandsUnits);

        foreach (var islandUnits in islandsUnits)
        {
            islandUnits.Key.SetStartUnits(islandUnits.Value);
            EditorUtility.SetDirty(islandUnits.Key.gameObject);
        }

        if (unitsVisualizator.IsVisualizationExist)
        {
            unitsVisualizator.Visualize(islands, unitPrefab, paintMaterials);
        }
    }

    private void AddFirstUnitToIslandsDictionary(IReadOnlyCollection<IslandInitializer> islands,
                                            Dictionary<UnitSlotType, int> unitSlotsAmount,
                                            Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits)
    {
        foreach (IslandInitializer initializer in islands)
        {
            List<UnitSlotType> validPaints = unitSlotsAmount.Keys
                                        .Where(unitSlotAmount => unitSlotAmount != initializer.UnitSlot)
                                        .ToList();

            UnitSlotType slot = validPaints[UnityEngine.Random.Range(0, validPaints.Count)];

            List<IslandStartUnits> startUnits = new List<IslandStartUnits>()
            {
                new IslandStartUnits(slot)
            };

            islandsUnits.Add(initializer, startUnits);
            unitSlotsAmount[slot] -= 1;

            if (unitSlotsAmount[slot] == 0)
            {
                unitSlotsAmount.Remove(slot);
            }
        }
    }

    private void AddLackingUnitsToIslandsDictionary(IReadOnlyCollection<IslandInitializer> islands,
                                                Dictionary<UnitSlotType, int> unitSlotsAmount,
                                                Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits)
    {
        foreach (IslandInitializer initializer in islands)
        {
            for (int i = 0; i < initializer.PointsCount - 1; i++)
            {
                UnitSlotType slot = unitSlotsAmount.Keys.ToList()[UnityEngine.Random.Range(0, unitSlotsAmount.Count)];
                List<IslandStartUnits> islandStartUnits = islandsUnits[initializer];
                IslandStartUnits startUnit = islandStartUnits.FirstOrDefault(unit => unit.Slot == slot);

                if (startUnit != null)
                {
                    IslandStartUnits newStartUnits = new IslandStartUnits(slot, startUnit.Amout + 1);
                    islandStartUnits.Remove(startUnit);
                    islandStartUnits.Add(newStartUnits);
                }
                else
                {
                    islandStartUnits.Add(new IslandStartUnits(slot));
                }

                unitSlotsAmount[slot] -= 1;

                if (unitSlotsAmount[slot] == 0)
                {
                    unitSlotsAmount.Remove(slot);
                }
            }
        }
    }
}
