using System;
using UnityEngine;

public class DebugCameraSwitcher : MonoBehaviour
{
    private CameraID[] _allIDs;

    private void OnEnable()
    {
        if (_allIDs == null)
        {
            _allIDs = Enum.GetValues(typeof(CameraID)) as CameraID[];
        }
    }

    public void CycleCamera()
    {
        CameraID id = VirtualCameraManager.Instance.CurrentCameraID;

        int currentIDIndex = -1;

        for(int i = 0; i < _allIDs.Length; ++i)
        {
            if (_allIDs[i] == id)
            {
                currentIDIndex = i;
                break;
            }
        }

        bool didChangeCamera = false;

        do
        {
            currentIDIndex = (currentIDIndex + 1) % _allIDs.Length;
            didChangeCamera = VirtualCameraManager.Instance.SwitchCamera(_allIDs[currentIDIndex]);
        }while (!didChangeCamera);
    }
}
