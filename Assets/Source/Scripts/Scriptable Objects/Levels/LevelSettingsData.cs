using UnityEngine;

[System.Serializable]
public struct LevelSettingsData
{
    [SerializeField] private int _id;
    [SerializeField] private IslandsGroupInitializer _levelPrefab;
    [SerializeField] private int _buferIslandSize;
    [SerializeField] private int _extraStarMoveLimit;
    [SerializeField] private float _extraStarTimeLimit;
    [SerializeField] private Vector3 _cameraHorizontalOrientationOffset;
    [SerializeField] private Vector3 _cameraVerticalOrientationOffset;
    [SerializeField] private float _cameraFoVOffset;
    [SerializeField] private float _angryBarSpeed;

    public LevelSettingsData(int id, IslandsGroupInitializer levelPrefab, int buferIslandSize,
                            int extraStarMoveLimit, float extraStarTimeLimit,
                            Vector3 cameraHorizontalOrientationOffset, Vector3 cameraVerticalOrientationOffset,
                            float cameraFoVOffset, float angryBarSpeed)
    {
        _id = id;
        _levelPrefab = levelPrefab;
        _buferIslandSize = buferIslandSize;
        _extraStarMoveLimit = extraStarMoveLimit;
        _extraStarTimeLimit = extraStarTimeLimit;
        _cameraHorizontalOrientationOffset = cameraHorizontalOrientationOffset;
        _cameraVerticalOrientationOffset = cameraVerticalOrientationOffset;
        _cameraFoVOffset = cameraFoVOffset;
        _angryBarSpeed = angryBarSpeed;
    }

    public int Id => _id;
    public IslandsGroupInitializer LevelPrefab => _levelPrefab;
    public int BuferIslandSize => _buferIslandSize;
    public int ExtraStarMoveLimit => _extraStarMoveLimit;
    public float ExtraStarTimeLimit => _extraStarTimeLimit;
    public Vector3 CameraHorizontalOrientationOffset => _cameraHorizontalOrientationOffset;
    public Vector3 CameraVerticalOrientationOffset => _cameraVerticalOrientationOffset;
    public float CameraFoVOffset => _cameraFoVOffset;
    public float AngryBarSpeed => _angryBarSpeed;
}
