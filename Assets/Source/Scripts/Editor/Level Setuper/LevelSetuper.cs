using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class LevelSetuper : EditorWindow
{
    private const string Title = "Level Setup Helper";

    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private PaintMaterials _paintMaterials;

    private Transform _islandsParent;
    private List<IslandInitializer> _islandInitializers = new List<IslandInitializer>();
    private Dictionary<Paint, int> _colorsUnitsAmount = new Dictionary<Paint, int>();
    private int _spacingOffset = 5;

    private IslandDependenciesCreator _islandDependenciesCreator = new IslandDependenciesCreator();
    private StartUnitsSettingsCreator _startUnitsSettingsCreator = new StartUnitsSettingsCreator();
    private UnitsVisualizator _unitsVisualizator = new UnitsVisualizator();
    private IslandSettingsView _islandSettingsViewCreator = new IslandSettingsView();

    private Vector2 _scrollPosition;

    [MenuItem("Window/" + Title)]
    public static void ShowWindow()
    {
        GetWindow<LevelSetuper>(Title);
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

        if (GUILayout.Button("Setup Islands") && _islandsParent != null)
        {
            _islandInitializers = _islandDependenciesCreator.SetupIslandInitializers(_islandsParent);
        }

        if (_islandInitializers.Count > 0)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Islands:", EditorStyles.boldLabel);

            foreach (IslandInitializer initializer in _islandInitializers)
            {
                _islandSettingsViewCreator.PrintIslandSettings(initializer, _paintMaterials);
            }
        }

        CreateUnitsSummary();

        if (GUILayout.Button("Distribute Units"))
        {
            _startUnitsSettingsCreator.DistributeIslandStartUnits(_islandInitializers.AsReadOnly(), _colorsUnitsAmount,
                                                                 _unitsVisualizator, _unitPrefab, _paintMaterials);
        }

        EditorGUILayout.Space(_spacingOffset);
        _unitPrefab = (Unit)EditorGUILayout.ObjectField("Unit Prefab", _unitPrefab, typeof(Unit), false);

        if (GUILayout.Button("Visualize Units"))
        {
            _unitsVisualizator.VisualizeUnits(_islandInitializers.AsReadOnly(), _unitPrefab, _paintMaterials);
        }

        if (GUILayout.Button("Clear Units Visualization"))
        {
            _unitsVisualizator.ClearUnitsVisualization();
        }

        EditorGUILayout.EndScrollView();
    }

    private void CreateUnitsSummary()
    {
        EditorGUILayout.Space(_spacingOffset);
        GUILayout.Label("Units Summary:", EditorStyles.boldLabel);

        int maxColorCount = Enum.GetValues(typeof(Paint)).Length;
        _colorsUnitsAmount = new Dictionary<Paint, int>();

        foreach (IslandInitializer island in _islandInitializers)
        {
            if (_colorsUnitsAmount.ContainsKey(island.Paint))
            {
                _colorsUnitsAmount[island.Paint] += island.PointsCount;
            }
            else
            {
                _colorsUnitsAmount.Add(island.Paint, island.PointsCount);
            }
        }

        foreach (var colorUnitsAmount in _colorsUnitsAmount)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Color: {colorUnitsAmount.Key} - {colorUnitsAmount.Value}", GUILayout.Width(300));
            EditorGUILayout.EndHorizontal();
        }
    }
}
