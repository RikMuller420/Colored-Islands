using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GenericPool<T> : MonoBehaviour where T : PoolableObject
{
    protected ObjectPool<T> _pool;

    [SerializeField] private T _prefab;

    private int _poolCapacity = 50;
    private int _poolMaxSize = 50;
    private List<T> _activeObjects = new List<T>();

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => InstantiatePrefab(),
            actionOnGet: (instance) => OnGet(instance),
            actionOnRelease: (instance) => OnRelease(instance),
            actionOnDestroy: (instance) => DestroyObject(instance),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
        );
    }

    public T Get()
    {
        return _pool.Get();
    }

    public void ReleaseActiveObjects()
    {
        for (int i = _activeObjects.Count - 1; i >= 0; i--)
        {
            _pool.Release(_activeObjects[i]);
        }
    }

    private T InstantiatePrefab()
    {
        T instance = Instantiate(_prefab);
        instance.gameObject.SetActive(false);
        instance.Deactivated += ReleaseInPool;

        return instance;
    }

    private void OnGet(T instance)
    {
        instance.gameObject.SetActive(true);
        _activeObjects.Add(instance);
    }

    private void OnRelease(T instance)
    {
        instance.gameObject.SetActive(false);
        _activeObjects.Remove(instance);
    }

    private void ReleaseInPool(PoolableObject instance)
    {
        _pool.Release(instance as T);
    }

    private void DestroyObject(PoolableObject instance)
    {
        instance.Deactivated -= ReleaseInPool;
        Destroy(instance);
    }
}
