using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    private Renderer _renderer;

    private void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.color = Color.white;
    }

    public void SetColor()
    {
        Color randomColor = Random.ColorHSV();

        _renderer.material.color = randomColor;
    }
}
