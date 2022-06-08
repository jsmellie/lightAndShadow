using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnHandler : SingletonBehaviour<PlayerSpawnHandler>
{

    [SerializeField] private Transform _spawnAnchor; //TODO replace this with a real spawn thing
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _bugsPrefab;

    protected override void Initialize()
    {
        //TODO get initial spawn point, probably won't be loaded yet actually
    }

    public void Spawn()
    {
        var player = Instantiate(_playerPrefab);
        player.transform.parent = this.transform;
        player.transform.position = _spawnAnchor.position;

        var bugs = Instantiate(_bugsPrefab);
        bugs.transform.position = _spawnAnchor.position;
        bugs.GetComponent<BugsController>().SetPlayerTransform(player.transform);

        //TODO set player idle animation here
        FullScreenWipe.FadeOut(1, OnAnimationCompleted);
    }
    
    private void OnAnimationCompleted()
    {
        //TODO do player animation stuff here
    }
}
