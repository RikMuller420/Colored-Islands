using UnityEngine;
using UnityEditor;

public class IslandPointsCreator : EditorWindow
{
    private const string Title = "Island Points Creator";

    private float _maxGridSpacing = 1f;
    private float _maxGridOffset = 1f;
    private float _maxHeightOffset = 0.1f;
    private float _raycastOffset = 10f;
    private float _raycastLenght = 20f;

    [SerializeField] private GameObject _pointPrefab;

    private MeshFilter _islandMesh;
    private string _prefabHolderSceneObjectName = "Placement Points";
    private Vector2 _gridSpacing = new Vector2(1f, 1f);
    private Vector2 _gridOffset = Vector2.zero;
    private float _heightOffset = 0.03f;
    private bool _autoUpdate = false;
    private Vector2 _scrollPosition;

    [MenuItem("Window/" + Title)]
    public static void ShowWindow()
    {
        GetWindow<IslandPointsCreator>(Title);
    }

    private void OnEnable()
    {
        _pointPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Source/Prefabs/PlacementPoint.prefab");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        _islandMesh = (MeshFilter)EditorGUILayout.ObjectField("Island Mesh", _islandMesh, typeof(MeshFilter), true);
        _pointPrefab = (GameObject)EditorGUILayout.ObjectField("Point Prefab", _pointPrefab, typeof(GameObject), false);
        _prefabHolderSceneObjectName = EditorGUILayout.TextField("Holder Object Name", _prefabHolderSceneObjectName);

        EditorGUI.BeginChangeCheck();

        _gridSpacing.x = EditorGUILayout.Slider("Grid Spacing X", _gridSpacing.x, 0f, _maxGridSpacing);
        _gridSpacing.y = EditorGUILayout.Slider("Grid Spacing Z", _gridSpacing.y, 0f, _maxGridSpacing);
        _gridOffset.x = EditorGUILayout.Slider("Grid Offset X", _gridOffset.x, 0f, _maxGridOffset);
        _gridOffset.y = EditorGUILayout.Slider("Grid Offset Z", _gridOffset.y, 0f, _maxGridOffset);
        _heightOffset = EditorGUILayout.Slider("Height Offset", _heightOffset, 0f, _maxHeightOffset);

        if (EditorGUI.EndChangeCheck() && _autoUpdate)
        {
            DistributePoints();
        }

        _autoUpdate = EditorGUILayout.Toggle("Auto Update", _autoUpdate);

        if (GUILayout.Button("Distribute Points"))
        {
            DistributePoints();
        }

        if (GUILayout.Button("Clear Distributed Points"))
        {
            ClearPoints();
        }

        EditorGUILayout.EndScrollView();
    }

    private bool TryGetComponents(out MeshFilter meshFilter, out Collider collider, out bool isExtraColliderCreated)
    {
        meshFilter = null;
        collider = null;
        isExtraColliderCreated = false;

        if (_islandMesh == null || _pointPrefab == null)
        {
            _autoUpdate = false;

            return false;
        }

        meshFilter = _islandMesh.GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            _autoUpdate = false;

            return false;
        }

        collider = _islandMesh.GetComponent<Collider>();

        if (collider == null)
        {
            MeshCollider buferCollider = _islandMesh.gameObject.AddComponent<MeshCollider>();
            buferCollider.sharedMesh = meshFilter.sharedMesh;
            collider = buferCollider;
            isExtraColliderCreated = true;
            Undo.RegisterCreatedObjectUndo(buferCollider, "Create Temporary MeshCollider");
        }

        return true;
    }

    private void DistributePoints()
    {
        bool objectIsFine = TryGetComponents(out MeshFilter meshFilter, out Collider collider,
                            out bool isExtraColliderCreated);

        if (objectIsFine == false)
        {
            return;
        }

        ClearPoints();

        Bounds bounds = meshFilter.sharedMesh.bounds;
        Matrix4x4 localToWorld = _islandMesh.transform.localToWorldMatrix;
        Vector3 worldMin = localToWorld.MultiplyPoint(bounds.min);
        Vector3 worldMax = localToWorld.MultiplyPoint(bounds.max);

        float minX = Mathf.Min(worldMin.x, worldMax.x);
        float minZ = Mathf.Min(worldMin.z, worldMax.z);
        float sizeX = Mathf.Abs(worldMax.x - worldMin.x);
        float sizeZ = Mathf.Abs(worldMax.z - worldMin.z);

        int rows = Mathf.FloorToInt(sizeZ / _gridSpacing.y);
        int columns = Mathf.FloorToInt(sizeX / _gridSpacing.x);

        if (rows < 1 || columns < 1)
        {
            if (isExtraColliderCreated)
            {
                Undo.DestroyObjectImmediate(collider);
            }

            _autoUpdate = false;

            return;
        }

        Transform parent = new GameObject(_prefabHolderSceneObjectName).transform;
        parent.SetParent(_islandMesh.transform);
        Undo.RegisterCreatedObjectUndo(parent.gameObject, "Create Placement Points");

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                TryCreatePrefab(i, j, minX, minZ, worldMax, collider, parent);
            }
        }

        if (isExtraColliderCreated)
        {
            Undo.DestroyObjectImmediate(collider);
        }
    }


    private void TryCreatePrefab(int cellX, int cellY, float minX, float minZ, Vector3 worldMax,
                                Collider collider, Transform parent)
    {
        Vector3 localPos = new Vector3(
                    _gridOffset.x + cellX * _gridSpacing.x,
                    0,
                    _gridOffset.y + cellY * _gridSpacing.y
                );

        Vector3 worldPos = new Vector3(
            minX + localPos.x,
            worldMax.y + _raycastOffset,
            minZ + localPos.z
        );

        Ray ray = new Ray(worldPos, Vector3.down);
        RaycastHit hit;

        if (collider.Raycast(ray, out hit, _raycastLenght))
        {
            Vector3 placePos = hit.point + Vector3.up * _heightOffset;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(_pointPrefab);
            Undo.RegisterCreatedObjectUndo(instance, "Create Prefab Instance");

            instance.transform.position = placePos;
            instance.transform.rotation = _islandMesh.transform.rotation;
            instance.transform.SetParent(parent);
        }
    }

    private void ClearPoints()
    {
        Transform parent = _islandMesh.transform.Find(_prefabHolderSceneObjectName);

        if (parent != null)
        {
            Undo.DestroyObjectImmediate(parent.gameObject);
        }
    }
}
