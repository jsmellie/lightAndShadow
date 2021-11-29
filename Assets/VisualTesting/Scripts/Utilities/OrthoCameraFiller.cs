using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrthoCameraFiller : MonoBehaviour
{
    [SerializeField] private Camera _mirroringCamera = null;
    [SerializeField] private bool _fillOnAwake = true;

    private Transform xform = null;

    private void Awake()
    {
        if (_fillOnAwake)
        {
            Camera cam = _mirroringCamera;

            if (cam == null)
            {
                cam = Camera.main;
            }

            FillToCameraSize(cam);
        }
    }

    public void FillToCameraSize(Camera camera)
    {
        if (camera != null)
        {
            if (camera.orthographic)
            {
                if (xform == null)
                {
                    xform = this.transform;
                }

                Vector3 scale = Vector3.one;
                scale.y = camera.orthographicSize * 2;
                scale.x = scale.y * camera.aspect;
                xform.localScale = scale;
            }
            else
            {
                Debug.LogError("Camera to fill isn't orthographic.");
            }
        }
        else
        {
            Debug.LogError("No valid camera was provided.");
        }
    }

    [ContextMenu("Fill")]
    private void CONTEXT_Fill()
    {
        FillToCameraSize(_mirroringCamera);
    }
}
