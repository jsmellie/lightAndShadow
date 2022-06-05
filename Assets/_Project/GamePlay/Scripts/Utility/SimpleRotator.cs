using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationAmount;
    [SerializeField] private Vector3 _rotationPeriod;
    [SerializeField] private bool _useUnscaledTime;

    private Vector3 _startRotation;
    private float _time;

    private void Awake()
    {
        _startRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if (_useUnscaledTime)
        {
            _time += Time.unscaledDeltaTime;
        }
        else
        {
            _time += Time.deltaTime;
        }

        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Vector3 rotation = _startRotation;

        rotation.x += _rotationAmount.x * Mathf.Sin(_time * _rotationPeriod.x);
        rotation.y += _rotationAmount.y * Mathf.Sin(_time * _rotationPeriod.y);
        rotation.z += _rotationAmount.z * Mathf.Sin(_time * _rotationPeriod.z);

        transform.rotation = Quaternion.Euler(rotation);
    }
}
