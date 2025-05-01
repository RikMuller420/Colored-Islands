using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitsSummaryInGUILayout 
{
    private int _spacingOffset = 5;

    public Dictionary<Paint, int> CreateUnitsSummary(IReadOnlyCollection<IslandInitializer> islands)
    {
        EditorGUILayout.Space(_spacingOffset);
        GUILayout.Label("Units Summary:", EditorStyles.boldLabel);

        int maxColorCount = Enum.GetValues(typeof(Paint)).Length;
        Dictionary<Paint, int> requireUnitsAmount = new Dictionary<Paint, int>();

        foreach (IslandInitializer island in islands)
        {
            if (requireUnitsAmount.ContainsKey(island.Paint))
            {
                requireUnitsAmount[island.Paint] += island.PointsCount;
            }
            else
            {
                requireUnitsAmount.Add(island.Paint, island.PointsCount);
            }
        }

        foreach (KeyValuePair<Paint, int> colorUnitsAmount in requireUnitsAmount)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Color: {colorUnitsAmount.Key} - {colorUnitsAmount.Value}", GUILayout.Width(300));
            EditorGUILayout.EndHorizontal();
        }

        return requireUnitsAmount;
    }
}
