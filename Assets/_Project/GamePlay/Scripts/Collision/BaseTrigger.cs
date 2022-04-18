using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BaseTrigger: MonoBehaviour
{
    public delegate void OnTrigeredDelegate(BaseTrigger trigger, Collider collider);

    public event OnTrigeredDelegate OnTrigerEnter;
    public event OnTrigeredDelegate OnTrigerExit;

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (OnTrigerEnter != null)
        {
            OnTrigerEnter.Invoke(this, collider);
        }
    }

    protected virtual void OnTriggerExit(Collider collider)
    {
        if (OnTrigerExit != null)
        {
            OnTrigerExit.Invoke(this, collider);
        }
    }
}
