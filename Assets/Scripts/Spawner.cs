using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _repeatRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<GameObject> _pool;
    private int _minPositionCoordinate = -5;
    private int _maxPositionCoordinate = 5;
    private int _spawnHeight = 9;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => ActionOnRelease(obj),
        actionOnDestroy: (obj) => ActionOnDestroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = new Vector3(Random.Range(_minPositionCoordinate, _maxPositionCoordinate), 
                                            _spawnHeight, 
                                            Random.Range(_minPositionCoordinate, _maxPositionCoordinate));
        obj.transform.rotation = Quaternion.Euler(0, 0, 0);
        obj.GetComponent<Renderer>().material.color = Color.white;
        obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _repeatRate);
    }

    private void GetCube()
    {
        GameObject obj = _pool.Get();

        obj.GetComponent<Cube>().DestroyCube += ReleaseObject;
    }

    private void ReleaseObject(GameObject obj)
    {        
        _pool.Release(obj);
    }
    private void ActionOnRelease(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void ActionOnDestroy(GameObject obj)
    {
        obj.GetComponent<Cube>().DestroyCube -= ReleaseObject;
        Destroy(obj);
    }
}
