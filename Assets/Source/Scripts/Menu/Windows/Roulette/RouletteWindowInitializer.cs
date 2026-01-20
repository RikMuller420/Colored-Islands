using UnityEngine;

public class RouletteWindowInitializer : MonoBehaviour
{
    [SerializeField] private UnitsFaceSettings _faceSettings;

    [SerializeField] private PlayerDataProvider _playerData;
    [SerializeField] private RouletteWheel _rouletteWhell;
    [SerializeField] private RouletteRewardWindow _rouletteRewardWindow;
    [SerializeField] private AviableSpinCountView _aviableSpinCountView;
    [SerializeField] private RouletteWindowOpener _rouletteWindowOpener;
    [SerializeField] private RouletteWindow _rouletteWindow;
    [SerializeField] private Roulette _roulette;

    public void Initialize(UpgradesProvider upgradesProvider, RemoveAdsProvider removeAdsProvider)
    {
        _rouletteWhell.Initialize(_playerData, _faceSettings, upgradesProvider);
        _rouletteRewardWindow.Initialize(_faceSettings, _playerData, removeAdsProvider);
        _aviableSpinCountView.Initialize(_playerData);
        _rouletteWindowOpener.Initialize(_playerData);
        _rouletteWindow.Initialize(_playerData);
        _roulette.Initialize(_playerData);
    }
}
