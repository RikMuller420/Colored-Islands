using TMPro;
using UnityEngine;

[System.Serializable]
public class DevelopLevelView
{
    public Transform LevelHolder { get; }
    public GameObject LevelObject { get; }
    public TextMeshPro LevelNumber { get; }
    public LevelSettingsData LevelSettings { get; }

    public DevelopLevelView(Transform levelHolder, GameObject levelObject,
                            TextMeshPro levelNumber, LevelSettingsData levelSettings)
    {
        LevelHolder = levelHolder;
        LevelObject = levelObject;
        LevelNumber = levelNumber;
        LevelSettings = levelSettings;
    }
}
