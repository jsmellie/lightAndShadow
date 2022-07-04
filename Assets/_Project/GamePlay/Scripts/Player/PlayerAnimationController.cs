using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public class PlayerAnimationInfo
    {
        public string AnimationName;
        public LookDirection Direction;
        public Vector3 AnimationOrigin;
        public AnimationState AnimationState;
        public bool InterruptOtherAnimations;
    }

    private const string IS_GROUNDED_PARAMETER = "IsGrounded";
    private const string VELOCITY_PARAMETER = "Velocity";

    private const string SPAWN_TRIGGER = "Spawn";
    private const string STAND_TRIGGER = "Stand";
    private const string DIE_TRIGGER = "Die";
    private const string RESPAWN_STAND_TRIGGER = "RespawnStand";
    private const string DEAD_TRIGGER = "Dead";

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
    private float _currentLookRotation = 1f;
    private float _targetLookRotation = 1f;

    private float _lookRotationVelocity;
    [SerializeField] private float _lookRotationAcceleration;

    [SerializeField] private Transform _lookRotationTransform;
    
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private Animator _animator;

    private PlayerAnimationInfo _currentAnimationInfo;
    private bool _waitingToPlayAnimtion = false;
    private bool _isPlayingAnimation = false;

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

    public void PlayAnimation(PlayerAnimationInfo animationInfo, bool forcePosition)
    {
        if (forcePosition)
        {
            transform.position = animationInfo.AnimationOrigin;
        }

        _currentAnimationInfo = animationInfo;

        PlayerController.Instance.SetInteractable(false);
        PlayerController.Instance.DetectTriggers(false);
       
        if (!_playerMovementController.IsGrounded)
        {
            _waitingToPlayAnimtion = true;
        }
        else
        {
            BeginAnimation();
        }        
    }

    private void BeginAnimation()
    {
        if (_isPlayingAnimation && !_currentAnimationInfo.InterruptOtherAnimations) return;

        _isPlayingAnimation = true;
        PlayerHealthController.Instance.FullHeal();
        PlayerHealthController.Instance.SetHealthDrainPaused(true);
        SetAnimationState(_currentAnimationInfo.AnimationState);
        SetLookDirection(_currentAnimationInfo.Direction);
        _animator.SetTrigger(_currentAnimationInfo.AnimationName);
    }

    public void EnterMovementState()
    {
        if (_isPlayingAnimation)
        {
            _isPlayingAnimation = false;
            SetAnimationState(AnimationState.Movement);
            PlayerController.Instance.SetInteractable(true);
            PlayerController.Instance.DetectTriggers(true);
        }
    }

    public void PlayStartLoop()
    {
        _animator.SetTrigger(SPAWN_TRIGGER);
    }

    public void PlayStartAnimation()
    {
        _animator.SetTrigger(STAND_TRIGGER);
    }

    public void PlayDeathAnimation()
    {
        _animator.SetTrigger(DIE_TRIGGER);
    }

    public void PlayRespawnAnimation()
    {
        _animator.SetTrigger(RESPAWN_STAND_TRIGGER);
    }

    public void PlayDeadLoop()
    {
        _animator.SetTrigger(DEAD_TRIGGER);
    }

    private void Update()
    {
        if(PauseController.IsPaused)
        {
            _animator.speed = 0;
            return;
        }
        else
        {
            _animator.speed = 1;
        }

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

        if (_waitingToPlayAnimtion && _playerMovementController.IsGrounded)
        {
            _waitingToPlayAnimtion = false;
            BeginAnimation();
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

        _lookRotationTransform.localScale = new Vector3(_currentLookRotation, _lookRotationTransform.localScale.y, _lookRotationTransform.localScale.z);
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
                _targetLookRotation = 1f;
                break;
            case LookDirection.Left:
                _targetLookRotation = -1f;
                break;
        }
    }
}
