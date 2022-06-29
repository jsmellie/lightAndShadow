using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform _topBounds;
    [SerializeField] private Transform _bottomBounds;

    [SerializeField] private float _stopDelay = 1f;
    [SerializeField] private float _tweenSpeed = 1f;

    [SerializeField] private bool _goingUp = true;

    private float _stoppedTime = 0f;

    private Vector3 _distance;

    void Start()
    {
        _distance = _topBounds.position - _bottomBounds.position;
    }

    // Update is called once per frame
    void Update()
    {
        var currentPos = transform.position;

        if(_stoppedTime > 0)
        {
            _stoppedTime -= Time.deltaTime; 
            return;
        }

        if(_goingUp)
        {
            currentPos += _distance * _tweenSpeed * Time.deltaTime;
            
            if(currentPos.y >= _topBounds.position.y)
            {
                currentPos = _topBounds.position;
                _stoppedTime = _stopDelay;
                _goingUp = false;
            }
        }
        else
        {
            currentPos -= _distance * _tweenSpeed * Time.deltaTime;
            
            if(currentPos.y <= _bottomBounds.position.y)
            {
                currentPos = _bottomBounds.position;
                _stoppedTime = _stopDelay;
                _goingUp = true;
            }
        }
        transform.position = currentPos;
    }
}
