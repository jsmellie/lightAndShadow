using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerInputManager
{
    private static readonly KeyCode[] JumpKeyCodes = new KeyCode[] { KeyCode.UpArrow, KeyCode.W, KeyCode.Space};
    private static readonly KeyCode[] LeftKeyCodes = new KeyCode[] { KeyCode.LeftArrow, KeyCode.A};
    private static readonly KeyCode[] RightKeyCodes = new KeyCode[] { KeyCode.RightArrow, KeyCode.D};

    public delegate void PlayerInputDelegate();
    public delegate bool JumpValidationDelegate();
    
    public event JumpValidationDelegate OnJumpValidation;
    public event PlayerInputDelegate OnJump;
    public event PlayerInputDelegate OnMoveLeftStart;
    public event PlayerInputDelegate OnMoveLeftEnd;
    public event PlayerInputDelegate OnMoveRightStart;
    public event PlayerInputDelegate OnMoveRightEnd;

    private bool _wasMovingLeft;
    private bool _wasMovingRight;

    public bool IsEnabled
    {
        get; set;
    }

    public void UpdateInput()
    {
        if (!IsEnabled)
        {
            return;
        }
        bool moveLeft = false;
        bool moveRight = false;
        bool canJump = false;

        for(int i = 0; i < LeftKeyCodes.Length; ++i)
        {
            moveLeft |= Input.GetKey(LeftKeyCodes[i]);
        }

        for(int i = 0; i < RightKeyCodes.Length; ++i)
        {
            moveRight |= Input.GetKey(RightKeyCodes[i]);
        }

        bool checkJump = false;

        if (OnJumpValidation != null)
        {
            checkJump = OnJumpValidation();
        }

        if (checkJump)
        {
            for(int i = 0; i < JumpKeyCodes.Length; ++i)
            {
                canJump |= Input.GetKeyDown(JumpKeyCodes[i]);
            }
        }

        if (moveLeft && !moveRight)
        {
            if (!_wasMovingLeft)
            {
                _wasMovingLeft = true;
                if (OnMoveLeftStart != null)
                {
                    OnMoveLeftStart();
                }
            }
        }
        else if (moveRight && !moveLeft)
        {
            if (!_wasMovingRight)
            {
                _wasMovingRight = true;
                if (OnMoveRightStart != null)
                {
                    OnMoveRightStart();
                }
            }
        }
        else if (moveRight && moveLeft)
        {
            StopMovingLeft();
            StopMovingRight();
        }

        if (!moveLeft)
        {
            StopMovingLeft();
        }

        if (!moveRight)
        {
            StopMovingRight();
        }

        if (canJump)
        {
            if (OnJump != null)
            {
                OnJump();
            }
        }
    }

    private void StopMovingLeft()
    {
        if (_wasMovingLeft)
        {
            _wasMovingLeft = false;
            if (OnMoveLeftEnd != null)
            {
                OnMoveLeftEnd();
            }
        }
    }

    private void StopMovingRight()
    {
        if (_wasMovingRight)
        {
            _wasMovingRight = false;
            if (OnMoveRightEnd != null)
            {
                OnMoveRightEnd();
            }
        }
    }
}
