using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private Wallet _wallet;

    [SerializeField] private Slider _levelSlider;
    [SerializeField] private TextMeshProUGUI _lvlText;
    [SerializeField] private Button _loadButton;
    [SerializeField] private Button _unloadButton;
    [SerializeField] private Button _addGoldButton;
    [SerializeField] private Button _spendGoldButton;

    private void OnEnable()
    {
        _levelSlider.onValueChanged.AddListener(OnSliderChange);
        _loadButton.onClick.AddListener(LoadLevel);
        _unloadButton.onClick.AddListener(UnloadLevel);
        _addGoldButton.onClick.AddListener(AddGold);
        _spendGoldButton.onClick.AddListener(SpendGold);
    }
    private void OnDisable()
    {
        _levelSlider.onValueChanged.RemoveListener(OnSliderChange);
        _loadButton.onClick.RemoveListener(LoadLevel);
        _unloadButton.onClick.RemoveListener(UnloadLevel);
        _addGoldButton.onClick.RemoveListener(AddGold);
        _spendGoldButton.onClick.RemoveListener(SpendGold);
    }

    private void Start()
    {
        _levelSlider.minValue = 1;
        _levelSlider.maxValue = _levelSettings.Levels.Count;
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
        _wallet.AddGold(100);
    }

    private void SpendGold()
    {
        if (_wallet.GoldAmount < 100)
        {
            return;
        }

        _wallet.SpendGold(100);
    }
}
