using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornBender : MonoBehaviour
{
    [SerializeField] private CornTrigger _leftTrigger;
    [SerializeField] private CornTrigger _rightTrigger;
    [SerializeField] private Transform _pivot;
    [SerializeField] private List<Sprite> _corns;
    [SerializeField] private SpriteRenderer _renderer;

    private Vector3 _leftRotation = new Vector3(0,0,10);
    private Vector3 _rightRotation = new Vector3(0,0,-10);
    private Vector3 _forwardRotation = new Vector3(20,0,0);
    private Vector3 _targetRotation = Vector3.zero;

    void Start()
    {
        _renderer.sprite = _corns[Random.Range(0,_corns.Count)];
    }

    void Update()
    {
        if(_leftTrigger.IsColliding && _rightTrigger.IsColliding)
        {
            _targetRotation = _forwardRotation;
        }
        else if (_leftTrigger.IsColliding)
        {
            _targetRotation = _rightRotation;
        }
        else if (_rightTrigger.IsColliding)
        {
            _targetRotation = _leftRotation;
        }
        else
        {
            _targetRotation = Vector3.zero;
        }
        
    }

    void FixedUpdate()
    {
        _pivot.localRotation = Quaternion.Slerp(_pivot.localRotation, Quaternion.Euler(_targetRotation.x, _targetRotation.y, _targetRotation.z), 0.3f);
    }
}
