using UnityEngine;
using UnityEngine.Events;


public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private bool _isCollidedWithPlain;
    private int _minDestroyDelay = 2;
    private int _maxDestroyDelay = 4;

    public event UnityAction<GameObject> DestroyCube;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _isCollidedWithPlain = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollidedWithPlain == false && collision.gameObject.tag == "Plain")
        {
            _isCollidedWithPlain = true;
            SetColor();

            Invoke(nameof(InvokeDestroyEvent), Random.Range(_minDestroyDelay, _maxDestroyDelay + 1));
        }
    }
    private void SetColor()
    {
        Color randomColor = Random.ColorHSV();

        _renderer.material.color = randomColor;
    }

    private void InvokeDestroyEvent()
    {
        DestroyCube?.Invoke(this.gameObject);
    }
}
