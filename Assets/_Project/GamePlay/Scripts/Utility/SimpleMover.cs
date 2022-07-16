using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMover : MonoBehaviour
{
    [SerializeField] private AnimationCurve _xCurve;
    [SerializeField] private AnimationCurve _yCurve;
    [SerializeField] private float _baseSpeed = 1f;



    private float _time = 0f;

    void Update()
    {
        _time += Time.deltaTime * _baseSpeed * GetScale();
        while (_time > 1)
        {
            _time -= 1;
        }

        transform.localPosition = new Vector3(_xCurve.Evaluate(_time), _yCurve.Evaluate(_time), transform.localPosition.z);
    }

    private float GetScale()
    {
        return 1;
    } 
}
