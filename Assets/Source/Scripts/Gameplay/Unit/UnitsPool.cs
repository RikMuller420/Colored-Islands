using System.Collections.Generic;
using UnityEngine;

namespace SlimeGround.Gameplay.Units
{
	public class UnitsPool : MonoBehaviour
	{
		[SerializeField] private Unit _prefab;

		private List<Unit> _activeObjects = new List<Unit>();
		private List<Unit> _inactiveObjects = new List<Unit>();

		public Unit Get()
		{
			Unit unit;

			if (_inactiveObjects.Count == 0)
			{
				unit = CreateUnit();	
			}
			else
			{
				unit = _inactiveObjects[0];
				_inactiveObjects.RemoveAt(0);
			}

			unit.gameObject.SetActive(true);
			_activeObjects.Add(unit);

			return unit;
		}

		public void ReleaseActiveUnits()
		{
			for (int i = _activeObjects.Count - 1; i >= 0; i--)
			{
				Unit unit = _activeObjects[i];
				unit.gameObject.SetActive(false);
				_activeObjects.Remove(unit);
				_inactiveObjects.Add(unit);
			}
		}

		private Unit CreateUnit()
		{
			Unit instance = Instantiate(_prefab);
			instance.gameObject.SetActive(false);

			return instance;
		}
	}
}
