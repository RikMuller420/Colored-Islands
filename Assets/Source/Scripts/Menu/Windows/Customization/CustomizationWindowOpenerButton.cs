using SlimeGround.Menu.Extensions.Windows;
using UnityEngine;

namespace SlimeGround.Menu.Windows.Customization
{
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

			_unitCustomizator.FaceUsed += UpdateUnusedMarkActivity;
			_unitCustomizator.HatUsed += UpdateUnusedMarkActivity;
			_customizationAviabiltyUpdater.HatButtonUnlocked += UpdateUnusedMarkActivity;
			_customizationAviabiltyUpdater.FaceButtonUnlocked += UpdateUnusedMarkActivity;

	        UpdateUnusedMarkActivity();
	    }

		public void Dispose()
		{
			_unitCustomizator.FaceUsed -= UpdateUnusedMarkActivity;
			_unitCustomizator.HatUsed -= UpdateUnusedMarkActivity;
			_customizationAviabiltyUpdater.HatButtonUnlocked -= UpdateUnusedMarkActivity;
			_customizationAviabiltyUpdater.FaceButtonUnlocked -= UpdateUnusedMarkActivity;
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
}
