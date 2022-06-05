using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : BaseTrigger
{
    [SerializeField] protected ParticleSystem _particles;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected GameObject _visual;
    [SerializeField] protected int _healthRestored = 10;
    
    public override void OnTriggerEnter(Collider collider)
    {
        if (_particles != null)
        {
            _particles.Play();
        }

        if (_collider != null)
        {
            _collider.enabled = false;
        }

        if (_visual != null)
        {
            _visual.SetActive(false);
        }

        PlayerHealthController.Instance.MakeSafe();
        PlayerHealthController.Instance.Heal(_healthRestored);

        base.OnTriggerEnter(collider);
    }
}
