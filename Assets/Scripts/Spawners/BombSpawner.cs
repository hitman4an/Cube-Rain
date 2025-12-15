using UnityEngine;
using UnityEngine.Pool;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private Spawner<Cube> _cubeSpawner;

    private void Awake()
    {
        Pool = new ObjectPool<Bomb>(
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
        _cubeSpawner.ObjectReleased += TakeObjectFromPool;
    }

    private void OnDisable()
    {
        _cubeSpawner.ObjectReleased -= TakeObjectFromPool;
    }

    private void TakeObjectFromPool(Cube cube)
    {
        Bomb obj = Pool.Get();
        
        obj.transform.position = cube.transform.position;
        obj.gameObject.SetActive(true);
        obj.DestroyFigure += ReleaseObject;
    }
}
