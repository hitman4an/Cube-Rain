using System;
using UnityEngine;

public interface IObjectable<T> where T: MonoBehaviour
{
    public event Action<T> DestroyFigure;
}
