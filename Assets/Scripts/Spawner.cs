using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;
    private bool _isActive = true;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => ReleaseObjectAction(obj),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(GetCube());
    }

    private void OnDisable()
    {
        StopCoroutine(GetCube());
        _isActive = false;
    }

    private IEnumerator GetCube()
    {
        while (_isActive)
        {
            var wait = new WaitForSeconds(_repeatRate);
            Cube obj = _pool.Get();

            obj.DestroyCube += ReleaseObject;

            yield return wait;
        }
    }

    private void ReleaseObject(Cube obj)
    {
        obj.DestroyCube -= ReleaseObject;
        _pool.Release(obj);
    }

    private void ReleaseObjectAction(Cube obj)
    {
        obj.gameObject.SetActive(false);
    }
}
