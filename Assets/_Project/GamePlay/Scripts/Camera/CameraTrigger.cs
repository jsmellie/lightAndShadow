using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraBehaviourController;

public class CameraTrigger : BaseTrigger
{
    [SerializeField] private CameraBehaviourInfo _info;

    [SerializeField] private CameraBehaviourState _cameraState;
    [SerializeField] private bool _disableAfterEntry;
    [SerializeField] private bool _returnToPreviousStateOnExit;

    private CameraBehaviourInfo _previousInfo;
    private CameraBehaviourState _previousState;
    private Transform _previousTarget;

    public override void OnTriggerEnter(Collider collider)
    {
        CameraBehaviourController behaviourController = collider.GetComponent<CameraBehaviourController>();

        if (_returnToPreviousStateOnExit)
        {
            _previousInfo = behaviourController.GetCameraBehaviourInfo();
            _previousState = behaviourController.GetCameraBehaviourState();
            _previousTarget = behaviourController.GetCameraTarget();
        }

        behaviourController.SetCameraBehaviourState(_cameraState);
        behaviourController.SetCameraBehaviourInfo(_info);

        if (_info.TargetTransform != null)
        {
            behaviourController.SetCameraTarget(_info.TargetTransform);
        }

        if (_disableAfterEntry)
        {
            gameObject.SetActive(false);
        }

        base.OnTriggerEnter(collider);
    }

    public override void OnTriggerExit(Collider collider)
    {
        if (_returnToPreviousStateOnExit)
        {
            CameraBehaviourController behaviourController = collider.GetComponent<CameraBehaviourController>();

            behaviourController.SetCameraBehaviourState(_previousState);
            behaviourController.SetCameraBehaviourInfo(_previousInfo);
            behaviourController.SetCameraTarget(_previousTarget);
        }

        base.OnTriggerExit(collider);
    }
}
