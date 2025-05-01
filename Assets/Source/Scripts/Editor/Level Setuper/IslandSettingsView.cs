using UnityEditor;
using UnityEngine;

public class IslandSettingsView
{
    private const int IslandLayerIndex = 6;

    private int _spacingOffset = 5;

    public void PrintIslandSettings(IslandInitializer initializer, PaintMaterials paintMaterials)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Space(_spacingOffset);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(initializer.gameObject.name, EditorStyles.boldLabel, GUILayout.Width(100));
        EditorGUILayout.BeginVertical();

        Paint newPaint = (Paint)EditorGUILayout.EnumPopup(initializer.Paint);

        if (newPaint != initializer.Paint)
        {
            ApplyPaintToIsland(newPaint, initializer, paintMaterials);
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

            ApplyPaintToIsland(newPaint, initializer, paintMaterials);
            initializer.gameObject.layer = IslandLayerIndex;
            EditorUtility.SetDirty(initializer.gameObject);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(_spacingOffset);
        EditorGUILayout.EndVertical();
    }

    private void ApplyPaintToIsland(Paint paint, IslandInitializer initializer, PaintMaterials paintMaterials)
    {
        initializer.SetPaint(paint);
        MeshRenderer meshRenderer = initializer.GetComponent<MeshRenderer>();
        IslandRenderer islandRenderer = new IslandRenderer(meshRenderer, paintMaterials);
        islandRenderer.SetPaint(paint);

        Undo.RegisterCreatedObjectUndo(meshRenderer, "Change material");
    }
}
