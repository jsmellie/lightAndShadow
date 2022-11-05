using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulser : MonoBehaviour
{
    [SerializeField] private Light _light;

    private float _maxIntensity = 125f;
    private float _minIntensity = 40f;

    // Update is called once per frame
    void FixedUpdate()
    {
        _light.intensity = Mathf.Lerp(_maxIntensity, _minIntensity, Mathf.Sin(Time.time));
    }
}
