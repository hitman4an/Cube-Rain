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

    private Coroutine _coroutine;

    private void Start()
    {
        _coroutine = StartCoroutine(GetObject());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(GetObject());
        
        IsActive = false;
    }

    protected override void ClearObjectTransform(Cube obj)
    {
        obj.transform.position = new Vector3(UnityEngine.Random.Range(_minPositionCoordinate, _maxPositionCoordinate),
                                        _spawnHeight,
                                        UnityEngine.Random.Range(_minPositionCoordinate, _maxPositionCoordinate));
        obj.gameObject.SetActive(true);

        base.ClearObjectTransform(obj);
    }

    private IEnumerator GetObject()
    {
        var wait = new WaitForSeconds(_repeatRate);

        while (IsActive)
        {
            Cube obj = Pool.Get();

            obj.DestroyFigure += ReleaseObject;            

            yield return wait;
        }
    }
}
