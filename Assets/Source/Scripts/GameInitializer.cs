using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private TestUI _testUI;

    [Header("Settings")]
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private BoostSettings _boostSettings;
    [SerializeField] private LeaderboardSettings _leaderboardSettings;
    [SerializeField] private LocalizationSettings _localizationSettings;
    [SerializeField] private UnitsFaceSettings _faceSettings;
    [SerializeField] private UnitsHatSettings _hatSettings;
    [SerializeField] private AudioMixers _audioMixers;
    [SerializeField] private LayerMask _allIslandsAndUnitsLayer;

    [Header("Links")]
    [SerializeField] private UnitsPool _unitsPool;
    [SerializeField] private BuferIslandsHolder _buferIslands;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private UIZoneSwitcher _uiZoneActivator;
    [SerializeField] private LevelProgressTracker _levelProgressTracker;
    [SerializeField] private Camera _camera;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private FinalScoreWindow _finalScoreWindow;

    [SerializeField] private FirstUnfinishedLevelButton _firstUnfinishedLevelButton;
    [SerializeField] private NextLevelButton _nextLevelButton;
    [SerializeField] private BoostInitializer _boostButtonInitializer;
    [SerializeField] private InGameShopInitializer _inGameShopInitializer;
    [SerializeField] private InAppPurchaseInitializer _inAppPurchaseInitializer;
    [SerializeField] private BoostBuyConfirmationWindow _boostBuyWindow;
    [SerializeField] private BackgroundMusicChanger _backgroundMusicChanger;
    [SerializeField] private GameplaySoundPlayer _gameplaySoundPlayer;
    [SerializeField] private DeviceStyleChangeInitializer _deviceStyleChangeInitializer;
    [SerializeField] private AngryBar _angryBar;
    [SerializeField] private LeanToken _currentLevelNumberToken;
    [SerializeField] private Transform _unitsLookAtPoint;
    [SerializeField] private AudioSource _unitMoveSound;
    [SerializeField] private UnitsMoveSoundPlayer _unitsMoveSoundPlayer;
    [SerializeField] private LeaderboardWindow _leaderboardWindow;
    [SerializeField] private LanguageChanger _languageChanger;
    [SerializeField] private GameProgressSaver _gameProgressSaver;
    [SerializeField] private CustomizationWindowInitializer _customizationWindowInitializer;
    [SerializeField] private BoostButtonActivator _boostButtonActivator;
    [SerializeField] private UIOrientationChanger _uIOrientationChanger;
    [SerializeField] private MenuWindow _inGameMenu;
    [SerializeField] private MenuTrainigSequence _menuTrainigSequence;
    [SerializeField] private List<LeaderboardTab> _leaderboardTabs;
    [SerializeField] private List<SoundToggleMuter> _soundToggleMuters;
    [SerializeField] private List<LoginButton> _loginButtons;
    [SerializeField] private List<LevelTabInitializer> _levelTabInitializers;

    private InAppPurchaseConsumeProvider _inAppConsumer;

    private void Start()
    {
        InitializeGame();
        _levelLoader.LoadMainMenu();
        _inAppConsumer.ConsumePurchase();
    }

    public void InitializeGame()
    {
        var interAdProvider = new InterstitialAdProvider();
        var rewardedAdProvider = new RewardedAdProvider();
        var authorizationProvider = new AuthorizationProvider();
        var saveProvider = new SaveProvider();
        _gameProgressSaver.Initialize(saveProvider);

        var inAppPurchaseProvider = new InAppPurchaseProvider();
        var leaderboardProvider = new LeaderboardProvider();

        var levelDataHolder = new LevelObjectsHolder();
        var unitMover = new UnitMover(_unitsLookAtPoint);
        var selectHandler = new SelectHandler(unitMover, _buferIslands, levelDataHolder);

        var defaultClickHandler = new DefaultClickHandler(selectHandler, _allIslandsAndUnitsLayer);
        var gameClickHandler = new ClickHandler(_inputHandler, _camera, defaultClickHandler);
        var progressStorage = new GameProgressStorage(_levelSettings, _faceSettings, _hatSettings, saveProvider, _gameProgressSaver);
        var leaderboardScoreCalculator = new LeaderboardScoreCalculator(progressStorage);

        var upgradesProvider = new UpgradesProvider(progressStorage);
        var boostAmountProvider = new BoostAmountProvider(progressStorage);
        var removeAdsProvider = new RemoveAdsProvider(progressStorage);

        var levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, progressStorage, leaderboardProvider,
                                                           _leaderboardSettings, leaderboardScoreCalculator);
        var walletProvider = new WalletProvider(progressStorage);
        var interAdOpener = new InterstitialAdOpener(_levelLoader, removeAdsProvider, interAdProvider, rewardedAdProvider);
        var soundVolumeProvider = new SoundVolumeProvider(_audioMixers, progressStorage);
        var levelEndSoundPlayer = new LevelEndSoundPlayer(_levelProgressTracker, _gameplaySoundPlayer);
        var angryTracker = new AngryTracker(levelDataHolder, _levelLoader);
        var localizationProvider = new LocalizationProvider();
        var customizationSettingsHolder = new CustomizationSettingsHolder(_materials, progressStorage, _faceSettings, _hatSettings);
        var trainigSequenceLoader = new TrainigSequenceLoader(_levelLoader, levelDataHolder, _buferIslands, selectHandler, unitMover,
                                                            _camera, _boostButtonActivator, _levelProgressTracker, _uIOrientationChanger,
                                                            progressStorage, _inGameMenu, _finalScoreWindow, _menuTrainigSequence);

        _nextLevelButton.Initialize(_levelLoader);
        _firstUnfinishedLevelButton.Initialize(progressStorage, _levelLoader);
        _customizationWindowInitializer.Initialize(progressStorage, _levelProgressTracker);

        _boostButtonInitializer.Initialize(unitMover, gameClickHandler, selectHandler, levelDataHolder,
                                           boostAmountProvider);
        _inGameShopInitializer.Initialize(upgradesProvider, boostAmountProvider, walletProvider);
        _boostBuyWindow.Initialize(boostAmountProvider, _boostSettings, walletProvider, rewardedAdProvider);
        _inAppPurchaseInitializer.Initialize(walletProvider, boostAmountProvider, removeAdsProvider, inAppPurchaseProvider);

        _walletView.Initialize(walletProvider);

        _levelProgressTracker.Initialize(progressStorage, levelDataHolder, unitMover, angryTracker);
        _finalScoreWindow.Initialize(progressStorage, _levelProgressTracker);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _levelProgressTracker,
                                _uiZoneActivator, levelDataHolder, _buferIslands, progressStorage,
                                _currentLevelNumberToken, _unitsLookAtPoint, customizationSettingsHolder);
        _buferIslands.Initialize(_levelSettings);
        _backgroundMusicChanger.Initialize(_levelLoader);
        _deviceStyleChangeInitializer.Initialize();
        _angryBar.Initialize(_levelProgressTracker, _levelLoader);
        _unitsMoveSoundPlayer.Initialize(unitMover, _unitMoveSound);
        _leaderboardWindow.Initialize(authorizationProvider);
        _languageChanger.Initialize(_localizationSettings, progressStorage, localizationProvider);

        foreach (LeaderboardTab leaderboardTab in _leaderboardTabs)
        {
            leaderboardTab.Initialize(leaderboardProvider, _leaderboardSettings);
        }

        foreach (SoundToggleMuter soundToggleMuter in _soundToggleMuters)
        {
            soundToggleMuter.Initialize(soundVolumeProvider);
        }

        foreach (LoginButton loginButton in _loginButtons)
        {
            loginButton.Initialize(authorizationProvider);
        }

        foreach (LevelTabInitializer levelTabInitializer in _levelTabInitializers)
        {
            levelTabInitializer.InitializeButtons(progressStorage, _levelLoader);
        }

        _testUI.Initialize(progressStorage, walletProvider);
        _inAppConsumer = new InAppPurchaseConsumeProvider();
    }
}
