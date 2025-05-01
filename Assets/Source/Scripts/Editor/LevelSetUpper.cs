using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

public class LevelSetUpper : EditorWindow
{
    private const string Title = "Level Setup Helper";
    private const string VisualizationHolderSceneObjectName = "Visualization";
    private const int IslandLayerIndex = 6;

    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private PaintMaterials _paintMaterials;

    private Transform _islandsParent;
    private List<IslandInitializer> _islandInitializers = new List<IslandInitializer>();
    private Dictionary<Paint, int> _colorsUnitsAmount = new Dictionary<Paint, int>();
    private GameObject _visualizationHolder = null;
    private int _spacingOffset = 5;
    private Vector2 _scrollPosition;


    [MenuItem("Window/" + Title)]
    public static void ShowWindow()
    {
        GetWindow<LevelSetUpper>(Title);
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
            SetupIslandInitializers();
        }

        if (_islandInitializers.Count > 0)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Islands:", EditorStyles.boldLabel);

            foreach (IslandInitializer initializer in _islandInitializers)
            {
                PrintIslandSettings(initializer);
            }
        }

        PrintUnitsSummary();

        if (GUILayout.Button("Distribute Units"))
        {
            DistributeIslandStartUnits();
        }

        EditorGUILayout.Space(_spacingOffset);
        _unitPrefab = (Unit)EditorGUILayout.ObjectField("Unit Prefab", _unitPrefab, typeof(Unit), false);

        if (GUILayout.Button("Visualize Units"))
        {
            VisualizeUnits();
        }

        if (GUILayout.Button("Clear Units Visualization"))
        {
            ClearUnitsVisualization();
        }

        EditorGUILayout.EndScrollView();
    }

    private void SetupIslandInitializers()
    {
        _islandInitializers.Clear();

        foreach (Transform child in _islandsParent)
        {
            IslandInitializer initializer = child.GetComponent<IslandInitializer>();

            if (child.TryGetComponent<Collider>(out _) == false)
            {
                child.gameObject.AddComponent<MeshCollider>();
                EditorUtility.SetDirty(child.gameObject);
            }

            if (child.TryGetComponent<Island>(out _) == false)
            {
                child.gameObject.AddComponent<Island>();
                EditorUtility.SetDirty(child.gameObject);
            }

            if (initializer == null)
            {
                initializer = child.gameObject.AddComponent<IslandInitializer>();
            }

            if (initializer != null)
            {
                _islandInitializers.Add(initializer);
            }

            EditorUtility.SetDirty(child.gameObject);
        }

        if (_islandsParent.TryGetComponent<IslandsGroupInitializer>(out _) == false)
        {
            _islandsParent.gameObject.AddComponent<IslandsGroupInitializer>();
        }

        _islandsParent.GetComponent<IslandsGroupInitializer>().SetIslands(_islandInitializers);
    }

    private void PrintUnitsSummary()
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

    private void DistributeIslandStartUnits()
    {
        Dictionary<Paint, int> colorsUnitsAmount = new Dictionary<Paint, int>(_colorsUnitsAmount);
        Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits = new Dictionary<IslandInitializer, List<IslandStartUnits>>();

        AddFirstUnitToIslandsDictionary(colorsUnitsAmount, islandsUnits);
        AddLackingUnitToIslandsDictionary(colorsUnitsAmount, islandsUnits);

        foreach (var islandUnits in islandsUnits)
        {
            islandUnits.Key.SetStartUnits(islandUnits.Value);
            EditorUtility.SetDirty(islandUnits.Key.gameObject);
        }

        if (_visualizationHolder != null)
        {
            VisualizeUnits();
        }
    }

    private void AddFirstUnitToIslandsDictionary(Dictionary<Paint, int> colorsUnitsAmount,
                                     Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits)
    {
        foreach (IslandInitializer initializer in _islandInitializers)
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

    private void AddLackingUnitToIslandsDictionary(Dictionary<Paint, int> colorsUnitsAmount,
                                     Dictionary<IslandInitializer, List<IslandStartUnits>> islandsUnits)
    {
        foreach (IslandInitializer initializer in _islandInitializers)
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

    private void PrintIslandSettings(IslandInitializer initializer)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Space(_spacingOffset);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(initializer.gameObject.name, EditorStyles.boldLabel, GUILayout.Width(100));
        EditorGUILayout.BeginVertical();

        Paint newPaint = (Paint)EditorGUILayout.EnumPopup(initializer.Paint);
        
        if (newPaint != initializer.Paint)
        {
            ApplyPaintToIsland(newPaint, initializer);
            EditorUtility.SetDirty(initializer.gameObject);
        }

        Transform newRootOfPoints = initializer.RootOfPoints;
        newRootOfPoints = (Transform)EditorGUILayout.ObjectField("Parent of points", initializer.RootOfPoints, typeof(Transform), true);

        if (newRootOfPoints != initializer.RootOfPoints)
        {
            initializer.FillPoints(newRootOfPoints);
            EditorUtility.SetDirty(initializer.gameObject);
        }

        EditorGUILayout.LabelField("Points: " + initializer.PointsCount, EditorStyles.boldLabel, GUILayout.Width(100));

        if (GUILayout.Button("Reset Island"))
        {
            initializer.FindRequireComponents();

            if (newRootOfPoints == null && initializer.transform.childCount > 0)
            {
                newRootOfPoints = initializer.transform.GetChild(0);
                initializer.FillPoints(newRootOfPoints);
            }

            ApplyPaintToIsland(newPaint, initializer);
            initializer.gameObject.layer = IslandLayerIndex;
            EditorUtility.SetDirty(initializer.gameObject);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(_spacingOffset);
        EditorGUILayout.EndVertical();
    }

    private void ApplyPaintToIsland(Paint paint, IslandInitializer initializer)
    {
        initializer.SetPaint(paint);
        MeshRenderer meshRenderer = initializer.GetComponent<MeshRenderer>();
        IslandRenderer islandRenderer = new IslandRenderer(meshRenderer, _paintMaterials);
        islandRenderer.SetPaint(paint);

        Undo.RegisterCreatedObjectUndo(meshRenderer, "Change material");
    }

    private void VisualizeUnits()
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

                    Unit unit = (Unit)PrefabUtility.InstantiatePrefab(_unitPrefab);
                    Undo.RegisterCreatedObjectUndo(unit, "Create Prefab Instance");

                    unit.transform.position = placePos;
                    unit.transform.SetParent(_visualizationHolder.transform);
                    unit.Initialize(island.Island, islandStartUnits.Paint, _paintMaterials);

                    pointIndex++;
                }
            }
        }
    }

    private void ClearUnitsVisualization()
    {
        if (_visualizationHolder != null)
        {
            Undo.DestroyObjectImmediate(_visualizationHolder);
        }
    }
}
