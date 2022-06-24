using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] private int _maxCheckpoint = 0;
    [SerializeField] private GameObject _collectable;

    private GameObject _spawnedCollectable = null;

    void Start()
    {
        CollectableManager.Instance.Register(this);
        Spawn();
    }

    void OnDestroy()
    {
        CollectableManager.Instance.Unregister(this);
    }

    public void Spawn()
    {
        if (CheckpointManager.Instance.CurrentCheckpoint < _maxCheckpoint)
        {
            if(_spawnedCollectable)
            {
                Destroy(_spawnedCollectable);
            }
            _spawnedCollectable = Instantiate(_collectable, transform);
            _spawnedCollectable.transform.localPosition = Vector3.zero;
        }
    }
}
