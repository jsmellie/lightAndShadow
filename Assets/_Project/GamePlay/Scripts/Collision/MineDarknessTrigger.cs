using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDarknessTrigger : BaseTrigger
{
    [SerializeField] private bool _enable = true;

    public override void OnTriggerEnter(Collider collider)
    {
        CameraEffectController.Instance.ToggleDarkness(_enable);
        base.OnTriggerEnter(collider);
    }
}
