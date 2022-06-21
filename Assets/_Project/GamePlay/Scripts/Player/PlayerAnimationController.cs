using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private const string IS_GROUNDED_PARAMETER = "IsGrounded";
    private const string VELOCITY_PARAMETER = "Velocity";

    public enum LookDirection
    {
        Right,
        Left
    }

    public enum AnimationState
    {
        Spawning,
        Movement,
        Celebration,
        Calm
    }

    private LookDirection _currentLookDirection;
    private float _currentLookRotation;
    private float _targetLookRotation;

    private float _lookRotationVelocity;
    [SerializeField] private float _lookRotationAcceleration;

    [SerializeField] private Transform _lookRotationTransform;
    
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private Animator _animator;

    private bool _acceptInput = true;

    private AnimationState _currentAnimationState = AnimationState.Movement;

    public void SetInteractable(bool isInteractable)
    {
        _acceptInput = isInteractable;
    }
    
    public void SetAnimationState(AnimationState animationState)
    {
        _currentAnimationState = animationState;
    }

    private void Update()
    {
        switch (_currentAnimationState)
        {
            case AnimationState.Spawning:
                UpdateSpawning();
                break;

            case AnimationState.Calm:
                UpdateCalm();
                break;

            case AnimationState.Celebration:
                UpdateCelebration();
                break;

            case AnimationState.Movement:
                UpdateMovement();
                break;
        }
    }

    private void UpdateSpawning()
    {

    }

    private void UpdateCalm()
    {
        if (_acceptInput)
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

            _animator.SetFloat(VELOCITY_PARAMETER, Mathf.Abs(_playerMovementController.Velocity.x) / _playerMovementController.MaxSpeed);
        }
        else
        {
            _animator.SetFloat(VELOCITY_PARAMETER, 0);
        }

        _animator.SetBool(IS_GROUNDED_PARAMETER, _playerMovementController.IsGrounded);
    }

    private void UpdateCelebration()
    {

    }

    private void UpdateMovement()
    {
        if (_acceptInput)
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

            _animator.SetFloat(VELOCITY_PARAMETER, Mathf.Abs(_playerMovementController.Velocity.x) / _playerMovementController.MaxSpeed);
        }
        else
        {
            _animator.SetFloat(VELOCITY_PARAMETER, 0);
        }

        _animator.SetBool(IS_GROUNDED_PARAMETER, _playerMovementController.IsGrounded);
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
