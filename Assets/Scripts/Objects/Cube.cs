using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
[RequireComponent(typeof(Rigidbody))]
public class Cube: MonoBehaviour, IObjectable<Cube>
{
    public event Action<Cube> DestroyFigure;

    private ColorChanger _colorChanger;
    private Rigidbody _rigidBody;
    private Coroutine _coroutine;

    private int _minDestroyDelay = 2;
    private int _maxDestroyDelay = 4;
    private bool _isCollidedWithPlain;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _isCollidedWithPlain = false;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollidedWithPlain == false && collision.gameObject.TryGetComponent(out Plane plane))
        {
            _isCollidedWithPlain = true;
            _colorChanger.SetColor();

            _coroutine = StartCoroutine(InvokeDestroyEvent());            
        }
    }

    private IEnumerator InvokeDestroyEvent()
    {
        var wait = new WaitForSeconds(UnityEngine.Random.Range(_minDestroyDelay, _maxDestroyDelay + 1));

        yield return wait;

        DestroyFigure?.Invoke(this);
    }
}
