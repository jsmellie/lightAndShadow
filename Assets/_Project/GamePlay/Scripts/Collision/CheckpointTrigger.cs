using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : BaseTrigger
{
    [SerializeField] private int CheckpointIndex = 0;
    [SerializeField] private Transform _spawnAnchor;

    void Start()
    {
        if (CheckpointManager.Instance.CurrentCheckpoint == CheckpointIndex)
        {
            PlayerSpawnHandler.Instance.Spawn(_spawnAnchor);
        }
    }

    public override void OnTriggerEnter(Collider collider)
    {
        CheckpointManager.Instance.SaveCheckpoint(CheckpointIndex);
        base.OnTriggerEnter(collider);
    }
}