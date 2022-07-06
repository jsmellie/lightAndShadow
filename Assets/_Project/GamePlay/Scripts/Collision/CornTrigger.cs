using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornTrigger : BaseTrigger
{
    public bool IsColliding = false;

    public override void OnTriggerEnter(Collider collider)
    {
        IsColliding = true;
        base.OnTriggerEnter(collider);

    }
    public override void OnTriggerExit(Collider collider)
    {
        IsColliding = false;
        base.OnTriggerExit(collider);
    }
}
