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
    [SerializeField] private Button _addGoldButton;
    [SerializeField] private Button _spendGoldButton;
    [SerializeField] private Button _finishLevelButton;
    [SerializeField] private Button _failLevelButton;
    [SerializeField] private Button _resetSaveButton;

    [SerializeField] private Button _addBuferIslandBoostButton;
    [SerializeField] private Button _addFinishIslandBoostButton;
    [SerializeField] private Button _addFreezeTimerBoostButton;
    [SerializeField] private Button _addReduceColorBoostButton;

    private WalletProvider _wallet;
    private GameProgressStorage _progressStorage;

    private void OnEnable()
    {
        _levelSlider.onValueChanged.AddListener(OnSliderChange);
        _loadButton.onClick.AddListener(LoadLevel);
        _addGoldButton.onClick.AddListener(AddGold);
        _spendGoldButton.onClick.AddListener(SpendGold);
        _finishLevelButton.onClick.AddListener(FinishLevel);
        _resetSaveButton.onClick.AddListener(ResetSave);

        _addBuferIslandBoostButton.onClick.AddListener(AddBuferIslandBoost);
        _addFinishIslandBoostButton.onClick.AddListener(AddFinishIslandBoost);
        _addFreezeTimerBoostButton.onClick.AddListener(AddFreezeTimerBoost);
        _addReduceColorBoostButton.onClick.AddListener(AddReducePaintBoost);
    }
    private void OnDisable()
    {
        _levelSlider.onValueChanged.RemoveListener(OnSliderChange);
        _loadButton.onClick.RemoveListener(LoadLevel);
        _addGoldButton.onClick.RemoveListener(AddGold);
        _spendGoldButton.onClick.RemoveListener(SpendGold);
        _finishLevelButton.onClick.RemoveListener(FinishLevel);
        _resetSaveButton.onClick.RemoveListener(ResetSave);

        _addBuferIslandBoostButton.onClick.RemoveListener(AddBuferIslandBoost);
        _addFinishIslandBoostButton.onClick.RemoveListener(AddFinishIslandBoost);
        _addFreezeTimerBoostButton.onClick.RemoveListener(AddFreezeTimerBoost);
        _addReduceColorBoostButton.onClick.RemoveListener(AddReducePaintBoost);
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

    private void ResetSave()
    {
        _progressStorage.SetTrainingFinished(false);
        //_progressStorage.ResetProgress();
        _levelLoader.LoadMainMenu();
    }

    private void AddBuferIslandBoost()
    {
        AddBoost(BoostType.GrowBuferIsland);
    }

    private void AddFinishIslandBoost()
    {
        AddBoost(BoostType.FinishIsland);
    }

    private void AddFreezeTimerBoost()
    {
        AddBoost(BoostType.FreezeObjectives);
    }

    private void AddReducePaintBoost()
    {
        AddBoost(BoostType.ReducePaints);
    }

    private void AddBoost(BoostType boostType)
    {
        int boostAmount = _progressStorage.GetBoostAmount(boostType) + 1;
        _progressStorage.SetBoostAmount(boostType, boostAmount);
    }
}
