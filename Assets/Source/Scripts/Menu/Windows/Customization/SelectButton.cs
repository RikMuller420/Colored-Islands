using UnityEngine;
using UnityEngine.UI;

namespace SlimeGround.Menu.Windows.Customization
{

	public class SelectButton : MonoBehaviour
	{
	    [SerializeField] private Button _button;
	    [SerializeField] private GameObject _lockIcon;
	    [SerializeField] private GameObject _selectedFrame;
	    [SerializeField] private GameObject _markerUnused;

	    protected Button Button => _button;

	    public bool IsUnusedMarkActive => _markerUnused.activeSelf;

	    public void Initialize(bool isAviable, bool wasUsed)
	    {
	        if (isAviable)
	        {
	            SetUnlockedStyle();

	            if (wasUsed == false)
	            {
	                ActivateUnusedMark();
	            }
	        }
	        else
	        {
	            SetLockedStyle();
	        }
	    }

	    public void SetLockedStyle()
	    {
	        _button.interactable = false;
	        _lockIcon.SetActive(true);
	    }

	    public void SetUnlockedStyle()
	    {
	        _button.interactable = true;
	        _lockIcon.SetActive(false);
	    }

	    public void SetSelectedStyle()
	    {
	        _button.interactable = false;
	        _selectedFrame.SetActive(true);
	    }

	    public void SetNonSelectedStyle()
	    {
	        _button.interactable = true;
	        _selectedFrame.SetActive(false);
	    }

	    public void ActivateUnusedMark()
	    {
	        _markerUnused.SetActive(true);
	    }

	    public void DeactivateUnusedMark()
	    {
	        _markerUnused.SetActive(false);
	    }
	}

}
