using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAnimationController;

public class SwingTrigger : AnimationTrigger
{
    [SerializeField] private SlideRunner _path;
    [SerializeField] private RopeSwinger _rope;

    public override void OnTriggerEnter(Collider collider)
    {
        _path.StartSlide(collider.transform);
        _rope.Begin();
        base.OnTriggerEnter(collider);
        PlayerController.Instance.DetectTriggers(true);
    }
}
