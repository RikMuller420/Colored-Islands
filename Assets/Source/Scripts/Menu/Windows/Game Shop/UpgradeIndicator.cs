using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Menu.Windows.GameShop
{

	public class UpgradeIndicator : MonoBehaviour
	{
	    [SerializeField] private List<GameObject> _upgradeMarks;

	    public void SetStage(int upgradeStage)
	    {
	        for (int i = 0; i < _upgradeMarks.Count; i++)
	        {
	            bool isActive = i < upgradeStage;
	            _upgradeMarks[i].SetActive(isActive);
	        }
	    }
	}

}
