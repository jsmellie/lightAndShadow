using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeTrigger : BaseTrigger
{
    public override void OnTriggerEnter(Collider collider)
    {
        PlayerController.Instance.EnableSlow(true);
        base.OnTriggerEnter(collider);
    }

    public override void OnTriggerExit(Collider collider)
    {
        PlayerController.Instance.EnableSlow(false);
        base.OnTriggerExit(collider);
    }
}
