using System;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T>: MonoBehaviour where T : MonoBehaviour, IObjectable<T>
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected int _poolCapacity = 5;
    [SerializeField] protected int _poolMaxSize = 5;

    public event Action ObjectCreated;
    public event Action ObjectReceived;
    public event Action<T> ObjectReleased;

    protected ObjectPool<T> Pool;
    protected bool IsActive = true;

    private void Awake()
    {
        Pool = new ObjectPool<T>(
        createFunc: () => CreateObject(_prefab),
        actionOnGet: (obj) => ClearObjectTransform(obj),
        actionOnRelease: (obj) => ReleaseObjectAction(obj),
        actionOnDestroy: (obj) => Destroy(obj.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    protected T CreateObject(T prefab)
    {
        T obj = Instantiate(prefab);

        ObjectCreated?.Invoke();

        return obj;

    }

    protected virtual void ClearObjectTransform(T obj)
    {
        ObjectReceived?.Invoke();
    }

    protected void ReleaseObjectAction(T obj)
    {
        obj.gameObject.SetActive(false);
        ObjectReleased?.Invoke(obj);
    }

    protected void ReleaseObject(T obj)
    {
        obj.DestroyFigure -= ReleaseObject;
        Pool.Release(obj);
    }
}
