using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeToCameraAspect : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _scaleMultiplier = 1;

    [ContextMenu("Testing")]
    private void Awake()
    {
        if (_camera != null)
        {
            Vector2 size = new Vector2(_camera.orthographicSize * 2 * _scaleMultiplier * _camera.aspect, _camera.orthographicSize * 2 * _scaleMultiplier);
            this.transform.localScale = size;
        }
    }
}
