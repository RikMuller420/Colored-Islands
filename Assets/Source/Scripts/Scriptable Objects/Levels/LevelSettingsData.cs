using UnityEngine;

[System.Serializable]
public struct LevelSettingsData
{
    [SerializeField] private int _id;
    [SerializeField] private Level _levelPrefab;
    [SerializeField] private int _buferIslandSize;
    [SerializeField] private int _extraStarMoveLimit;
    [SerializeField] private float _extraStarTimeLimit;
    [SerializeField] private float _angryBarSpeed;

    public LevelSettingsData(int id, Level levelPrefab, int buferIslandSize,
                            int extraStarMoveLimit, float extraStarTimeLimit,
                            float angryBarSpeed)
    {
        _id = id;
        _levelPrefab = levelPrefab;
        _buferIslandSize = buferIslandSize;
        _extraStarMoveLimit = extraStarMoveLimit;
        _extraStarTimeLimit = extraStarTimeLimit;
        _angryBarSpeed = angryBarSpeed;
    }

    public int Id => _id;
    public Level LevelPrefab => _levelPrefab;
    public int BuferIslandSize => _buferIslandSize;
    public int ExtraStarMoveLimit => _extraStarMoveLimit;
    public float ExtraStarTimeLimit => _extraStarTimeLimit;
    public float AngryBarSpeed => _angryBarSpeed;
}
