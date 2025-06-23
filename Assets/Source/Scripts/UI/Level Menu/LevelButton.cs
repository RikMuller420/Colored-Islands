using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private GameObject _starsHolder;
    [SerializeField] private List<Image> _starsImages;
    [SerializeField] private GameObject _lockIcon;

    [SerializeField] private Sprite _noStarSprite;
    [SerializeField] private Sprite _starSprite;

    private int _levelId;
    private bool _isLevelAviable;
    private GameProgressStorage _progressStorage;
    private LevelLoader _levelLoader;

    private void OnEnable()
    {
        _button.onClick.AddListener(LoadLevel);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadLevel);
    }

    public void Initialize(int levelId, GameProgressStorage progressStorage, LevelLoader levelLoader)
    {
        _levelId = levelId;
        _progressStorage = progressStorage;
        _levelLoader = levelLoader;

        _levelNumberText.text = levelId.ToString();
        UpdateButtonAviability();

        if (_isLevelAviable)
        {
            UpdateStarSprites();
        }

        _progressStorage.LevelProgressChanged += OnLevelProgressChanged;
    }

    private void UpdateButtonAviability()
    {
        _isLevelAviable = _levelId <= _progressStorage.FirstUnfinishedLevel.Id;
        _starsHolder.SetActive(_isLevelAviable);
        _button.interactable = _isLevelAviable;
        _lockIcon.SetActive(!_isLevelAviable);
    }

    private void UpdateStarSprites()
    {
        LevelProgress levelProgress = _progressStorage.Levels.FirstOrDefault(level => level.Id == _levelId);
        int starsCount = levelProgress.GetStarsCount();

        for (int i = 0; i < starsCount; i++)
        {
            _starsImages[i].sprite = _starSprite;
        }

        for (int i = starsCount; i < _starsImages.Count; i++)
        {
            _starsImages[i].sprite = _noStarSprite;
        }
    }

    private void OnLevelProgressChanged(int changedLevelId)
    {
        if (_isLevelAviable == false && changedLevelId == _levelId - 1)
        {
            UpdateButtonAviability();
        }
        else if(changedLevelId == _levelId)
        {
            UpdateStarSprites();
        }
    }

    private void LoadLevel()
    {
        _levelLoader.LoadLevel(_levelId);
    }
}
