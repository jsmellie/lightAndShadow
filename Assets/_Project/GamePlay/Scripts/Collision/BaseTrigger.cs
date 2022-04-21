using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseTrigger: MonoBehaviour
{
    public delegate void OnTrigeredDelegate(BaseTrigger trigger, Collider collider);

    public event OnTrigeredDelegate TriggerEnterEvent;
    public event OnTrigeredDelegate TriggerExitEvent;

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (TriggerEnterEvent != null)
        {
            TriggerEnterEvent.Invoke(this, collider);
        }
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        if (TriggerExitEvent != null)
        {
            TriggerExitEvent.Invoke(this, collider);
        }
    }
}
