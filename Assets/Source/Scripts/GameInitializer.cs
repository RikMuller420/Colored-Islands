using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private TestUI _testUI;

    [Header("Settings")]
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private PaintMaterials _materials;
    [SerializeField] private BoostSettings _boostSettings;
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
    [SerializeField] private LastLevelTextUpdater _lastLevelTextUpdater;
    [SerializeField] private FinalScoreWindow _finalScoreWindow;

    [SerializeField] private FirstUnfinishedLevelButton _firstUnfinishedLevelButton;
    [SerializeField] private NextLevelButton _nextLevelButton;
    [SerializeField] private BoostInitializer _boostButtonInitializer;
    [SerializeField] private InGameShopInitializer _inGameShopInitializer;
    [SerializeField] private InAppPurchaseInitializer _inAppPurchaseInitializer;
    [SerializeField] private BoostBuyConfirmationWindow _boostBuyWindow;
    [SerializeField] private LoginButton _loginButton;
    [SerializeField] private List<LeaderboardTab> _leaderboardTabs;

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
        var inAppPurchaseProvider = new InAppPurchaseProvider();
        var leaderboardProvider = new LeaderboardProvider();

        var levelDataHolder = new LevelObjectsHolder();
        var unitMover = new UnitMover();
        var selectHandler = new SelectHandler(unitMover, _buferIslands, levelDataHolder);

        var defaultClickHandler = new DefaultClickHandler(selectHandler, _allIslandsAndUnitsLayer);
        var gameClickHandler = new ClickHandler(_inputHandler, _camera, defaultClickHandler);
        var gameProgressStorage = new GameProgressStorage(_levelSettings, saveProvider);

        var upgradesProvider = new UpgradesProvider(gameProgressStorage);
        var boostAmountProvider = new BoostAmountProvider(gameProgressStorage);
        var removeAdsProvider = new RemoveAdsProvider(gameProgressStorage);

        var levelProgressUpdater = new GameProgressUpdater(_levelProgressTracker, gameProgressStorage, leaderboardProvider);
        var walletProvider = new WalletProvider(gameProgressStorage);
        var interAdOpener = new InterstitialAdOpener(_levelLoader, removeAdsProvider, interAdProvider, rewardedAdProvider);


        _nextLevelButton.Initialize(_levelLoader);
        _firstUnfinishedLevelButton.Initialize(gameProgressStorage, _levelLoader);



        _boostButtonInitializer.Initialize(unitMover, gameClickHandler, selectHandler, levelDataHolder,
                                           boostAmountProvider);
        _inGameShopInitializer.Initialize(upgradesProvider, boostAmountProvider, walletProvider);
        _boostBuyWindow.Initialize(boostAmountProvider, _boostSettings, walletProvider, rewardedAdProvider);
        _inAppPurchaseInitializer.Initialize(walletProvider, boostAmountProvider, removeAdsProvider, inAppPurchaseProvider);

        _walletView.Initialize(walletProvider);
        _lastLevelTextUpdater.Initialize(gameProgressStorage);
        _loginButton.Initialize(authorizationProvider);

        _levelProgressTracker.Initialize(gameProgressStorage, levelDataHolder, unitMover);
        _finalScoreWindow.Initialize(gameProgressStorage, _levelProgressTracker);

        _levelLoader.Initialize(_levelSettings, _unitsPool, _materials, _levelProgressTracker,
                                _uiZoneActivator, levelDataHolder, _buferIslands, gameProgressStorage);
        _buferIslands.Initialize(_levelSettings);

        foreach (LeaderboardTab leaderboardTab in _leaderboardTabs)
        {
            leaderboardTab.Initialize(leaderboardProvider);
        }

        _testUI.Initialize(gameProgressStorage, walletProvider);


        _inAppConsumer = new InAppPurchaseConsumeProvider();
    }
}
