using Cinemachine;

public enum CameraID
{
    Regular = 0,
    ZoomedOut = 1,
    ZoomedIn = 2
}

[System.Serializable]
public class VirtualCameraData
{
    public CameraID ID;
    public CinemachineVirtualCameraBase Camera;

    public VirtualCameraFollowData FollowData;
}

[System.Serializable]
public class VirtualCameraFollowData
{
    public enum FollowType
    {
        Player = 0,
        StaticPoint = 1
    }
    public bool IsFollowing;
    public FollowType Type = FollowType.Player;
}
