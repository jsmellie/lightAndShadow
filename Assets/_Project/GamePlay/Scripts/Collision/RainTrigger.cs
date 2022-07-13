using UnityEngine;

public class RainTrigger : BaseTrigger
{
    [SerializeField] private bool _enable = true;

    public override void OnTriggerEnter(Collider collider)
    {
        var camera = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID);
        camera.transform.GetChild(0).gameObject.SetActive(_enable);

        base.OnTriggerEnter(collider);

    }
}
