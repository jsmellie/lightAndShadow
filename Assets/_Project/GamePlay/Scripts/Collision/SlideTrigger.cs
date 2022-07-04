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
        PlayerHealthController.Instance.OnDeath -= ResetOnDeath;
        PlayerHealthController.Instance.OnDeath += ResetOnDeath;
        base.OnTriggerEnter(collider);
        PlayerController.Instance.DetectTriggers(true);
    }
    void OnDestroy()
    {
        if(!PlayerHealthController.IsInstanceNull)
            PlayerHealthController.Instance.OnDeath -= ResetOnDeath;
    }
    private void ResetOnDeath()
    {
        gameObject.SetActive(true);
    }

}
