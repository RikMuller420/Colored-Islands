using SlimeGround.Data.Saves;
using SlimeGround.Data.ScriptableObjects.UnitFaces;
using SlimeGround.Menu.Ads;
using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Roulette
{
	public class RouletteRewardWindow : MenuWindow
	{
	    [SerializeField] private Slot _slot;
	    [SerializeField] private Button _receiveButton;

	    private PlayerDataProvider _playerDataProvider;
	    private RemoveAdsProvider _removeAdsProvider;

	    protected override void OnEnable()
	    {
	        _receiveButton.onClick.AddListener(ReceiveReward);
	        base.OnEnable();
	    }

	    protected override void OnDisable()
	    {
	        _receiveButton.onClick.RemoveListener(ReceiveReward);
	        base.OnDisable();
	    }

	    public void Initialize(UnitsFaceSettings unitsFaceSettings, PlayerDataProvider playerDataProvider,
	                        RemoveAdsProvider removeAdsProvider)
	    {
	        _playerDataProvider = playerDataProvider;
	        _removeAdsProvider = removeAdsProvider;
	        _slot.Initialize(unitsFaceSettings);
	    }

	    public void Open(Slot winnedSlot)
	    {
	        switch (winnedSlot.RouletteRewardType)
	        {
	            case RouletteRewardType.Gold:
	                _slot.ActivateGoldIcon(winnedSlot.GoldAmount);
	                break;

	            case RouletteRewardType.Face:
	                _slot.ActivateFaceIcon(winnedSlot.FaceID);
	                break;

	            case RouletteRewardType.RemoveAds:
	                _slot.ActivateRemoveAdsIcon();
	                break;
	        }

	        base.OpenUnclosableWindow();
	    }

	    private void ReceiveReward()
	    {
	        switch (_slot.RouletteRewardType)
	        {
	            case RouletteRewardType.Gold:
	                _playerDataProvider.SetGoldAmount(_playerDataProvider.GoldAmount + _slot.GoldAmount);
	                break;

	            case RouletteRewardType.Face:
	                _playerDataProvider.UnlockFace(_slot.FaceID);
	                break;

	            case RouletteRewardType.RemoveAds:
	                _removeAdsProvider.RemoveAds();
	                break;
	        }

	        _playerDataProvider.Save();
	        Close();
	    }
	}
}
