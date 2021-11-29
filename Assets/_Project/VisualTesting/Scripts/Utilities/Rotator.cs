using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationAmount = Vector3.zero;
    private Transform _xform = null;

    private void Update()
    {
        if (_xform == null)
        {
            _xform = this.transform;
        }

        if (_rotationAmount.sqrMagnitude > 0)
        {
            Vector3 rotation = _xform.localEulerAngles;
            rotation += _rotationAmount * Time.deltaTime;
            _xform.localEulerAngles = rotation;
        }
    }
}
