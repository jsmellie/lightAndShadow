using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneLift : MonoBehaviour
{
    [SerializeField] private Transform _destination;

    [SerializeField] private float _delay = 0f;
    [SerializeField] private float _tweenSpeed = 0.25f;

    private bool _isRunning = false;

    private Vector3 _distance;
    private Vector3 _startPosition;

    void Start()
    {
        _distance = _destination.position - transform.position;
        _startPosition = transform.position;
    }

    public void Elevate()
    {
        _isRunning = true;
    }

    public void ResetPosition()
    {
        transform.position = _startPosition;
    }

    // Update is called once per frame
    void Update()
    {

        if(_isRunning)
        {
            if(_delay > 0)
            {
                _delay -= Time.deltaTime;
                return;
            }
            var currentPos = transform.position;
            currentPos += _distance * _tweenSpeed * Time.deltaTime;
            
            if(currentPos.y >= _destination.position.y)
            {
                currentPos = _destination.position;
                _isRunning = false;
            }
            transform.position = currentPos;
        }
    }
}
