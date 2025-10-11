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
    [SerializeField] private InAppSettings _inAppSettings;
    [SerializeField] private LevelRewardSettings _levelRewardSettings;
    [SerializeField] private UpgradeSettings _upgradeSettings;
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
    [SerializeField] private GameObject _mainMenuIslands;
    [SerializeField] private TrainingMenuUpdater _trainingMenuUpdater;
    [SerializeField] private RouletteWheel _rouletteWhell;
    [SerializeField] private RouletteRewardWindow _rouletteRewardWindow;
    [SerializeField] private AviableSpinCountView _aviableSpinCountView;
    [SerializeField] private RouletteWindowOpener _rouletteWindowOpener;
    [SerializeField] private RouletteWindow _rouletteWindow;
    [SerializeField] private Roulette _roulette;
    [SerializeField] private LevelRewardWindow _levelRewardWindow;
    [SerializeField] private AddMultipliedRewardWindow _addMultipliedRewardWindow;
    [SerializeField] private LeaderboardSynchronizer _leaderboardSynchronizer;
    [SerializeField] private ScreenSizeChangeTracker _screenSizeChangeTracker;
    [SerializeField] private CameraPositionChanger _cameraPositionChanger;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private List<LeaderboardTab> _leaderboardTabs;
    [SerializeField] private List<SoundToggleMuter> _soundToggleMuters;
    [SerializeField] private List<LoginButton> _loginButtons;
    [SerializeField] private List<LevelTabInitializer> _levelTabInitializers;

    private InAppPurchaseConsumeProvider _inAppConsumer;
    private GameProgressStorage _progressStorage;
    private TrainigSequenceLoader _trainigLoader;

    private void Start()
    {
        InitializeGame();
        _levelLoader.LoadMainMenu();
        _inAppConsumer.ConsumePurchase();

        _trainigLoader.TryLoadTrainingLevel();
    }

    public void InitializeGame()
    {
        var interAdProvider = new InterstitialAdProvider();
        var rewardedAdProvider = new RewardedAdProvider();
        var authorizationProvider = new AuthorizationProvider();
        var stickyAdProvider = new StickyAdProvider();
        var saveProvider = new SaveProvider();
        _gameProgressSaver.Initialize(saveProvider);

        var inAppPurchaseProvider = new InAppPurchaseProvider();
        var leaderboardProvider = new LeaderboardProvider();

        var levelDataHolder = new LevelObjectsHolder();
        var unitMover = new UnitMover(_unitsLookAtPoint);
        var selectHandler = new SelectHandler(unitMover, _buferIslands, levelDataHolder);

        var defaultClickHandler = new DefaultClickHandler(selectHandler, _allIslandsAndUnitsLayer);
        var gameClickHandler = new ClickHandler(_inputHandler, _camera, defaultClickHandler);
        _progressStorage = new GameProgressStorage(_levelSettings, _faceSettings, _hatSettings, _levelRewardSettings, saveProvider, _gameProgressSaver);
        var leaderboardScoreCalculator = new LeaderboardScoreCalculator(_progressStorage);

        var upgradesProvider = new UpgradesProvider(_progressStorage, _upgradeSettings);
        var boostAmountProvider = new BoostAmountProvider(_progressStorage);
        var removeAdsProvider = new RemoveAdsProvider(_progressStorage);
        var inAppByAddViewProvider = new InAppByAddViewProvider(_progressStorage, _inAppSettings);

        var levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, _progressStorage, leaderboardProvider,
                                                           _leaderboardSettings, leaderboardScoreCalculator);
        var walletProvider = new WalletProvider(_progressStorage);
        var interAdOpener = new InterstitialAdOpener(_levelLoader, removeAdsProvider, interAdProvider, rewardedAdProvider);
        var soundVolumeProvider = new SoundVolumeProvider(_audioMixers, _progressStorage);
        var levelEndSoundPlayer = new LevelEndSoundPlayer(_levelProgressTracker, _gameplaySoundPlayer);
        var angryTracker = new AngryTracker(levelDataHolder, _levelLoader, _levelProgressTracker, upgradesProvider);
        var localizationProvider = new LocalizationProvider();
        var customizationSettingsHolder = new CustomizationSettingsHolder(_materials, _progressStorage, _faceSettings, _hatSettings);
        _trainigLoader = new TrainigSequenceLoader(_levelLoader, levelDataHolder, _buferIslands, selectHandler, unitMover,
                                                   _camera, _boostButtonActivator, _levelProgressTracker, _uIOrientationChanger,
                                                   _progressStorage, _inGameMenu, _finalScoreWindow, _menuTrainigSequence,
                                                   _levelSettings, _screenSizeChangeTracker, _mainCanvas);

        _nextLevelButton.Initialize(_levelLoader);
        _firstUnfinishedLevelButton.Initialize(_progressStorage, _levelLoader);
        _customizationWindowInitializer.Initialize(_progressStorage, _levelProgressTracker);

        _boostButtonInitializer.Initialize(unitMover, gameClickHandler, selectHandler, levelDataHolder,
                                           boostAmountProvider);
        _inGameShopInitializer.Initialize(upgradesProvider, boostAmountProvider, walletProvider);
        _boostBuyWindow.Initialize(boostAmountProvider, _boostSettings, walletProvider, rewardedAdProvider);
        _inAppPurchaseInitializer.Initialize(walletProvider, boostAmountProvider, removeAdsProvider, inAppPurchaseProvider,
                                             inAppByAddViewProvider, rewardedAdProvider, _progressStorage);

        _walletView.Initialize(walletProvider);

        _levelProgressTracker.Initialize(_progressStorage, levelDataHolder, unitMover, angryTracker, upgradesProvider);
        _finalScoreWindow.Initialize(_progressStorage, _levelProgressTracker, _levelLoader, _levelRewardSettings);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _levelProgressTracker,
                                _uiZoneActivator, levelDataHolder, _buferIslands, upgradesProvider,
                                _currentLevelNumberToken, _unitsLookAtPoint, customizationSettingsHolder,
                                _mainMenuIslands);
        _buferIslands.Initialize(_levelSettings, unitMover);
        _backgroundMusicChanger.Initialize(_levelLoader);
        _deviceStyleChangeInitializer.Initialize();
        _angryBar.Initialize(_levelProgressTracker, _levelLoader, boostAmountProvider, _boostButtonInitializer.ObjectivesFreezeBoost);
        _unitsMoveSoundPlayer.Initialize(unitMover, _unitMoveSound);
        _leaderboardWindow.Initialize(authorizationProvider);
        _languageChanger.Initialize(_localizationSettings, _progressStorage, localizationProvider);
        _trainingMenuUpdater.Initialize(_progressStorage);
        _rouletteWhell.Initialize(_progressStorage, _faceSettings, upgradesProvider);
        _rouletteRewardWindow.Initialize(_faceSettings, _progressStorage, removeAdsProvider);
        _aviableSpinCountView.Initialize(_progressStorage);
        _rouletteWindowOpener.Initialize(_progressStorage);
        _rouletteWindow.Initialize(_progressStorage);
        _roulette.Initialize(_progressStorage);
        _levelRewardWindow.Initialize(_hatSettings, _progressStorage, rewardedAdProvider, upgradesProvider);
        _addMultipliedRewardWindow.Initialize(_hatSettings, upgradesProvider);
        _leaderboardSynchronizer.Initialize(leaderboardProvider, _leaderboardSettings, leaderboardScoreCalculator);
        _cameraPositionChanger.Initialize(_levelLoader, levelDataHolder, _buferIslands, _uIOrientationChanger, _screenSizeChangeTracker);

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
            levelTabInitializer.InitializeButtons(_progressStorage, _levelLoader);
        }

        _testUI.Initialize(_progressStorage, walletProvider);
        _inAppConsumer = new InAppPurchaseConsumeProvider();
    }
}
