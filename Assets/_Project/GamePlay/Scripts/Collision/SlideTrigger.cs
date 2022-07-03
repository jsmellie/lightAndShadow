using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SlideTrigger : AnimationTrigger
{
    [SerializeField] private SlideRunner _slider;



    public override void OnTriggerEnter(Collider collider)
    {
        _slider.StartSlide(collider.transform);
       
        base.OnTriggerEnter(collider);
        PlayerController.Instance.DetectTriggers(true);
    }



}
