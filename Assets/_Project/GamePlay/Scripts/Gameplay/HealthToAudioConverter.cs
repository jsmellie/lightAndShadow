using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthToAudioConverter : MonoBehaviour
{
    private const int LEVEL_1_THRESHOLD = 21;
    private const int LEVEL_2_THRESHOLD = 41;
    private const int LEVEL_3_THRESHOLD = 61;
    private const int LEVEL_4_THRESHOLD = 81;

    [SerializeField] private LayeredMusicController _layeredMusicController;
    

    void Start()
    {
        PlayerHealthController.Instance.OnHealthChanged += UpdateAudioLayer;
    }

    private void UpdateAudioLayer(int health)
    {
        if(health < LEVEL_1_THRESHOLD)
        {
            _layeredMusicController.SetStage(0);
        }
        else if (health < LEVEL_2_THRESHOLD)
        {
            _layeredMusicController.SetStage(1);
        }
        else if (health < LEVEL_3_THRESHOLD)
        {
            _layeredMusicController.SetStage(2);
        }
        else if (health < LEVEL_4_THRESHOLD)
        {
            _layeredMusicController.SetStage(3);
        }
        else 
        {
            _layeredMusicController.SetStage(4);
        }

    }

}
