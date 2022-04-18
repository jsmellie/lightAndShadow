using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : SingletonBehaviour<UIController>
{
    private Canvas _canvas;

    protected override void Initialize()
    {
        _canvas = GetComponent<Canvas>();
    }
}
