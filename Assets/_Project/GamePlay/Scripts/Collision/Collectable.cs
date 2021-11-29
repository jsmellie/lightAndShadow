using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : BaseTrigger
{
    [SerializeField] protected ParticleSystem _particles;
    [SerializeField] protected Collider2D _collider;
    [SerializeField] protected GameObject _visual;
    
    protected override void OnTriggerEnter2D(Collider2D collider)
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

        base.OnTriggerEnter2D(collider);
    }
}
