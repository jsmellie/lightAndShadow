using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonBehaviour<CameraController>
{
    public const string GAMEPLAY_CAMERA_ID = "Gameplay";
    public const string UI_CAMERA_ID = "UI";

    public const string VIDEO_CAMERA_ID = "Cutscene";

    [SerializeField] private GameObject _gameCameraPrefab;
    [SerializeField] private GameObject _uiCameraPrefab;
    [SerializeField] private GameObject _videoCameraPrefab;

    private Dictionary<string, Camera> _loadedCameras = new Dictionary<string, Camera>();

    protected override void Initialize()
    {
        Camera gameCamera = Instantiate<GameObject>(_gameCameraPrefab).GetComponent<Camera>();
        Camera uiCamera = Instantiate<GameObject>(_uiCameraPrefab).GetComponent<Camera>();
        Camera videoCamera = Instantiate<GameObject>(_videoCameraPrefab).GetComponent<Camera>();
        gameCamera.transform.parent = transform;
        uiCamera.transform.parent = transform;
        videoCamera.transform.parent = transform;
        videoCamera.gameObject.SetActive(false);
        _loadedCameras.Add(GAMEPLAY_CAMERA_ID, gameCamera);
        _loadedCameras.Add(UI_CAMERA_ID, uiCamera);
        _loadedCameras.Add(VIDEO_CAMERA_ID, videoCamera);
    }

    public Camera GetCamera(string ID)
    {
        if (_loadedCameras.ContainsKey(ID))
        {
            return _loadedCameras[ID];
        }
        else
        {
            return null;
        }
    }

    public void AddCamera(string ID, Camera camera)
    {
        if (string.IsNullOrEmpty(ID))
        {
            throw new ArgumentNullException(nameof(ID));
        }

        if (camera == null)
        {
            throw new ArgumentNullException(nameof(camera));
        }

        if (_loadedCameras.ContainsKey(ID))
        {
            throw new NotSupportedException($"ID {ID} already present in {nameof(_loadedCameras)}");
        }

        _loadedCameras.Add(ID, camera);
    }

    public Camera RemoveCamera(string ID)
    {
        Camera removedCamera = null;

        if (string.IsNullOrEmpty(ID))
        {
            throw new ArgumentNullException(nameof(ID));
        }

        if (!_loadedCameras.ContainsKey(ID))
        {
            throw new NotSupportedException($"ID {ID} not present in {nameof(_loadedCameras)}");
        }

        removedCamera = _loadedCameras[ID];
        _loadedCameras.Remove(ID);

        return removedCamera;
    }
}
