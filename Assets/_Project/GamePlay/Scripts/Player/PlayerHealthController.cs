using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    
    [SerializeField] private float MaxHealth = 100f;
    [SerializeField] private float DifficultyScale = 2f;
    private int BEATS_PER_DAMAGE = 8;
    private int MAX_SAFE_BEATS = 2;
    private float _health = 100f;
    private int _beatCounter = 0;
    private int _safeBeats = 0;

    private void CheckDeath()
    {
        if (_health <= 0)
        Debug.Log("ded"); //trigger death
    }

    [ContextMenu("MakeSafe")]
    public void MakeSafe()
    {
        _safeBeats = MAX_SAFE_BEATS;
    }

    public void HandleBeat()
    {
        _beatCounter++;
        if(_beatCounter >= BEATS_PER_DAMAGE)
        {
            if(_safeBeats > 0)
            {
                _safeBeats--;
            }
            else
            {
                Damage(5);
            }

            _beatCounter -= BEATS_PER_DAMAGE;
        }
    }

    public void Damage(float losthealth = 20)
    {
        _health -= losthealth;
        CheckDeath();
    }

    public void Heal(float addedHealth = 20)
    {
        _health = Mathf.Min(MaxHealth, _health + addedHealth);
    }

    [ContextMenu("Full Heal")]
    public void FullHeal()
    {
        Heal(1000);
    }

}
