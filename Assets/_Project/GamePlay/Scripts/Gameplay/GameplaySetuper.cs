using System.Collections;
using System.Collections.Generic;
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

            FullScreenWipe.FadeIn(0, null);
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

                VirtualCameraManager.Instance.OnStageLoaded();

                FullScreenWipe.FadeOut(1, OnAnimationCompleted);
            }
        }
    }

    private void OnAnimationCompleted()
    {
        
    }
}
