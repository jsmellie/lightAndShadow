using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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

    private float _targetZoom = 10;
    private float _currentZoom;

    private Vector3 _velocity = Vector3.zero;
    private float _zoomVelocity = 0;

    private CinemachinePath _cameraMinimumHeight;

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

    private float CalculateCameraMinHeight()
    {
        if (_cameraMinimumHeight == null)
        {
            return 0;
        }

        float cameraXPosition = _currentPosition.x;

        if (cameraXPosition < _cameraMinimumHeight.m_Waypoints[0].position.x)
        {
            return _cameraMinimumHeight.m_Waypoints[0].position.y;
        }

        if (cameraXPosition > _cameraMinimumHeight.m_Waypoints[_cameraMinimumHeight.m_Waypoints.Length - 1].position.x)
        {
            return _cameraMinimumHeight.m_Waypoints[_cameraMinimumHeight.m_Waypoints.Length - 1].position.y;
        }

        int currentWaypoint = -1;

        for (int i = 0; i < _cameraMinimumHeight.m_Waypoints.Length - 1; i++)
        {
            if (cameraXPosition > _cameraMinimumHeight.m_Waypoints[i + 1].position.x)
            {
                continue;
            }

            currentWaypoint = i;

            float currentX = _cameraMinimumHeight.m_Waypoints[i].position.x;
            float nextX = _cameraMinimumHeight.m_Waypoints[i + 1].position.x;

            float percentage = (cameraXPosition - currentX) / (nextX - currentX);

            return _cameraMinimumHeight.EvaluatePosition(currentWaypoint + percentage).y;

        }

        return _cameraMinimumHeight.m_Waypoints[0].position.y;
    }

    public void SetPlayerTransform(Transform player)
    {
        _playerTransform = player;
    }

    public void SetCameraMinimumHeight(CinemachinePath path)
    {
        _cameraMinimumHeight = path;
    }

    private void Update()
    {
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
    }

    private void FixedUpdate()
    {
        UpdateCurrentPosition();
        UpdateCurrentZoom();

        UpdateCameraMinimumHeight();

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

        _currentPosition = Vector3.SmoothDamp(_currentPosition, offsetTargetPosition, ref _velocity, cameraLerpSpeed);
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

        _currentZoom = Mathf.SmoothDamp(_currentZoom, targetZoom, ref _zoomVelocity, zoomLerpSpeed);
    }

    private void UpdateCameraMinimumHeight()
    {
        float cameraMinHeight = CalculateCameraMinHeight();

        float newCameraHeight = _currentPosition.y;

        if (_currentPosition.y - (_currentZoom) < cameraMinHeight)
        {
            newCameraHeight = cameraMinHeight + (_currentZoom);
        }

        _currentPosition.y = newCameraHeight;
    }
}
