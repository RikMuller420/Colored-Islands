using UnityEngine;

public class CustomizationWindowOpenerButton : MenuWindowOpener
{
    [SerializeField] private GameObject unusedMarkPanel;
    [SerializeField] private GameObject unusedMarkParticle;

    private UnitCustomizator _unitCustomizator;
    private CustomizationButtonAviabiltyUpdater _customizationAviabiltyUpdater;

    public void Initialize(UnitCustomizator unitCustomizator, CustomizationButtonAviabiltyUpdater customizationAviabiltyUpdater)
    {
        _unitCustomizator = unitCustomizator;
        _customizationAviabiltyUpdater = customizationAviabiltyUpdater;

        unitCustomizator.FaceUsed += UpdateUnusedMarkActivity;
        unitCustomizator.HatUsed += UpdateUnusedMarkActivity;
        customizationAviabiltyUpdater.HatButtonUnlocked += UpdateUnusedMarkActivity;
        customizationAviabiltyUpdater.FaceButtonUnlocked += UpdateUnusedMarkActivity;

        UpdateUnusedMarkActivity();
    }

    private void UpdateUnusedMarkActivity()
    {
        bool isAnyUnusedButton = false;

        foreach (HatSelectButton hatButton in _unitCustomizator.HatSelectButtons)
        {
            if (hatButton.IsUnusedMarkActive)
            {
                isAnyUnusedButton = true;

                break;
            }
        }

        foreach (FaceSelectButton faceButton in _unitCustomizator.FaceSelectButtons)
        {
            if (faceButton.IsUnusedMarkActive)
            {
                isAnyUnusedButton = true;

                break;
            }
        }

        unusedMarkPanel.SetActive(isAnyUnusedButton);
        unusedMarkParticle.SetActive(isAnyUnusedButton);
    }
}
