using System.Collections.Generic;
using SlimeGround.Data;
using SlimeGround.Data.ScriptableObjects.Paints;
using SlimeGround.Gameplay.Islands;
using UnityEditor;
using UnityEngine;

namespace SlimeGround.Editor.LevelComponentsCreator
{
    public class IslandSettingsInGUILayout
    {
        private const int IslandLayerIndex = 6;

        private int _spacingOffset = 5;

        public void PrintIslandsSettings(IReadOnlyCollection<IslandInitializer> islands, ColorSampleMaterials paintMaterials)
        {
            EditorGUILayout.Space();
            GUILayout.Label("Islands:", EditorStyles.boldLabel);

            foreach (IslandInitializer initializer in islands)
            {
                PrintIslandSettings(initializer, paintMaterials);
            }
        }

        private void PrintIslandSettings(IslandInitializer initializer, ColorSampleMaterials paintMaterials)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space(_spacingOffset);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(initializer.gameObject.name, EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.BeginVertical();

            UnitSlotType newSlot = (UnitSlotType)EditorGUILayout.EnumPopup(initializer.UnitSlot);

            if (newSlot != initializer.UnitSlot)
            {
                ApplyPaintToIsland(newSlot, initializer, paintMaterials);
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

                ApplyPaintToIsland(newSlot, initializer, paintMaterials);
                initializer.gameObject.layer = IslandLayerIndex;
                EditorUtility.SetDirty(initializer.gameObject);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(_spacingOffset);
            EditorGUILayout.EndVertical();
        }

        private void ApplyPaintToIsland(UnitSlotType slot, IslandInitializer initializer, ColorSampleMaterials paintMaterials)
        {
            initializer.SetRequredUnitSlot(slot);
            MeshRenderer meshRenderer = initializer.GetComponent<MeshRenderer>();
            IslandRenderer islandRenderer = new IslandRenderer(meshRenderer, paintMaterials);
            ColorSample colorSample = GetDefaultColorSample(slot);
            islandRenderer.SetPaint(colorSample, initializer.Points);

            Undo.RegisterCreatedObjectUndo(meshRenderer, "Change material");
        }

        private ColorSample GetDefaultColorSample(UnitSlotType slot) => (ColorSample)(int)slot;
    }
}