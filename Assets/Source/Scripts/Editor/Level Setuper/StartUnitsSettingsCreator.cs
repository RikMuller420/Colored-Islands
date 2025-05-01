using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StartUnitsSettingsCreator
{
    public void DistributeIslandStartUnits(IReadOnlyCollection<IslandInitializer> islandInitializers, 
                                           Dictionary<Paint, int> colorsUnitsAmount, UnitsVisualizator unitsVisualizator,
                                           Unit unitPrefab, PaintMaterials paintMaterials)
    {
        Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits = new Dictionary<IslandInitializer, List<IslandStartUnits>>();

        AddFirstUnitToIslandsDictionary(islandInitializers, colorsUnitsAmount, islandsUnits);
        AddLackingUnitToIslandsDictionary(islandInitializers, colorsUnitsAmount, islandsUnits);

        foreach (var islandUnits in islandsUnits)
        {
            islandUnits.Key.SetStartUnits(islandUnits.Value);
            EditorUtility.SetDirty(islandUnits.Key.gameObject);
        }

        if (unitsVisualizator.IsVisualizationExist)
        {
            unitsVisualizator.VisualizeUnits(islandInitializers, unitPrefab, paintMaterials);
        }
    }

    private void AddFirstUnitToIslandsDictionary(IReadOnlyCollection<IslandInitializer> islandInitializers,
                                            Dictionary<Paint, int> colorsUnitsAmount,
                                            Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits)
    {
        foreach (IslandInitializer initializer in islandInitializers)
        {
            List<Paint> validPaints = colorsUnitsAmount.Keys
                                        .Where(paint => paint != initializer.Paint)
                                        .ToList();

            Paint paint = validPaints[UnityEngine.Random.Range(0, validPaints.Count)];

            List<IslandStartUnits> startUnits = new List<IslandStartUnits>()
            {
                new IslandStartUnits(paint)
            };

            islandsUnits.Add(initializer, startUnits);
            colorsUnitsAmount[paint] -= 1;

            if (colorsUnitsAmount[paint] == 0)
            {
                colorsUnitsAmount.Remove(paint);
            }
        }
    }

    private void AddLackingUnitToIslandsDictionary(IReadOnlyCollection<IslandInitializer> islandInitializers,
                                                Dictionary<Paint, int> colorsUnitsAmount,
                                                Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits)
    {
        foreach (IslandInitializer initializer in islandInitializers)
        {
            for (int i = 0; i < initializer.PointsCount - 1; i++)
            {
                Paint paint = colorsUnitsAmount.Keys.ToList()[UnityEngine.Random.Range(0, colorsUnitsAmount.Count)];

                List<IslandStartUnits> islandStartUnits = islandsUnits[initializer];

                IslandStartUnits startUnit = islandStartUnits.FirstOrDefault(unit => unit.Paint == paint);

                if (startUnit != null)
                {
                    IslandStartUnits newStartUnits = new IslandStartUnits(paint, startUnit.Count + 1);
                    islandStartUnits.Remove(startUnit);
                    islandStartUnits.Add(newStartUnits);
                }
                else
                {
                    islandStartUnits.Add(new IslandStartUnits(paint));
                }

                colorsUnitsAmount[paint] -= 1;

                if (colorsUnitsAmount[paint] == 0)
                {
                    colorsUnitsAmount.Remove(paint);
                }
            }
        }
    }
}
