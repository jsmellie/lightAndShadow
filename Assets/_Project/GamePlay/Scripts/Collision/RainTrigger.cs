using UnityEngine;

public class RainTrigger : BaseTrigger
{
    [SerializeField] private bool _enable = true;

    public override void OnTriggerEnter(Collider collider)
    {
        CameraEffectController.Instance.ToggleRain(_enable);

        base.OnTriggerEnter(collider);

    }
}
