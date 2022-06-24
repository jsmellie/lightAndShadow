using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : SingletonBehaviour<CollectableManager>
{
    List<CollectableSpawner> _spawners = new List<CollectableSpawner>();

    protected override void Initialize()
    {

    }

    public void Register(CollectableSpawner spawner)
    {
        if(!_spawners.Contains(spawner))
        {
            _spawners.Add(spawner);
        }
    }

    public void Unregister(CollectableSpawner spawner)
    {
        _spawners.Remove(spawner);
    }

    public void Spawn()
    {
        foreach (var spawner in _spawners)
        {
            spawner.Spawn();
        }
    }
}
