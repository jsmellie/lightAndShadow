using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeTrigger : BaseTrigger
{
    [System.Serializable]
    private struct CameraTriggerData
    {
        public bool ShouldChangeCamera;
        public CameraID CameraID;

        public bool ShouldMoveStaticTarget;
        public Vector3 StaticTargetPosition;
    }
    private VirtualCameraManager _cameraManager;

    [SerializeField] private CameraTriggerData _onTriggerEnterCameraData;
    [Space]
    [SerializeField] private bool _shouldReturnToPreviousCameraOnExit;
    [SerializeField] private CameraTriggerData _onTriggerExitCameraData;

    private CameraID? _prevCameraID = null;

    private void Awake()
    {
        _cameraManager = VirtualCameraManager.Instance;
    }
    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        if (_onTriggerEnterCameraData.ShouldChangeCamera)
        {
            _prevCameraID = _cameraManager.CurrentCameraID;

            _cameraManager.SwitchCamera(_onTriggerEnterCameraData.CameraID);

            if (_onTriggerEnterCameraData.ShouldMoveStaticTarget)
            {
                _cameraManager.MoveStaticTarget(_onTriggerEnterCameraData.StaticTargetPosition);
            }
        }
    }

    protected override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);

        if (_shouldReturnToPreviousCameraOnExit && _prevCameraID.HasValue)
        {
            _cameraManager.SwitchCamera(_prevCameraID.Value);
        }
        else if (_onTriggerExitCameraData.ShouldChangeCamera)
        {
            _cameraManager.SwitchCamera(_onTriggerExitCameraData.CameraID);

            if (_onTriggerExitCameraData.ShouldMoveStaticTarget)
            {
                _cameraManager.MoveStaticTarget(_onTriggerExitCameraData.StaticTargetPosition);
            }
        }
    }
}
