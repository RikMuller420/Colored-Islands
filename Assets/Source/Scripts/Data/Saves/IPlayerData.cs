namespace SlimeGround.Data.Saves
{
	public interface IPlayerData 
	{
		public CustomizationDataProvider Customization { get;}
		public GameSettingsProvider Settings { get;  }
		public PlayerResourcesProvider Resources { get; }
		public GameProgressProvider Progress { get; }
	}
}
