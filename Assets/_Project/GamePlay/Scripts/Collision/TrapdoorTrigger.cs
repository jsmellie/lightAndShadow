using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapdoorTrigger : BaseTrigger
{
    [SerializeField] private Animation _trapdoor;

    public override void OnTriggerEnter(Collider collider)
    {
        _trapdoor.Play();

        base.OnTriggerEnter(collider);
    }
}