using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTrigger : AnimationTrigger
{
    [SerializeField] private Transform _pickup;

    public override void OnTriggerEnter(Collider collider)
    {
        _pickup.SetParent(PlayerController.Instance.transform);

        base.OnTriggerEnter(collider);
    }
}