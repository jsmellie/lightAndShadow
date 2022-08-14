using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffectController : SingletonBehaviour<CameraEffectController>
{

    [SerializeField] private GameObject _rainRoot;
    [SerializeField] private List<SpriteRenderer> _rainLayers;
    [SerializeField] private SpriteRenderer _darkness;

    private Color OFF = new Color(1,1,1,0);
    private Color ON = new Color(1,1,1,1);

    private float _currentRainScale = 0f;

    private float _targetRainScale = 0f;

    protected override void Initialize()
    {
       ToggleRain(false);
    }

    public void ToggleRain(bool toggle)
    {
        _rainRoot.SetActive(toggle);
    }

    public void ScaleRain(float scale)
    {
        for (int i = 0; i<_rainLayers.Count; i++)
        {
            if(scale <= i)
            {
                _rainLayers[i].color = OFF;
            }
            else if (scale >= i +1)
            {
                _rainLayers[i].color = ON;
            }
            else
            {
                _rainLayers[i].color = new Color(1,1,1, scale-i);
            }

        }
        
    }

    public void SetTargetRainScale(float scale)
    {
        _targetRainScale = scale;
    }

    public void ToggleDarkness(bool toggle)
    {
        _darkness.gameObject.SetActive(toggle);
    }

    void Update()
    {
        if(_currentRainScale !=_targetRainScale)
        {
            _currentRainScale = Mathf.Lerp(_currentRainScale,_targetRainScale,0.5f);
            if(Mathf.Abs(_targetRainScale-_currentRainScale) < 0.05f)
            {
                _currentRainScale = _targetRainScale;
            }
            ScaleRain(_currentRainScale);
        }
    }
}
