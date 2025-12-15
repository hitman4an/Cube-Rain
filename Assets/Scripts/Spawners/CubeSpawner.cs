using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : Spawner<Cube>
{
    [SerializeField] private float _repeatRate = 1f;

    private int _minPositionCoordinate = -5;
    private int _maxPositionCoordinate = 5;
    private int _spawnHeight = 9;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => CreateObject(_prefab),
            actionOnGet: (obj) => ClearObjectTransform(obj),
            actionOnRelease: (obj) => ReleaseObjectAction(obj),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(GetObject());
    }

    private void OnDisable()
    {
        StopCoroutine(GetObject());
        _isActive = false;
    }

    private void ClearObjectTransform(Cube obj)
    {
        obj.transform.position = new Vector3(UnityEngine.Random.Range(_minPositionCoordinate, _maxPositionCoordinate),
                                        _spawnHeight,
                                        UnityEngine.Random.Range(_minPositionCoordinate, _maxPositionCoordinate));
        obj.gameObject.SetActive(true);
    }

    private IEnumerator GetObject()
    {
        while (_isActive)
        {
            var wait = new WaitForSeconds(_repeatRate);
            Cube obj = _pool.Get();

            obj.DestroyFigure += ReleaseObject;
            InvokeObjectReceived();

            yield return wait;
        }
    }
}
