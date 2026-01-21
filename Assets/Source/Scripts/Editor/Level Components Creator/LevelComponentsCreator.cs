using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using SlimeGround.Gameplay.Units;
using SlimeGround.Data.ScriptableObjects.Paints;
using SlimeGround.Gameplay.Islands;
using SlimeGround.Data;

namespace SlimeGround.Editor.LevelComponentsCreator
{
    public class LevelComponentsCreator : EditorWindow
    {
        private const string Title = "Require Level Components Creator";

        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private ColorSampleMaterials _paintMaterials;
        [SerializeField] private Mesh _cubeMesh;

        private Transform _islandsParent;

        private IReadOnlyCollection<IslandInitializer> _islands = new List<IslandInitializer>();

        private IslandsComponentsCreator _islandsComponentsCreator;
        private UnitsOnIslandDistributor _unitsOnIslandDistributor = new UnitsOnIslandDistributor();
        private UnitsVisualizator _unitsVisualizator = new UnitsVisualizator();
        private IslandSettingsInGUILayout _islandSettingsInGUILayout = new IslandSettingsInGUILayout();
        private UnitsSummaryInGUILayout _unitsSummaryInGUILayout = new UnitsSummaryInGUILayout();

        private int _spacingOffset = 5;
        private Vector2 _scrollPosition;

        [MenuItem("Window/" + Title)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<LevelComponentsCreator>(Title);
        }

        private void OnEnable()
        {
            _unitPrefab = AssetDatabase.LoadAssetAtPath<Unit>("Assets/Source/Prefabs/Unit.prefab");
            _paintMaterials = AssetDatabase.LoadAssetAtPath<ColorSampleMaterials>("Assets/Source/Prefabs/PaintMaterials.asset");
            _cubeMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
            _islandsComponentsCreator = new IslandsComponentsCreator(_cubeMesh);
        }

        private void OnGUI()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            _islandsParent = (Transform)EditorGUILayout.ObjectField("Islands Parent", _islandsParent, typeof(Transform), true);
            _paintMaterials = (ColorSampleMaterials)EditorGUILayout.ObjectField("Materials Data", _paintMaterials, typeof(ColorSampleMaterials), false);

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
            Dictionary<UnitSlotType, int> unitsAmounts = _unitsSummaryInGUILayout.CreateUnitsSummary(_islands);

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
}
