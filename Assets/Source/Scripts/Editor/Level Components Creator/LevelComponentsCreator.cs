using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class LevelComponentsCreator : EditorWindow
{
    private const string Title = "Require Level Components Creator";

    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private PaintMaterials _paintMaterials;
    private Transform _islandsParent;

    private IReadOnlyCollection<IslandInitializer> _islands = new List<IslandInitializer>();

    private IslandsComponentsCreator _islandsComponentsCreator = new IslandsComponentsCreator();
    private UnitsOnIslandDistributor _unitsOnIslandDistributor = new UnitsOnIslandDistributor();
    private UnitsVisualizator _unitsVisualizator = new UnitsVisualizator();
    private IslandSettingsInGUILayout _islandSettingsInGUILayout = new IslandSettingsInGUILayout();
    private UnitsSummaryInGUILayout _unitsSummaryInGUILayout = new UnitsSummaryInGUILayout();

    private int _spacingOffset = 5;
    private Vector2 _scrollPosition;

    [MenuItem("Window/" + Title)]
    public static void ShowWindow()
    {
        GetWindow<LevelComponentsCreator>(Title);
    }

    private void OnEnable()
    {
        _unitPrefab = AssetDatabase.LoadAssetAtPath<Unit>("Assets/Source/Prefabs/Unit.prefab");
        _paintMaterials = AssetDatabase.LoadAssetAtPath<PaintMaterials>("Assets/Source/Prefabs/PaintMaterials.asset");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        _islandsParent = (Transform)EditorGUILayout.ObjectField("Islands Parent", _islandsParent, typeof(Transform), true);
        _paintMaterials = (PaintMaterials)EditorGUILayout.ObjectField("Materials Data", _paintMaterials, typeof(PaintMaterials), false);

        if (GUILayout.Button("Create Require Components") && _islandsParent != null)
        {
            _islands = _islandsComponentsCreator.CreateRequireComponents(_islandsParent);
        }

        if (_islands == null || _islands.Count == 0 || _islandsParent == null)
        {
            EditorGUILayout.EndScrollView();

            return;
        }

        _islandSettingsInGUILayout.PrintIslandsSettings(_islands, _paintMaterials);
        Dictionary<Paint, int> unitsAmounts = _unitsSummaryInGUILayout.CreateUnitsSummary(_islands);

        if (GUILayout.Button("Distribute Units"))
        {
            _unitsOnIslandDistributor.DistributeUnits(_islands, unitsAmounts, _unitsVisualizator,
                                                    _unitPrefab, _paintMaterials);
        }

        EditorGUILayout.Space(_spacingOffset);
        _unitPrefab = (Unit)EditorGUILayout.ObjectField("Unit Prefab", _unitPrefab, typeof(Unit), false);

        if (GUILayout.Button("Visualize Units"))
        {
            _unitsVisualizator.Visualize(_islands, _unitPrefab, _paintMaterials);
        }

        if (GUILayout.Button("Clear Units Visualization"))
        {
            _unitsVisualizator.ClearVisualization();
        }

        EditorGUILayout.EndScrollView();
    }  
}
