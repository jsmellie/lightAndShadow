using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeToCameraAspect : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [ContextMenu("Testing")]
    private void Awake()
    {
        if (_camera != null)
        {
            Vector2 size = new Vector2(_camera.orthographicSize * 2 * _camera.aspect, _camera.orthographicSize * 2);
            this.transform.localScale = size;
        }
    }
}
