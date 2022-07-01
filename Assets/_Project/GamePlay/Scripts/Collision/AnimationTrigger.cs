using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimationController;

public class AnimationTrigger : BaseTrigger
{
    [SerializeField] private string _animationName;
    [SerializeField] private Transform _animationAnchor;
    [SerializeField] private PlayerAnimationController.AnimationState _animationState = PlayerAnimationController.AnimationState.Movement;
    [SerializeField] private LookDirection _lookDirection;

    [SerializeField] private bool _forcePosition;

    public override void OnTriggerEnter(Collider collider)
    {
        PlayerAnimationController playerAnimationController = collider.GetComponent<PlayerAnimationController>();

        if (playerAnimationController != null)
        {
            PlayerAnimationInfo animationInfo = new PlayerAnimationInfo();
            animationInfo.AnimationName = _animationName;

            if (_animationAnchor != null)
            {
                animationInfo.AnimationOrigin = _animationAnchor.transform.position;
            }
            else
            {
                animationInfo.AnimationOrigin = transform.position;
            }
            
            animationInfo.AnimationState = _animationState;
            animationInfo.Direction = _lookDirection;

            playerAnimationController.PlayAnimation(animationInfo, _forcePosition);

        }

        base.OnTriggerEnter(collider);
    }

    public override void OnTriggerExit(Collider collider)
    {
        gameObject.SetActive(false);
        PlayerHealthController.Instance.SetHealthDrainPaused(false);   
        base.OnTriggerExit(collider);
    }
}
