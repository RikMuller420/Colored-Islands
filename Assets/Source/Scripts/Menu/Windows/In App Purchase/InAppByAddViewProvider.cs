using System;
using System.Linq;

public class InAppByAddViewProvider
{
    private PlayerDataProvider _playerData;
    private InAppSettings _inAppSettings;

    public event Action<InAppType> ProgressChanged;
    public event Action<string> InAppProgressFinished;

    public InAppByAddViewProvider(PlayerDataProvider playerData, InAppSettings inAppSettings)
    {
        _playerData = playerData;
        _inAppSettings = inAppSettings;
    }

    public int EarnedInAppWithAddProgress(InAppType inAppType) => _playerData.GetEarnedInAppWithAddProgress(inAppType);

    public void AddUpgradeStage(InAppType inAppType)
    {
        int upgradeStage = EarnedInAppWithAddProgress(inAppType);
        upgradeStage++;
        _playerData.SetEarnInAppWithAddProgress(inAppType, upgradeStage);
        _playerData.Save();
        ProgressChanged?.Invoke(inAppType);

        InAppSettingsData inAppSettings = _inAppSettings.InApps.FirstOrDefault(inApp => inApp.Type == inAppType);

        if (upgradeStage == inAppSettings.EarnWithAddViewCount)
        {
            InAppProgressFinished?.Invoke(inAppSettings.Id);
        }
    }
}
