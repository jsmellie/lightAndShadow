using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerSpawnHandler : SingletonBehaviour<PlayerSpawnHandler>
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _bugsPrefab;

    protected override void Initialize()
    {
        //TODO get initial spawn point, probably won't be loaded yet actually
    }

    public void Spawn(Transform spawnAnchor)
    {
        var player = Instantiate(_playerPrefab);
        player.transform.parent = this.transform;
        player.transform.position = spawnAnchor.position;

        var bugs = Instantiate(_bugsPrefab);
        bugs.transform.position = spawnAnchor.position;
        bugs.GetComponent<BugsController>().SetPlayerTransform(player.transform);

        CameraBehaviourController cameraBehaviourController = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).GetComponent<CameraBehaviourController>();
        cameraBehaviourController.SetPlayerTransform(player.transform);
        cameraBehaviourController.SetCameraBehaviourState(CameraBehaviourController.CameraBehaviourState.FollowPlayer);
        cameraBehaviourController.SetCameraMinimumHeight(GameObject.FindObjectOfType<CinemachinePath>());

        //TODO set player idle animation here
        if(!FullScreenWipe.IsWiping)
            FullScreenWipe.FadeOut(1, OnAnimationCompleted);
    }
    
    private void OnAnimationCompleted()
    {
        //TODO do player animation stuff here
    }
}
