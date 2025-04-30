using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    [SerializeField] private Unit _prefab;

    public Unit Create(Transform spawnPoint)
    {
        Unit unit = Instantiate(_prefab);
        unit.transform.position = spawnPoint.position;

        return unit;
    }
}
