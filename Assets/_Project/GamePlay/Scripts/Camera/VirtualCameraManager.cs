using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VirtualCameraManager : Singleton<VirtualCameraManager>
{
    private const string SettingsPath = "ScriptableObjects/Settings/VirtualCameraManagerSettings";

    private const int DisabledPriority = 10;
    private const int EnabledPriority = DisabledPriority + 1;
    private VirtualCameraManagerSettings _settings;
    private Transform _staticFollowTarget = null;
    private CinemachineBrain _brain;

    private Dictionary<CameraID, CinemachineVirtualCameraBase> _loadedCameras = null;

    private CameraID _currentCameraID;
    public CameraID CurrentCameraID
    {
        get { return _currentCameraID; }
    }

    public override void Initialize()
    {
        if (_settings == null)
        {
            _settings = Resources.Load<VirtualCameraManagerSettings>(SettingsPath);
            Debug.Log($"Did load Virtual Camera Settings - {_settings != null}");
        }

        if (_staticFollowTarget == null)
        {
            _staticFollowTarget = new GameObject("VirtualCamera_StaticFollowTarget").transform;
            GameObject.DontDestroyOnLoad(_staticFollowTarget);
        }
    }

    public void OnStageLoaded()
    {
        if (_brain == null)
        {
            Camera gameplayCamera = GameObject.FindGameObjectWithTag("GameplayCamera").GetComponent<Camera>();
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            _staticFollowTarget.position = player.position;
            _brain = gameplayCamera.GetComponent<CinemachineBrain>();
            _brain.enabled = false;

            VirtualCameraData[] cameraDatas = _settings.CameraDatas;

            if (_loadedCameras == null)
            {
                _loadedCameras = new Dictionary<CameraID, CinemachineVirtualCameraBase>(cameraDatas.Length);
            }
            else
            {
                CleanUpCameras();
            }

            foreach(VirtualCameraData cameraData in cameraDatas)
            {
                CinemachineVirtualCameraBase currentCamera = GameObject.Instantiate<CinemachineVirtualCameraBase>(cameraData.Camera);
                if (cameraData.FollowData.IsFollowing)
                {
                    if (cameraData.FollowData.Type == VirtualCameraFollowData.FollowType.Player)
                    {
                        currentCamera.Follow = player;
                    }
                    else
                    {
                        currentCamera.Follow = _staticFollowTarget;
                    }
                }
                currentCamera.Priority = DisabledPriority;
                currentCamera.transform.parent = gameplayCamera.transform;
                _loadedCameras.Add(cameraData.ID, currentCamera);
            }

            _currentCameraID = _settings.DefaultCamera;
            CinemachineBlendDefinition originalBlend = _brain.m_DefaultBlend;
            _loadedCameras[_currentCameraID].Priority = EnabledPriority;

            _brain.enabled = true;
        }
    }

    public void CleanUpStage()
    {
        CleanUpCameras();

        _brain = null;
    }

    public bool SwitchCamera(CameraID id)
    {
        bool result = true;

        if (id != _currentCameraID)
        {
            if (_loadedCameras.ContainsKey(id))
            {
                _loadedCameras[_currentCameraID].Priority = DisabledPriority;
                _currentCameraID = id;
                _loadedCameras[_currentCameraID].Priority = EnabledPriority;
            }
            else
            {
                return false;
            }
        }

        return result;
    }

    public bool MoveStaticTarget(Vector2 position)
    {
        bool result = true;

        if (_staticFollowTarget != null)
        {
            Vector3 targetPosition = _staticFollowTarget.position;
            targetPosition.x = position.x;
            targetPosition.y = position.y;
            _staticFollowTarget.position = targetPosition;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private void CleanUpCameras()
    {
        if (_loadedCameras != null)
        {   
            foreach(KeyValuePair<CameraID, CinemachineVirtualCameraBase> pair in _loadedCameras)
            {
                if (pair.Value != null)
                {
                    GameObject.Destroy(pair.Value.gameObject);
                }
            }

            _loadedCameras.Clear();
        }
    }
}
