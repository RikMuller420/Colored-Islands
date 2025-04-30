using UnityEngine;

public class UnitCreator : MonoBehaviour
{
    [SerializeField] private Unit _prefab;

    public Unit Create()
    {
        Unit unit = Instantiate(_prefab);

        return unit;
    }
}
