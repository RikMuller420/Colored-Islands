using System;
using System.Linq;

public class InAppByAddViewProvider
{
    private GameProgressStorage _gameProgressStorage;
    private InAppSettings _inAppSettings;

    public event Action<InAppType> ProgressChanged;
    public event Action<string> InAppProgressFinished;

    public InAppByAddViewProvider(GameProgressStorage gameProgressStorage, InAppSettings inAppSettings)
    {
        _gameProgressStorage = gameProgressStorage;
        _inAppSettings = inAppSettings;
    }

    public int EarnedInAppWithAddProgress(InAppType inAppType) => _gameProgressStorage.GetEarnedInAppWithAddProgress(inAppType);

    public void AddUpgradeStage(InAppType inAppType)
    {
        int upgradeStage = EarnedInAppWithAddProgress(inAppType);
        upgradeStage++;
        _gameProgressStorage.SetEarnInAppWithAddProgress(inAppType, upgradeStage);
        _gameProgressStorage.Save();
        ProgressChanged?.Invoke(inAppType);

        InAppSettingsData inAppSettings = _inAppSettings.InApps.FirstOrDefault(inApp => inApp.Type == inAppType);

        if (upgradeStage == inAppSettings.EarnWithAddViewCount)
        {
            InAppProgressFinished?.Invoke(inAppSettings.Id);
        }
    }
}
