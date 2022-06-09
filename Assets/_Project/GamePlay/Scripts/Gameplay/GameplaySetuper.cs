using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySetuper : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] private Zones _zone;
    [SerializeField] private int _stageIndex;
    [Header("Setup")]
    [SerializeField] private Transform _spawnAnchor;
    [SerializeField] private Transform _playerPrefab;
    [SerializeField] private Transform _bugsPrefab;

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            StageUtility.GetZoneAndStageFromString(this.gameObject.scene.name, out _zone, out _stageIndex);
        }
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            CameraController cameraController = CameraController.Instance;

            //FullScreenWipe.FadeIn(0, null);
            StageController.Instance.SetStageInfo(_zone, _stageIndex);
            OnStageLoaded(_zone, _stageIndex, LevelLoadingErrorCodes.None);
        }
    }

    private void OnStageLoaded(Zones zone, int stageIndex, LevelLoadingErrorCodes errorCode)
    {
        if (!StageController.IsInstanceNull)
        {
            StageController.Instance.OnStageLoaded -= OnStageLoaded;
        }
        if (_zone == zone && _stageIndex == stageIndex)
        {
            if (errorCode == LevelLoadingErrorCodes.None)
            {
                Transform player = Instantiate<Transform>(_playerPrefab);
                player.parent = this.transform;
                player.position = _spawnAnchor.position;

                var bugs = Instantiate(_bugsPrefab);
                bugs.transform.position = _spawnAnchor.position;
                bugs.GetComponent<BugsController>().SetPlayerTransform(player.transform);

                CameraBehaviourController cameraBehaviourController = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).GetComponent<CameraBehaviourController>();
                cameraBehaviourController.SetPlayerTransform(player);
                cameraBehaviourController.SetCameraBehaviourState(CameraBehaviourController.CameraBehaviourState.FollowPlayer);
                cameraBehaviourController.SetCameraMinimumHeight(GameObject.FindObjectOfType<CinemachinePath>());

                if(CheckpointManager.Instance.CurrentCheckpoint == 0)
                {
                    //FullScreenWipe.FadeOut(1, OnAnimationCompleted);
                    CheckpointManager.Instance.SaveCheckpoint(1);
                }
            }
        }
    }

    private void OnAnimationCompleted()
    {
        
    }
}
