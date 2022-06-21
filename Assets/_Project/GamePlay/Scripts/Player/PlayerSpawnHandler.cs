using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerSpawnHandler : SingletonBehaviour<PlayerSpawnHandler>
{

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _bugsPrefab;

    private GameObject _currentPlayer = null;
    private GameObject _currentBugs = null;

    protected override void Initialize()
    {
        //TODO get initial spawn point, probably won't be loaded yet actually
    }

    public void Spawn(Transform spawnAnchor, Action onSpawnComplete = null)
    {
        if(_currentPlayer == null)
        {
            _currentPlayer = Instantiate(_playerPrefab);
            _currentPlayer.transform.parent = this.transform;

            CameraBehaviourController cameraBehaviourController = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).GetComponent<CameraBehaviourController>();
            cameraBehaviourController.SetPlayerTransform(_currentPlayer.transform);
            cameraBehaviourController.SetCameraBehaviourState(CameraBehaviourController.CameraBehaviourState.FollowPlayer);
            cameraBehaviourController.SetCameraMinimumHeight(GameObject.FindObjectOfType<CinemachinePath>());
        }
        _currentPlayer.transform.position = spawnAnchor.position;

        if(_currentBugs == null)
        {
            _currentBugs = Instantiate(_bugsPrefab);
            _currentBugs.transform.position = spawnAnchor.position;
            _currentBugs.GetComponent<BugsController>().SetPlayerTransform(_currentPlayer.transform);
        }

        //TODO set player idle animation here

        PlayerHealthController.Instance.FullHeal();
        PlayerController.Instance.SetInitialState();

        if(!FullScreenWipe.IsWiping)
            FullScreenWipe.FadeOut(1, OnAnimationCompleted);

        onSpawnComplete?.Invoke();
    }
    
    private void OnAnimationCompleted()
    {
        //TODO do player animation stuff here
    }
}
