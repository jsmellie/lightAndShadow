using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(menuName = "Light & Shadow/Settings/Virtual Camera Manager Settings")]
public class VirtualCameraManagerSettings : ScriptableObject
{
    public VirtualCameraData[] CameraDatas;

    public CameraID DefaultCamera;
}
