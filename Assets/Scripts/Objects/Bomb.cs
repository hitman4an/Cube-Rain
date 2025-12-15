using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Exploder))]
[RequireComponent(typeof(ColorChanger))]
public class Bomb : MonoBehaviour, IObjectable<Bomb>
{
    public event Action<Bomb> DestroyFigure;

    private Exploder _exploder;
    private ColorChanger _colorChanger;
    private Coroutine _coroutine;

    private int _minDestroyDelay = 2;
    private int _maxDestroyDelay = 4;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _exploder = GetComponent<Exploder>();
    }
    private void OnEnable()
    {
        _coroutine = StartCoroutine(Blow());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator Blow()
    {
        int blowCooldown = UnityEngine.Random.Range(_minDestroyDelay, _maxDestroyDelay + 1);
        var wait = new WaitForSeconds(blowCooldown);

        _colorChanger.SetAlphaColor();

        yield return wait;

        _exploder.Explode(this);
        DestroyFigure?.Invoke(this);
    }
}
