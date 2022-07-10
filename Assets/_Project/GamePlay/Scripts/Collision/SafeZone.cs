using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : BaseTrigger
{
    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        PlayerHealthController.Instance.FullHeal();
        PlayerHealthController.Instance.SetHealthDrainPaused(true);
    }

    public override void OnTriggerExit(Collider collider)
    {
        base.OnTriggerExit(collider);
        PlayerHealthController.Instance.SetHealthDrainPaused(false);
    }
}
