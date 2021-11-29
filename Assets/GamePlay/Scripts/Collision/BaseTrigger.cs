using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BaseTrigger: MonoBehaviour
{
    public delegate void OnTrigeredDelegate(BaseTrigger trigger, Collider2D collider);

    public event OnTrigeredDelegate OnTrigerEnter;
    public event OnTrigeredDelegate OnTrigerExit;

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if (OnTrigerEnter != null)
        {
            OnTrigerEnter.Invoke(this, collider);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        if (OnTrigerExit != null)
        {
            OnTrigerExit.Invoke(this, collider);
        }
    }
}
