using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    private struct PresetCameraData
    {
        public string ResourcesPath;
        public string ID;

        public PresetCameraData(string resourcePath, string id)
        {
            ResourcesPath = resourcePath;
            ID = id;
        }
    }

    private static readonly PresetCameraData[] _presetCameraData = new PresetCameraData[] {
        new PresetCameraData("Prefabs/Camera/GameplayCamera", "Gameplay"),
        new PresetCameraData("Prefabs/Camera/UICamera", "UI")
    };

    private Dictionary<string, Camera> _loadedCameras;

    public override void Initialize()
    {
        _loadedCameras = new Dictionary<string, Camera>(_presetCameraData.Length);

        for(int i = 0; i < _presetCameraData.Length; ++i)
        {
            PresetCameraData cameraData = _presetCameraData[i];
            Camera loadedCamera = Resources.Load<Camera>(cameraData.ResourcesPath);
            loadedCamera = GameObject.Instantiate<Camera>(loadedCamera);
            GameObject.DontDestroyOnLoad(loadedCamera.gameObject);

            _loadedCameras.Add(cameraData.ID, loadedCamera);
        }
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
