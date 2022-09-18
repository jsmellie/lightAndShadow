using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArounder : MonoBehaviour
{
    [SerializeField] private float _rotationAmount;
    [SerializeField] private bool _useUnscaledTime;
    [SerializeField] private Transform _point;

    private float _time;

    private void Update()
    {
        if (_useUnscaledTime)
        {
            _time = Time.unscaledDeltaTime;
        }
        else
        {
            _time = Time.deltaTime;
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {

        var rot = transform.rotation;
        transform.RotateAround(_point.position,Vector3.forward,_rotationAmount*_time);
        transform.rotation = rot;
    }
}
