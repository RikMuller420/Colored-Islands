using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterRipplesParticlePool : MonoBehaviour
{
	[SerializeField] private GameObject _smallRipplePrefab;
	[SerializeField] private GameObject _bigRipplePrefab;

	private List<GameObject> _smallRipples = new();
	private List<GameObject> _bigRipples = new();

	public GameObject GetSmallRipple() => GetRipple(_smallRipplePrefab, _smallRipples);
	public GameObject GetBigRipple() => GetRipple(_bigRipplePrefab, _bigRipples);

	private GameObject GetRipple(GameObject prefab, List<GameObject> collection)
	{
		GameObject freeRipple = collection.FirstOrDefault(ripple => ripple.activeSelf == false);

		if (freeRipple == null)
		{
			freeRipple = Instantiate(prefab, transform);
			collection.Add(freeRipple);
		}

		return freeRipple;
	}
}
