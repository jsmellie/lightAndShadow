using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : BaseTrigger
{
    [SerializeField] private int CheckpointIndex = 0;

    public override void OnTriggerEnter(Collider collider)
    {
        CheckpointManager.Instance.SaveCheckpoint(CheckpointIndex);
        base.OnTriggerEnter(collider);
    }
}