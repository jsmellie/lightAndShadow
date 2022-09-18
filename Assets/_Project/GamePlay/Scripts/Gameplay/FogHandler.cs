using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogHandler : MonoBehaviour
{

    [SerializeField] private Transform _fog;
    [SerializeField] private Transform _highPoint;
    [SerializeField] private Transform _lowPoint;

    [SerializeField] private int _lowestHealthThreshold;

    private HealthToFogConverter _instance;
    private Vector3 _currentPosition;
    private Vector3 _targetPosition;
    private Vector3 _velocity;

    void Start()
    {
        _instance = HealthToFogConverter.Instance;
        _currentPosition = _fog.position;
        _targetPosition = _currentPosition;
    }

    void Update()
    {
        int health = _instance.Health;

        if(health <= _lowestHealthThreshold)
        {
            health = _lowestHealthThreshold;
        }

        float scaler = Mathf.InverseLerp(100,_lowestHealthThreshold, health);

        _targetPosition = Vector3.Lerp(_lowPoint.position,_highPoint.position,scaler);        

    }

    void FixedUpdate()
    {
        _currentPosition = Vector3.SmoothDamp(_currentPosition, _targetPosition, ref _velocity, 0.4f);
        _fog.position = _currentPosition;
    }


}
