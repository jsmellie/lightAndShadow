using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTrigger : AnimationTrigger
{
    [SerializeField] private Transform _pickup;

    public override void OnTriggerEnter(Collider collider)
    {
        Transform hand = PlayerController.Instance.GetHand();
        _pickup.SetParent(hand);
        _pickup.localPosition = Vector3.zero;

        base.OnTriggerEnter(collider);
    }
}