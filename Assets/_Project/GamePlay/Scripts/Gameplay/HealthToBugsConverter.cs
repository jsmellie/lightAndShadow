using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthToBugsConverter : MonoBehaviour
{

    private readonly Dictionary<int, float> _healthRadiusDictionary = new Dictionary<int, float>()
    {
        {1, 0.95f},
        {11, 0.9f},
        {21, 0.8f},
        {31, 0.7f},
        {41, 0.6f},
        {51, 0.5f},
        {61, 0.4f},
        {71, 0.3f},
        {81, 0.2f},
        {91, 0.1f},
        {1000, 0f}
    };

    [SerializeField] private BugsController _bugsController;

    void Start()
    {
        PlayerHealthController.Instance.OnHealthChanged += UpdateBugsRadius;
    }

    private void UpdateBugsRadius(int health)
    {
        if (_bugsController.OverrideHealth)
        {
            return;
        }
        
        foreach (int threshold in _healthRadiusDictionary.Keys)
        {
            if (health < threshold)
            {
                _bugsController.SetRadius(_healthRadiusDictionary[threshold]);
                break;
            }
        }
    }
}
