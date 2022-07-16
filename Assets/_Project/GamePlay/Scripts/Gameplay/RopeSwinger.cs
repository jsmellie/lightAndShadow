using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSwinger : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private float _speed = 1f;
    private bool _isTriggered = false;
    private float _timer = 0f;

    void Update()
    {
        if(_isTriggered)
        {
            _timer += Time.deltaTime * _speed;
            transform.localRotation = Quaternion.Euler(0,0,GetRotation());
            if(_timer >= 1)
            {
                _timer = 0; 
                _isTriggered = false;
            }
        }
    }

    private float GetRotation()
    {
        float val = _curve.Evaluate(_timer);

        return val;
    }

    public void Begin()
    {
        _isTriggered = true;
    }
}
