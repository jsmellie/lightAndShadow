using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonBehaviour<PlayerController>
{
    [SerializeField] private PlayerMovementController _movementController;
    [SerializeField] private PlayerAnimationController _animationController;
    [SerializeField] private PlayerTriggerController _triggerController;
    [SerializeField] private PlayerStickController _stickController;

    private bool _isInteractable = false;

    protected override void Initialize()
    {

    }

    public void EnterMovementState()
    {
        _animationController.EnterMovementState();
    }

    public void SetInteractable(bool isInteractable)
    {
        _isInteractable = isInteractable;

        _movementController.SetInteractable(_isInteractable);
        _animationController.SetInteractable(_isInteractable);
        PlayerHealthController.Instance.SetHealthDrainPaused(!_isInteractable);
        
    }

    public void SetAnimationState(PlayerAnimationController.AnimationState animationState)
    {
        _animationController.SetAnimationState(animationState);
    }

    public void DetectTriggers(bool detectTriggers)
    {
        _triggerController.DetectTriggers(detectTriggers);
    }

    public void SetInitialState()
    {
        SetInteractable(false);
        _animationController.SetAnimationState(PlayerAnimationController.AnimationState.Movement);
        _triggerController.DetectTriggers(false);
        _stickController.AddSticks();
    }
}
