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

    private void Awake()
    {
        if (!StageLoader.IsInstanceNull)
        {
            StageLoader.Instance.OnStageLoaded += OnStageLoaded;
        }
        else
        {
            StageLoader.Instance.SetStageInfo(_zone, _stageIndex);
            OnStageLoaded(_zone, _stageIndex, LevelLoadingErrorCodes.None);
        }
    }

    private void OnStageLoaded(Zones zone, int stageIndex, LevelLoadingErrorCodes errorCode)
    {
        if (_zone == zone && _stageIndex == stageIndex)
        {
            if (errorCode == LevelLoadingErrorCodes.None)
            {
                Transform player = Instantiate<Transform>(_playerPrefab);
                player.parent = this.transform;
                player.position = _spawnAnchor.position;

                VirtualCameraManager.Instance.OnStageLoaded();

                GameObject guiCameraObj = GameObject.FindGameObjectWithTag("GUICamera");

                if (guiCameraObj == null)
                {
                    Camera guiCamera = new GameObject("GUI Camera", typeof(Camera)).GetComponent<Camera>();
                    guiCamera.cullingMask = LayerMask.GetMask("UI");
                    guiCamera.clearFlags = CameraClearFlags.Depth;
                    guiCamera.transform.tag = "GUICamera";
                    DontDestroyOnLoad(guiCamera.gameObject);
                }
            }
        }
    }
}
