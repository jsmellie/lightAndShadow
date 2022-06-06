using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private int _maxCheckpoint = 0;
    [SerializeField] private GameObject _collectable;

    void Start()
    {
        if (CheckpointManager.Instance.CurrentCheckpoint < _maxCheckpoint)
        {
            Instantiate(_collectable,transform);
        }
    }
}
