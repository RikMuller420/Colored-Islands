using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DevelopLevelCreator : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;

    [SerializeField] private List<DevelopLevelView> _levelViews = new List<DevelopLevelView>();

    [ContextMenu("Update Level Views")]
    public void UpdateLevelViews()
    {
        foreach (LevelSettingsData levelSettings in _levelSettings.Levels)
        {
            DevelopLevelView levelView = _levelViews.FirstOrDefault(view => view.LevelSettings.Id == levelSettings.Id);

            if (levelView == null)
            {
                levelView = CreateLevelView(levelSettings);
                _levelViews.Add(levelView);
            }

            LocateLevelView(levelView);
        }
    }

    private DevelopLevelView CreateLevelView(LevelSettingsData levelSettings)
    {
        GameObject levelHolder = new GameObject("Level View " + levelSettings.Id);
        Transform levelHolderTransform = levelHolder.transform;
        levelHolderTransform.parent = transform;

        Object levelPrefab = levelSettings.LevelPrefab.gameObject;
        GameObject levelObject = (GameObject)PrefabUtility.InstantiatePrefab(levelPrefab, levelHolderTransform);
        Level level = levelObject.GetComponent<Level>();

        GameObject levelNumberObject = new GameObject("Level Number");
        levelNumberObject.transform.parent = levelHolderTransform;
        RectTransform numberRect = levelNumberObject.AddComponent<RectTransform>();
        numberRect.localPosition = new Vector3(0, 0, 6);
        numberRect.sizeDelta = new Vector2(20, 5);
        numberRect.localEulerAngles = new Vector3(270, 0, 0);
        numberRect.localScale = new Vector3(1, -1, 1);
        TextMeshPro numberText = levelNumberObject.AddComponent<TextMeshPro>();
        numberText.text = levelSettings.Id.ToString("D3");
        numberText.fontSize = 36;
        numberText.alignment = TextAlignmentOptions.Center;
        numberText.color = Color.black;

        return new DevelopLevelView(levelHolderTransform, levelNumberObject, numberText, levelSettings);
    }

    private void LocateLevelView(DevelopLevelView levelView)
    {
        levelView.LevelHolder.localPosition = new Vector3(0.8f + (levelView.LevelSettings.Id * 10f), 0, 0);
    }
}
