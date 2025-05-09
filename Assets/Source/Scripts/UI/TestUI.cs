using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    private const int MainMenuLvlIndex = -1;

    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private LevelProgressTracker _progressTracker;

    [SerializeField] private Slider _levelSlider;
    [SerializeField] private TextMeshProUGUI _lvlText;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _unloadButton;
    [SerializeField] private Button _addGoldButton;
    [SerializeField] private Button _spendGoldButton;
    [SerializeField] private Button _finishLevelButton;
    [SerializeField] private Button _failLevelButton;
    [SerializeField] private Button _resetSaveButton;

    private WalletProvider _wallet;
    private GameProgressStorage _progressStorage;

    private void OnEnable()
    {
        _levelSlider.onValueChanged.AddListener(OnSliderChange);
        _loadButton.onClick.AddListener(LoadLevel);
        _unloadButton.onClick.AddListener(UnloadLevel);
        _addGoldButton.onClick.AddListener(AddGold);
        _spendGoldButton.onClick.AddListener(SpendGold);
        _finishLevelButton.onClick.AddListener(FinishLevel);
        _failLevelButton.onClick.AddListener(FailLevel);
        _resetSaveButton.onClick.AddListener(ResetSave);
    }
    private void OnDisable()
    {
        _levelSlider.onValueChanged.RemoveListener(OnSliderChange);
        _loadButton.onClick.RemoveListener(LoadLevel);
        _unloadButton.onClick.RemoveListener(UnloadLevel);
        _addGoldButton.onClick.RemoveListener(AddGold);
        _spendGoldButton.onClick.RemoveListener(SpendGold);
        _finishLevelButton.onClick.RemoveListener(FinishLevel);
        _failLevelButton.onClick.RemoveListener(FailLevel);
        _resetSaveButton.onClick.RemoveListener(ResetSave);
    }

    public void Initialize(GameProgressStorage progressStorage, WalletProvider wallet)
    {
        _progressStorage = progressStorage;
        _wallet = wallet;
        _levelSlider.minValue = 1;
        _levelSlider.maxValue = _levelSettings.Levels.Count;
        enabled = true;
    }

    private void OnSliderChange(float value)
    {
        _lvlText.text = value.ToString();
    }

    private void LoadLevel()
    {
        int level = Mathf.FloorToInt(_levelSlider.value);
        _levelLoader.LoadLevel(level);
    }

    private void UnloadLevel()
    {
        _levelLoader.UnloadCurrentLevel();
    }

    private void AddGold()
    {
        int newGoldAmount = _progressStorage.GoldAmount + 100;
        _progressStorage.SetGoldAmount(newGoldAmount);
    }

    private void SpendGold()
    {
        if (_wallet.GoldAmount < 100)
        {
            return;
        }

        _wallet.SpendGold(100);
    }

    private void FinishLevel()
    {
        if (_levelLoader.CurrentLevelData.Id == MainMenuLvlIndex)
        {
            return;
        }

        _progressTracker.FinishLevel();
    }

    private void FailLevel()
    {
        if (_levelLoader.CurrentLevelData.Id == MainMenuLvlIndex)
        {
            return;
        }

        _progressTracker.FailLevel();
    }

    private void ResetSave()
    {
        _progressStorage.ResetProgress();
        _levelLoader.LoadMainMenu();
    }
}
