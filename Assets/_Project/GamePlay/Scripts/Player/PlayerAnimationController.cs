using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public enum LookDirection
    {
        Right,
        Left
    }

    private LookDirection _currentLookDirection;
    private float _currentLookRotation;
    private float _targetLookRotation;

    private float _lookRotationVelocity;
    [SerializeField] private float _lookRotationAcceleration;

    [SerializeField] private Transform _lookRotationTransform;
    
    [SerializeField] private PlayerMovementController _playerMovementController;

    private void Update()
    {
        AxisInput axis = InputController.Instance.GetAxis(InputController.eAxis.Horizontal);

        if (axis.IsPositive)
        {
            SetLookDirection(LookDirection.Right);
        }
        else if (axis.IsNegative)
        {
            SetLookDirection(LookDirection.Left);
        }
    }

    private void FixedUpdate()
    {
        float acceleration = _lookRotationAcceleration * Time.fixedDeltaTime;

        if (_currentLookDirection == LookDirection.Right)
        {
            _lookRotationVelocity += acceleration;
        }
        else
        {
            _lookRotationVelocity -= acceleration;
        }

        _currentLookRotation += _lookRotationVelocity * Time.fixedDeltaTime;

        switch (_currentLookDirection)
        {
            case LookDirection.Right:
                if (_currentLookRotation < _targetLookRotation)
                {
                    _currentLookRotation += _lookRotationVelocity;

                    if (_currentLookRotation > _targetLookRotation)
                    {
                        _currentLookRotation = _targetLookRotation;
                        _lookRotationVelocity = 0;
                    }
                }
                else
                {
                    _currentLookRotation = _targetLookRotation;
                    _lookRotationVelocity = 0;
                }
                break;
            case LookDirection.Left:
                if (_currentLookRotation > _targetLookRotation)
                {
                    _currentLookRotation += _lookRotationVelocity;

                    if (_currentLookRotation < _targetLookRotation)
                    {
                        _currentLookRotation = _targetLookRotation;
                        _lookRotationVelocity = 0;
                    }
                }
                else
                {
                    _currentLookRotation = _targetLookRotation;
                    _lookRotationVelocity = 0;
                }
                break;
        }

        _lookRotationTransform.localRotation = Quaternion.Euler(_lookRotationTransform.eulerAngles.x, _currentLookRotation, _lookRotationTransform.eulerAngles.x);
    }

    public void SetLookDirection(LookDirection lookDirection)
    {
        if (lookDirection != _currentLookDirection)
        {
            _lookRotationVelocity = -_lookRotationVelocity;
        }
        else
        {
            return;
        }

        _currentLookDirection = lookDirection;

        switch(_currentLookDirection)
        {
            case LookDirection.Right:
                _targetLookRotation = 0;
                break;
            case LookDirection.Left:
                _targetLookRotation = -180;
                break;
        }
    }
}
