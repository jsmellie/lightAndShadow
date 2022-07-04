using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SlideRunner : MonoBehaviour
{
    [SerializeField] private CinemachinePath _path;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay = 0f;
    [SerializeField] private Transform _slideEnd;
    [SerializeField] private GameObject _endTriggerObject;
    
    private float _pathTravelled = 0f;

    private bool _triggered = false;
    private Transform _player;

    public void StartSlide(Transform player)
    {
        _player = player;
        _pathTravelled = 0f;
        _endTriggerObject.SetActive(true);
        _triggered = true;
    }


    void Update()
    {
        if(_triggered)
        {
            if(_delay > 0)
            {
                _delay -= Time.deltaTime;
                return;
            }

                _player.position = _path.EvaluatePosition(_pathTravelled);
                _pathTravelled += Time.deltaTime * _speed;

            if(_player.position.x > _slideEnd.position.x)
            {
                _triggered = false;
            }
        }
        
    }

}
