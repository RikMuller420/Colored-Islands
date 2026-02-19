using SlimeGround.Core.CameraSystem;
using SlimeGround.Core.InputHandling;
using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Data.ScriptableObjects.Upgrades;
using SlimeGround.Effects;
using SlimeGround.Gameplay;
using SlimeGround.Gameplay.Boosts;
using SlimeGround.Gameplay.Levels;
using SlimeGround.Gameplay.Training;
using SlimeGround.Gameplay.Units;
using SlimeGround.Integration.Ads;
using SlimeGround.Integration.Authorization;
using SlimeGround.Integration.InAppPurchase;
using SlimeGround.Integration.Leaderboards;
using SlimeGround.Integration.Metrics;
using SlimeGround.Menu;
using SlimeGround.Menu.Extensions.DeviceStyle;
using SlimeGround.Menu.Windows.GameShop.Upgrades;
using SlimeGround.Menu.Windows.Leaderboard;
using UnityEngine;

namespace SlimeGround
{
	public class GameInitializer : MonoBehaviour
	{
	    [Header("Settings")]
	    [SerializeField] private LevelSettings _levelSettings;
	    [SerializeField] private UpgradeSettings _upgradeSettings;
	    [SerializeField] private LayerMask _allIslandsAndUnitsLayer;

	    [Header("Component Initializers")]
	    [SerializeField] private MetricInitializer _metricInitializer;
	    [SerializeField] private BoostInitializer _boostInitializer;
	    [SerializeField] private LeaderboardInitializer _leaderboardInitializer;
	    [SerializeField] private MenuInitializer _menuInitializer;
	    [SerializeField] private EffectsInitializer _effectsInitializer;
	    [SerializeField] private GameplayInitializer _gameplayInitializer;
	    [SerializeField] private DeviceStyleChangeInitializer _deviceStyleChangeInitializer;

	    [Header("Gameplay")]
	    [SerializeField] private PlayerDataProvider _playerData;
	    [SerializeField] private InputHandler _inputHandler;
	    [SerializeField] private LevelLoader _levelLoader;
	    [SerializeField] private Camera _camera;
	    [SerializeField] private Transform _unitsLookAtPoint;
	    [SerializeField] private CameraPositionChanger _cameraPositionChanger;
	    [SerializeField] private TrainigSequenceLoader _trainigLoader;

		private BoostAmountProvider _boostAmountProvider;
		private WalletProvider _walletProvider;
		private RewardedAdProvider _rewardedAdProvider;
		private LeaderboardProvider _leaderboardProvider;
		private AuthorizationProvider _authorizationProvider;
		private ClickHandler _clickHandler;

		private void Start()
	    {
	        InitializeGame();

	        _levelLoader.LoadMainMenu();

	        var inAppConsumer = new InAppPurchaseConsumeProvider();
	        inAppConsumer.ConsumePurchase();

	        _trainigLoader.TryLoadTrainingLevel();
	    }

		private void OnDestroy()
		{
			_boostAmountProvider.Dispose();
			_walletProvider.Dispose();
			_leaderboardProvider.Dispose();
			_rewardedAdProvider.Dispose();
			_authorizationProvider.Dispose();

			_clickHandler.Dispose();
			_gameplayInitializer.Dispose();
			_levelLoader.Dispose();
			_boostInitializer.Dispose();
			_menuInitializer.Dispose();
			_trainigLoader.Dispose();
			_effectsInitializer.Dispose();
			_leaderboardInitializer.Dispose();
		}

		private void InitializeGame()
	    {
	        _playerData.Initialize();

	        var upgradesProvider = new UpgradesProvider(_playerData, _upgradeSettings);
			_boostAmountProvider = new BoostAmountProvider(_playerData);
			_walletProvider = new WalletProvider(_playerData);
			_rewardedAdProvider = new RewardedAdProvider();
			_authorizationProvider = new AuthorizationProvider();
			_leaderboardProvider = new LeaderboardProvider();

	        var levelDataHolder = new LevelDataHolder(_levelSettings.MainMenuSettings);

	        var unitMover = new UnitMover(_unitsLookAtPoint);
			_clickHandler = new ClickHandler(unitMover, _inputHandler, _camera,
	                                         _allIslandsAndUnitsLayer,
	                                         out IUnitsSelectedEvent unitsSelectedEvent);

	        _gameplayInitializer.Initialize(upgradesProvider, _leaderboardProvider,
	                                        levelDataHolder, unitMover);

	        _levelLoader.Initialize(upgradesProvider, unitMover, levelDataHolder);

	        _boostInitializer.Initialize(unitMover, _clickHandler, levelDataHolder,
										 _boostAmountProvider, _walletProvider, _rewardedAdProvider,
	                                     out IBoostStopApplyedEvent freezeBoostApplyedEvent);

	        _menuInitializer.Initialize(upgradesProvider, _authorizationProvider, _rewardedAdProvider,
										_boostAmountProvider, _walletProvider, freezeBoostApplyedEvent);

	        _trainigLoader.Initilize(unitsSelectedEvent, unitMover);
	        _deviceStyleChangeInitializer.Initialize();
	        _metricInitializer.Initilize(levelDataHolder);
	        _effectsInitializer.Initialize(unitMover);
	        _cameraPositionChanger.Initialize(levelDataHolder);
	        _leaderboardInitializer.Initialize(_leaderboardProvider, _authorizationProvider);
	    }
	}
}
