using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonBehaviour<CameraController>
{
    public const string GAMEPLAY_CAMERA_ID = "Gameplay";
    public const string UI_CAMERA_ID = "UI";

    [SerializeField] private GameObject _gameCameraPrefab;
    [SerializeField] private GameObject _uiCameraPrefab;

    private Dictionary<string, Camera> _loadedCameras = new Dictionary<string, Camera>();

    protected override void Initialize()
    {
        Camera gameCamera = Instantiate<GameObject>(_gameCameraPrefab).GetComponent<Camera>();
        Camera uiCamera = Instantiate<GameObject>(_uiCameraPrefab).GetComponent<Camera>();
        gameCamera.transform.parent = transform;
        uiCamera.transform.parent = transform;

        _loadedCameras.Add(GAMEPLAY_CAMERA_ID, gameCamera);
        _loadedCameras.Add(UI_CAMERA_ID, uiCamera);
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
