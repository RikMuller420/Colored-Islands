using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Data.ScriptableObjects.InApps
{

	[CreateAssetMenu(fileName = "InAppSettings", menuName = "Custom/InAppSettings")]
	public class InAppSettings : ScriptableObject
	{
	    [SerializeField] private InAppSettingsData[] _inApps;

	    public IEnumerable<InAppSettingsData> InApps => _inApps;
	}

}
