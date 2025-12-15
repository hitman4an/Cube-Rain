using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorChanger))]
[RequireComponent(typeof(Rigidbody))]
public class Figure: MonoBehaviour
{
    public event Action<Figure> DestroyFigure;

    protected ColorChanger _colorChanger;
    protected Rigidbody _rigidBody;
    protected Coroutine _coroutine;

    protected int _minDestroyDelay = 2;
    protected int _maxDestroyDelay = 4;
    
    protected virtual void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    public void InvokeDestroyFigure()
    {
        DestroyFigure?.Invoke(this);
    }
}
