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

    protected ObjectPool<T> _pool;
    protected bool _isActive = true;

    public void InvokeObjectReceived()
    {
        ObjectReceived?.Invoke();
    }

    protected T CreateObject(T prefab)
    {
        T obj = Instantiate(prefab);

        ObjectCreated?.Invoke();

        return obj;

    }
    protected void ReleaseObjectAction(T obj)
    {
        obj.gameObject.SetActive(false);
        ObjectReleased?.Invoke(obj);
    }

    protected void ReleaseObject(T obj)
    {
        obj.DestroyFigure -= ReleaseObject;
        _pool.Release(obj);
    }
}
