using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ColorChanger))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private ColorChanger _colorChanger;
    private Rigidbody _rigidBody;
    private bool _isCollidedWithPlain;
    private int _minDestroyDelay = 2;
    private int _maxDestroyDelay = 4;
    private Coroutine _coroutine;

    public event Action<Cube> DestroyCube;

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
        DestroyCube?.Invoke(this);
    }
}
