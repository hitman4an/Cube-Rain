using System.Collections;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] Color _startColor = Color.white;
    
    private Renderer _renderer;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = _startColor;
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine( _coroutine );
    }

    public void SetColor()
    {
        Color randomColor = Random.ColorHSV();

        _renderer.material.color = randomColor;
    }

    public void SetAlphaColor()
    {
        _coroutine = StartCoroutine(ChangeAlpha());
    }

    private IEnumerator ChangeAlpha()
    {
        float changeAmount = 0.1f;
        
        while (enabled)
        {
            Color color = _renderer.material.color;

            color.a -= changeAmount * Time.deltaTime;
            _renderer.material.color = color;

            yield return null;
        }
    }
}
