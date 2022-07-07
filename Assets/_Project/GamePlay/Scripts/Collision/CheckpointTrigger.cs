using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : BaseTrigger
{
    [SerializeField] private int CheckpointIndex = 0;
    [SerializeField] private Transform _spawnAnchor;

    public Transform SpawnAnchor
    {
        get { return _spawnAnchor; }
    }

    void Awake()
    {
        CheckpointManager.Instance.RegisterCheckpoint(this, CheckpointIndex);
    }

    void OnDestroy()
    {
        if(!CheckpointManager.IsInstanceNull)
        {
            CheckpointManager.Instance.UnregisterCheckpoint(CheckpointIndex);
        }
    }

    public override void OnTriggerEnter(Collider collider)
    {
        CheckpointManager.Instance.SaveCheckpoint(CheckpointIndex);
        base.OnTriggerEnter(collider);
    }
}