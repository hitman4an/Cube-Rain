using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ColorChanger))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private ColorChanger _colorChanger;
    private bool _isCollidedWithPlain;
    private int _minDestroyDelay = 2;
    private int _maxDestroyDelay = 4;
    private int _minPositionCoordinate = -5;
    private int _maxPositionCoordinate = 5;
    private int _spawnHeight = 9;

    public event Action<Cube> DestroyCube;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
    }

    private void OnEnable()
    {
        _isCollidedWithPlain = false;

        transform.position = new Vector3(UnityEngine.Random.Range(_minPositionCoordinate, _maxPositionCoordinate),
                                    _spawnHeight,
                                    UnityEngine.Random.Range(_minPositionCoordinate, _maxPositionCoordinate));
        transform.rotation = Quaternion.Euler(Vector3.zero);

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    private void OnDisable()
    {
        StopCoroutine(InvokeDestroyEvent());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollidedWithPlain == false && collision.gameObject.TryGetComponent(out Plane plane))
        {
            _isCollidedWithPlain = true;
            _colorChanger.SetColor();

            StartCoroutine(InvokeDestroyEvent());            
        }
    }

    private IEnumerator InvokeDestroyEvent()
    {
        var wait = new WaitForSeconds(UnityEngine.Random.Range(_minDestroyDelay, _maxDestroyDelay + 1));
        yield return wait;
        DestroyCube?.Invoke(this);
    }
}
