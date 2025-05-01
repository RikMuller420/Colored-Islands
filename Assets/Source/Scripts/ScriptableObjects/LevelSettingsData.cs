using UnityEngine;

[System.Serializable]
public struct LevelSettingsData
{
    [SerializeField] private int _id;
    [SerializeField] private IslandsGroupInitializer _levelPrefab;
    [SerializeField] private int _buferIslandSize;
    [SerializeField] private int _levelMoveLimit;
    [SerializeField] private int _extraStarMoveLimit;
    [SerializeField] private float _extraStarTimeLimit;

    public int Id => _id;
    public IslandsGroupInitializer LevelPrefab => _levelPrefab;
    public int BuferIslandSize => _buferIslandSize;
    public int LevelMoveLimit => _levelMoveLimit;
    public int ExtraStarMoveLimit => _extraStarMoveLimit;
    public float ExtraStarTimeLimit => _extraStarTimeLimit;

    public LevelSettingsData(int id, IslandsGroupInitializer levelPrefab, int buferIslandSize,
                            int levelMoveLimit, int extraStarMoveLimit, float extraStarTimeLimit)
    {
        _id = id;
        _levelPrefab = levelPrefab;
        _buferIslandSize = buferIslandSize;
        _levelMoveLimit = levelMoveLimit;
        _extraStarMoveLimit = extraStarMoveLimit;
        _extraStarTimeLimit = extraStarTimeLimit;
    }
}
