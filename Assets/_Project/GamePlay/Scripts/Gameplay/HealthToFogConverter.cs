using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthToFogConverter : SingletonBehaviour<HealthToFogConverter>
{
   
    public int Health {get; private set;}

    void Start()
    {
        PlayerHealthController.Instance.OnHealthChanged += SaveHealth;
    }

    void SaveHealth(int health)
    {
        Health = health;
    }

    protected override void Initialize()
    {
        //stub
    }
}
