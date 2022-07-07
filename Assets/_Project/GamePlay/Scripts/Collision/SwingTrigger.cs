using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimationController;

public class SwingTrigger : AnimationTrigger
{
    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        PlayerController.Instance.DetectTriggers(true);
    }
}
