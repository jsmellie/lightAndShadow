using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourController : MonoBehaviour
{
    [Serializable]
    public class CameraBehaviourInfo
    {
        public Transform TargetTransform = null;
        public float OverrideLerpSpeed = -1;
        public float OverrideZoom = -1;
        public float OverrideZoomSpeed = -1;
        public Vector3 PositionOffset = Vector3.zero;
    }

    public enum CameraBehaviourState
    {
        None,
        FollowTarget,
        FollowPlayer,
        Static
    }

    private CameraBehaviourState _currentState;
    private CameraBehaviourInfo _currentInfo;

    [SerializeField] private Transform _followTransform;
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private float _cameraLerpSpeed = 5;
    [SerializeField] private float _zoomLerpSpeed = 5;

    [SerializeField] private Camera _camera;

    private Vector3 _targetPosition;
    private Vector3 _currentPosition;

    private float _targetZoom = 15;
    private float _currentZoom;

    public CameraBehaviourState GetCameraBehaviourState()
    {
        return _currentState;
    }

    public CameraBehaviourInfo GetCameraBehaviourInfo()
    {
        return _currentInfo;
    }

    public Transform GetCameraTarget()
    {
        return _followTransform;
    }

    public void SetCameraBehaviourState(CameraBehaviourState state)
    {
        _currentState = state;
    }

    public void SetCameraBehaviourInfo(CameraBehaviourInfo info)
    {
        _currentInfo = info;
    }

    public void SnapToTarget()
    {
        _currentPosition = _targetPosition;
        _currentZoom = _targetZoom;
    }

    private void Awake()
    {
        SnapToTarget();
    }

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            _playerTransform = GameObject.FindObjectOfType<PlayerMovementController>().transform;
            _currentState = CameraBehaviourState.FollowPlayer;
        }

        switch(_currentState)
        {
            case CameraBehaviourState.FollowTarget:
                UpdateFollowTarget();
                break;
            case CameraBehaviourState.FollowPlayer:
                UpdateFollowPlayer();
                break;
            case CameraBehaviourState.Static:
                UpdateStatic();
                break;
            case CameraBehaviourState.None:
                UpdateNone();
                break;
        }

        UpdateCurrentPosition();
        UpdateCurrentZoom();
    }

    private void FixedUpdate()
    {
        transform.position = _currentPosition;
        _camera.orthographicSize = _currentZoom;
    }

    private void UpdateFollowTarget()
    {
        if (_followTransform != null)
        {
            _targetPosition = _followTransform.position;
        }
    }

    private void UpdateFollowPlayer()
    {
        if (_playerTransform != null)
        {
            _targetPosition = _playerTransform.position;
        }
    }

    private void UpdateStatic()
    {

    }

    private void UpdateNone()
    {
        
    }

    public void SetCameraTarget(Transform followTransform)
    {
        _followTransform = followTransform;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {

    }

    private void UpdateCurrentPosition()
    {
        Vector3 offsetTargetPosition = _targetPosition;
        float cameraLerpSpeed = _cameraLerpSpeed;

        if (_currentInfo != null)
        {
            offsetTargetPosition += _currentInfo.PositionOffset;
            if (_currentInfo.OverrideLerpSpeed >= 0)
            {
                cameraLerpSpeed = _currentInfo.OverrideLerpSpeed;
            }
        }

        _currentPosition = Vector3.Lerp(_currentPosition, offsetTargetPosition, Time.deltaTime * cameraLerpSpeed);
        _currentPosition.z = -100;
    }

    private void UpdateCurrentZoom()
    {
        float targetZoom = _targetZoom;
        float zoomLerpSpeed = _zoomLerpSpeed;

        if (_currentInfo != null)
        {
            if (_currentInfo.OverrideZoom >= 0)
            {
                targetZoom = _currentInfo.OverrideZoom;
            }

            if (_currentInfo.OverrideZoomSpeed >= 0)
            {
                zoomLerpSpeed = _currentInfo.OverrideZoomSpeed;
            }
        }

        _currentZoom = Mathf.Lerp(_currentZoom, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}
