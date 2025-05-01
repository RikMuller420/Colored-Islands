using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitsVisualizator
{
    private const string VisualizationHolderSceneObjectName = "Visualization";

    private GameObject _visualizationHolder = null;

    public bool IsVisualizationExist => _visualizationHolder != null;

    public void VisualizeUnits(IReadOnlyCollection<IslandInitializer> _islandInitializers, Unit unitPrefab, PaintMaterials paintMaterials)
    {
        ClearUnitsVisualization();

        if (_visualizationHolder == null)
        {
            _visualizationHolder = new GameObject(VisualizationHolderSceneObjectName);
        }

        foreach (IslandInitializer island in _islandInitializers)
        {
            int pointIndex = 0;

            foreach (IslandStartUnits islandStartUnits in island.StartUnits)
            {
                for (int i = 0; i < islandStartUnits.Count; i++)
                {
                    Vector3 placePos = island.Points[pointIndex].position;

                    Unit unit = (Unit)PrefabUtility.InstantiatePrefab(unitPrefab);
                    Undo.RegisterCreatedObjectUndo(unit, "Create Prefab Instance");

                    unit.transform.position = placePos;
                    unit.transform.SetParent(_visualizationHolder.transform);
                    unit.Initialize(island.Island, islandStartUnits.Paint, paintMaterials);

                    pointIndex++;
                }
            }
        }
    }

    public void ClearUnitsVisualization()
    {
        if (_visualizationHolder != null)
        {
            Undo.DestroyObjectImmediate(_visualizationHolder);
        }
    }
}
