using UnityEngine;

public class RainTrigger : BaseTrigger
{
    [SerializeField] private bool _enable = true;
    [SerializeField] private float _scale = -1f;

    public override void OnTriggerEnter(Collider collider)
    {
        CameraEffectController.Instance.ToggleRain(_enable);
        
        CameraEffectController.Instance.SetTargetRainScale(_scale);

        base.OnTriggerEnter(collider);

    }
}
