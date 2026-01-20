using UnityEngine;

public class MenuInitializer : MonoBehaviour
{
    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private SettingsWindowInitializer _settingsWindowInitializer;
    [SerializeField] private RouletteWindowInitializer _rouletteWindowInitializer;
    [SerializeField] private GameShopInitializer _gameShopInitializer;
    [SerializeField] private CustomizationWindowInitializer _customizationWindowInitializer;
    [SerializeField] private LevelChangeEventTracker _levelChangeEventTracker;
    [SerializeField] private LevelRewardWindow _levelRewardWindow;
    [SerializeField] private LevelsWindow _levelsWindow;
    [SerializeField] private AngryBarView _angryBarView;
    [SerializeField] private PlayButton _playButton;
    [SerializeField] private TrainingMenuUpdater _trainingMenuUpdater;
    [SerializeField] private LanguageChanger _languageChanger;

    public void Initialize(UpgradesProvider upgradesProvider, AuthorizationProvider authorizationProvider,
                            RewardedAdProvider rewardedAdProvider,
                            BoostAmountProvider boostAmountProvider, WalletProvider walletProvider,
                            IBoostStopApplyedEvent freezeBoostApplyedEvent)
    {
        var interAdProvider = new InterstitialAdProvider();
        var removeAdsProvider = new RemoveAdsProvider(_playerData);
        var interAdOpener = new InterstitialAdOpener(_levelChangeEventTracker, removeAdsProvider, interAdProvider, rewardedAdProvider);

        _customizationWindowInitializer.Initialize();
        _angryBarView.Initialize(boostAmountProvider, freezeBoostApplyedEvent);    
        _levelRewardWindow.Initialize(rewardedAdProvider, upgradesProvider);
        _levelsWindow.Initialize(_playerData);
        _gameShopInitializer.Initialize(upgradesProvider, rewardedAdProvider, boostAmountProvider, removeAdsProvider, walletProvider);
        _settingsWindowInitializer.Initialize(authorizationProvider);
        _rouletteWindowInitializer.Initialize(upgradesProvider, removeAdsProvider);
        _playButton.Initialize(_playerData);
        _trainingMenuUpdater.Initilize(_playerData);
        _languageChanger.Initialize();
    }
}
