using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    private class MovementManager
    {
        private enum MovementState
        {
            Inactive,
            Normal,
            Fast,
            Slowing
        }
        MovementState State = MovementState.Inactive;
        float MaxSpeed = 5f;
        Rigidbody2D Body = null;
        float CurrentSpeed = 5f;
        float TimeToSlow = 0.2f;
        float SlowTimeStamp = -1f;

        public MovementManager(Rigidbody2D body, float speed)
        {
            Body = body;
            MaxSpeed = speed;
        }

        public void UpdateMaxSpeed(float speed)
        {
            MaxSpeed = speed;
        }

        public void Start()
        {
            if (State == MovementState.Inactive || State == MovementState.Slowing)
            {
                State =  MovementState.Normal;
                CurrentSpeed = MaxSpeed;
            }
        }

        public void Stop(bool immediate = false)
        {
            if (State != MovementState.Inactive)
            {
                if (!immediate)
                {
                    State = MovementState.Slowing;
                    SlowTimeStamp = Time.fixedTime;
                }
                else
                {
                    State = MovementState.Inactive;
                    CurrentSpeed = 0f;
                }
            }
        }

        public void FixedUpdate()
        {
            if (State != MovementState.Inactive)
            {
                Vector2 velocity = Body.velocity;
                velocity.x = CurrentSpeed;
                Body.velocity = velocity;
            }

            if (State == MovementState.Slowing)
            {
                float slowingTime = Time.fixedTime - SlowTimeStamp;
                float slowingPercentage = 1 - Mathf.Clamp01(slowingTime / TimeToSlow);
                CurrentSpeed = MaxSpeed * slowingPercentage;

                if (Mathf.Approximately(slowingPercentage, 0f))
                {
                    State = MovementState.Inactive;
                }
            }
        }
    }

    private static readonly int OnGroundLayerMask = 1 << 10; //Terrain
    private const float OnGroundCastDistance = 0.1f;

    [SerializeField] private Rigidbody2D _playerBody;
    [SerializeField] private Collider2D _playerCollider;
    [SerializeField] private Transform[] _groundCheckCastPoints;
    [Space]
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _doubleJumpSpeed = 13f;
    [SerializeField] private float _movementSpeed = 2f;

    int _frameNumberOnJumpCheck = -1;
    bool _isOnGround = true;
    RaycastHit2D _reusableRaycastHitObject = default(RaycastHit2D);
    bool _hasDoubleJumped = false;
    PlayerInputManager _inputManager;

    MovementManager _leftManager = null;
    MovementManager _rightManager = null;

    public bool CanJump
    {
        get
        {
            if (!HasCheckedJumpThisFrame)
            {
                UpdateGroundState();
            }

            return _isOnGround || !_hasDoubleJumped;
        }
    }

    private bool HasCheckedJumpThisFrame
    {
        get { return _frameNumberOnJumpCheck == Time.frameCount; }
    }

    private void Awake()
    {
        Assert.IsNotNull(_playerBody, "Player Rigidbody");
        Assert.IsNotNull(_playerCollider, "Player Collider");
        Assert.AreNotEqual(_groundCheckCastPoints.Length, 0, "Ground Check Cast Points");

        _inputManager = new PlayerInputManager();
        _inputManager.OnJumpValidation += JumpValidation;
        _inputManager.OnJump += OnJump;
        _inputManager.OnMoveLeftStart += OnStartMoveLeft;
        _inputManager.OnMoveRightStart += OnStartMoveRight;
        _inputManager.OnMoveLeftEnd += OnEndMoveLeft;
        _inputManager.OnMoveRightEnd += OnEndMoveRight;

        _leftManager = new MovementManager(_playerBody, -_movementSpeed);
        _rightManager = new MovementManager(_playerBody, _movementSpeed);
    }

    private void OnStartMoveLeft()
    {
        _rightManager.Stop(true);
        _leftManager.Start();
    }

    private void OnStartMoveRight()
    {
        _leftManager.Stop(true);
        _rightManager.Start();
    }

    private void OnEndMoveLeft()
    {
        _leftManager.Stop();
    }

    private void OnEndMoveRight()
    {
        _rightManager.Stop();
    }

    private void OnJump()
    {
        Vector2 velocity = _playerBody.velocity;
        float jumpSpeed = _jumpSpeed;
        if (!_isOnGround)
        {
            _hasDoubleJumped = true;
            jumpSpeed = _doubleJumpSpeed;
        }

        velocity.y = jumpSpeed;
        _playerBody.velocity = velocity;
    }

    private bool JumpValidation()
    {
        return CanJump;
    }

    private void Update()
    {
        _inputManager.UpdateInput();

        if (_hasDoubleJumped)
        {
            UpdateGroundState();
        }
    }

    private void FixedUpdate()
    {
        _leftManager.FixedUpdate();
        _rightManager.FixedUpdate();
    }

    private void UpdateGroundState()
    {
        _isOnGround = false;

        StringBuilder builder = new StringBuilder();

        builder.Append("Update Ground State - ");

        for(int i = 0; i < _groundCheckCastPoints.Length; ++i)
        {
            _reusableRaycastHitObject = Physics2D.Raycast(_groundCheckCastPoints[i].position, -Vector2.up, OnGroundCastDistance, OnGroundLayerMask);
            bool castHit = _reusableRaycastHitObject.collider != null;
            if (i > 0)
            {
                builder.Append(" | ");
            }

            builder.Append($"Point {i}: {castHit}");
            _isOnGround |= _reusableRaycastHitObject.collider != null;
        }

        builder.Append($" | On Ground: {_isOnGround}");

        if (_isOnGround)
        {
            _hasDoubleJumped = false;
        }

        _frameNumberOnJumpCheck = Time.frameCount;
    }

    public void DebugSetValues(float movementSpeed, float jumpSpeed, float doubleJumpSpeed)
    {
        _movementSpeed = movementSpeed;
        _jumpSpeed = jumpSpeed;
        _doubleJumpSpeed = doubleJumpSpeed;
    }

    public void DebugGetValues(out float movementSpeed, out float jumpSpeed, out float doubleJumpSpeed)
    {
        movementSpeed = _movementSpeed;
        jumpSpeed = _jumpSpeed;
        doubleJumpSpeed = _doubleJumpSpeed;
    }
}
