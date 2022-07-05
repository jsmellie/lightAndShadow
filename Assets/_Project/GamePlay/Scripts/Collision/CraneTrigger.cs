using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimationController;

public class CraneTrigger : AnimationTrigger
{
    [SerializeField] private CraneLift _lift;

    public override void OnTriggerEnter(Collider collider)
    {
        _lift.Elevate();

        base.OnTriggerEnter(collider);
        PlayerController.Instance.DetectTriggers(true);
    }
}
