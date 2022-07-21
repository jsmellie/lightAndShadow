using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public async Task Spawn(Transform spawnAnchor, Action onSpawnComplete = null)
    {
        if(_currentPlayer == null)
        {
            _currentPlayer = Instantiate(_playerPrefab);
            _currentPlayer.transform.parent = this.transform;

            PlayerController.Instance.SetInitialState();

        }
            CameraBehaviourController cameraBehaviourController = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).GetComponent<CameraBehaviourController>();
            cameraBehaviourController.SetPlayerTransform(_currentPlayer.transform);
            cameraBehaviourController.SetCameraBehaviourState(CameraBehaviourController.CameraBehaviourState.FollowPlayer);
            cameraBehaviourController.SetCameraMinimumHeight();
        _currentPlayer.transform.position = spawnAnchor.position;

            CameraEffectController.Instance.ToggleDarkness(CheckpointManager.Instance.CurrentCheckpoint == 27 ||
            CheckpointManager.Instance.CurrentCheckpoint == 28 ||
            CheckpointManager.Instance.CurrentCheckpoint == 29 );

     
        cameraBehaviourController.SnapToTarget();
        cameraBehaviourController.ForceCurrentPosition();

        if(_currentBugs == null)
        {
            _currentBugs = Instantiate(_bugsPrefab);
            _currentBugs.transform.position = spawnAnchor.position;
            _currentBugs.GetComponent<BugsController>().SetPlayerTransform(_currentPlayer.transform);
        }

        _currentBugs.GetComponent<BugsController>().ForceTargetPosition();
        _currentBugs.GetComponent<BugsController>().ForceRadius();
        onSpawnComplete?.Invoke();

        await Task.CompletedTask;
    }
    
    private void OnAnimationCompleted()
    {
        //TODO do player animation stuff here
    }
}
