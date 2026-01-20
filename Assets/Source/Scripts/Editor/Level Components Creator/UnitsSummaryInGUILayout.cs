using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitsSummaryInGUILayout 
{
    private int _spacingOffset = 5;

    public Dictionary<UnitSlotType, int> CreateUnitsSummary(IReadOnlyCollection<IslandInitializer> islands)
    {
        EditorGUILayout.Space(_spacingOffset);
        GUILayout.Label("Units Summary:", EditorStyles.boldLabel);

        int maxSlotCount = Enum.GetValues(typeof(UnitSlotType)).Length;
        Dictionary<UnitSlotType, int> requireUnitSlotsAmount = new Dictionary<UnitSlotType, int>();

        foreach (IslandInitializer island in islands)
        {
            if (requireUnitSlotsAmount.ContainsKey(island.UnitSlot))
            {
                requireUnitSlotsAmount[island.UnitSlot] += island.PointsCount;
            }
            else
            {
                requireUnitSlotsAmount.Add(island.UnitSlot, island.PointsCount);
            }
        }

        foreach (KeyValuePair<UnitSlotType, int> unitSlotsAmount in requireUnitSlotsAmount)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Color: {unitSlotsAmount.Key} - {unitSlotsAmount.Value}", GUILayout.Width(300));
            EditorGUILayout.EndHorizontal();
        }

        return requireUnitSlotsAmount;
    }
}
