using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerController : MonoBehaviour
{
    [SerializeField] protected LayerMask _triggerLayers;

    protected List<Collider> _collidingTriggers = new List<Collider>();

    private void FixedUpdate()
    {
        Vector3 colliderExtents = GetComponent<BoxCollider>().bounds.extents;
        Vector3 detectorExtents = colliderExtents * 0.99f;

        Collider[] colliders = Physics.OverlapBox(transform.position, detectorExtents, transform.rotation, _triggerLayers, QueryTriggerInteraction.Collide);

        List<Collider> collidersThisFrame = new List<Collider>();

        foreach(Collider collider in colliders)
        {
            collidersThisFrame.Add(collider);
            AddCollidingTrigger(collider);
        }

        RemoveUnusedTriggers(collidersThisFrame);

        TriggerEnterColliders(_collidingTriggers);
    }

    private void TriggerEnterColliders(List<Collider> colliders)
    {
        foreach (Collider collider in colliders)
        {
            if (!_collidingTriggers.Contains(collider))
            {
                collider.GetComponent<BaseTrigger>().OnTriggerEnter(GetComponent<BoxCollider>());
                AddCollidingTrigger(collider);
            }
        }
    }

    private void RemoveUnusedTriggers(List<Collider> collidersInUse)
    {
        List<Collider> collidersToRemove = new List<Collider>();

        foreach (Collider collider in _collidingTriggers)
        {
            if (!collidersInUse.Contains(collider))
            {
                collidersToRemove.Add(collider);
                collider.GetComponent<BaseTrigger>().OnTriggerExit(GetComponent<BoxCollider>());
            }
        }

        foreach(Collider collider in collidersToRemove)
        {
            RemoveCollidingTrigger(collider);
        }
    }

    private void AddCollidingTrigger(Collider trigger)
    {
        if (!_collidingTriggers.Contains(trigger))
        {
            _collidingTriggers.Add(trigger);
        }
    }

    private void RemoveCollidingTrigger(Collider trigger)
    {
        if (_collidingTriggers.Contains(trigger))
        {
            _collidingTriggers.Remove(trigger);
        }
    }
}
