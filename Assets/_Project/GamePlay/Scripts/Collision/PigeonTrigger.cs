using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimationController;

public class PigeonTrigger : BaseTrigger
{
    [SerializeField] private string _animationName = "anim_joy_1-4_pigeons_fly";
    [SerializeField] private Animator _animator;

    public override void OnTriggerEnter(Collider collider)
    {
        
        PlayerHealthController.Instance.FullHeal();

        _animator.Play(_animationName);
      

        base.OnTriggerEnter(collider);
        gameObject.SetActive(false);
    }
}
