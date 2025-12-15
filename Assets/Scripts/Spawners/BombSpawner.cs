using System;
using UnityEngine;
using UnityEngine.Pool;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private Spawner<Cube> _cubeSpawner;

    private Cube _cube = null;

    private void Awake()
    {
        _pool = new ObjectPool<Bomb>(
            createFunc: () => CreateObject(_prefab),
            actionOnGet: (obj) => ClearObjectTransform(obj),
            actionOnRelease: (obj) => ReleaseObjectAction(obj),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void OnEnable()
    {
        _cubeSpawner.ObjectReleased += GetObject;
    }

    private void ClearObjectTransform(Bomb obj)
    {
        if (_cube != null)
        {
            obj.transform.position = _cube.transform.position;
            _cube = null;
            obj.gameObject.SetActive(true);
        }
    }

    private void GetObject(Cube cube)
    {
        Bomb obj = _pool.Get();

        _cube = cube;
        obj.DestroyFigure += ReleaseObject;
        InvokeObjectReceived();
    }
}
