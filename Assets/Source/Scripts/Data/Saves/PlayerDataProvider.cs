using SlimeGround.Data.ScriptableObjects.Hats;
using SlimeGround.Data.ScriptableObjects.LevelRewards;
using SlimeGround.Data.ScriptableObjects.Levels;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using SlimeGround.Integration.Saves;
using UnityEngine;

namespace SlimeGround.Data.Saves
{
	public class PlayerDataProvider : MonoBehaviour, IPlayerData
	{
	    [SerializeField] private LevelSettings _levelSettings;
	    [SerializeField] private UnitsFaceSettings _unitsFaceSettings;
	    [SerializeField] private UnitsHatSettings _unitsHatSettings;
	    [SerializeField] private LevelRewardSettings _levelRewardSettings;
	    [SerializeField] private PlayerDataSaver _playerDataSaver;

	    private PlayerData _playerData;
		private SaveProvider _saveProvider;

		public CustomizationDataProvider Customization { get; private set; }
		public GameSettingsProvider Settings { get; private set; }
		public PlayerResourcesProvider Resources { get; private set; }
		public GameProgressProvider Progress { get; private set; }

		public void Initialize()
		{
			_saveProvider = new SaveProvider();
			_playerDataSaver.Initialize(_saveProvider);

			PlayerDataReader playerDataReader = new PlayerDataReader(_levelSettings, _unitsFaceSettings, _unitsHatSettings,
																	 _levelRewardSettings, _saveProvider);
			_playerData = playerDataReader.GetData();

			Customization = new CustomizationDataProvider(_playerData);
			Settings = new GameSettingsProvider(_playerData);
			Resources = new PlayerResourcesProvider(_playerData);
			Progress = new GameProgressProvider(_playerData);

			Save();
		}

		public void Save() => _playerDataSaver.SaveWhileEnabled(_playerData);
	}
}
