using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimationController;

public class MineSwingTrigger : SwingTrigger
{
    //drop lantern
    [SerializeField]private Transform _lantern;
    [SerializeField]private Transform _parent;

    public override void OnTriggerEnter(Collider collider)
    {
        
        _lantern.SetParent(_parent);
        base.OnTriggerEnter(collider);

    }
}
