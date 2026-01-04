using UnityEngine;
using UnityEngine.UI;

public class RouletteRewardWindow : MenuWindow
{
    [SerializeField] private Slot _slot;
    [SerializeField] private Button _receiveButton;

    private GameProgressStorage _progressStorage;
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

    public void Initialize(UnitsFaceSettings unitsFaceSettings, GameProgressStorage progressStorage,
                        RemoveAdsProvider removeAdsProvider)
    {
        _progressStorage = progressStorage;
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
                _progressStorage.SetGoldAmount(_progressStorage.GoldAmount + _slot.GoldAmount);
                break;

            case RouletteRewardType.Face:
                _progressStorage.UnlockFace(_slot.FaceID);
                break;

            case RouletteRewardType.RemoveAds:
                _removeAdsProvider.RemoveAds();
                break;
        }

        _progressStorage.Save();
        Close();
    }
}
