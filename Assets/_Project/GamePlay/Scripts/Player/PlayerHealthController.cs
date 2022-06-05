using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : SingletonBehaviour<PlayerHealthController>
{
    
    [SerializeField] private int MaxHealth = 100;
    private int BEATS_PER_DAMAGE = 2;
    private int MAX_SAFE_BEATS = 1;
    private int _health = 40;
    private int _beatCounter = 0;
    private int _safeBeats = 0;

    public Action<int> OnHealthChanged;
    public Action OnDeath;

    void Start()
    {
        AudioController.Instance.OnBeat += HandleBeat;
    }

    protected override void Initialize()
    {
        //stub
    }

    private void CheckDeath()
    {
        if (_health <= 0) OnDeath?.Invoke(); //more death stuff here
    }

    [ContextMenu("MakeSafe")]
    public void MakeSafe()
    {
        _safeBeats = MAX_SAFE_BEATS;
    }

    public void HandleBeat(int beat)
    {
        if(beat == 0)
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
                    Damage(10);
                }

                _beatCounter -= BEATS_PER_DAMAGE;
            }
        }
    }

    public void Damage(int losthealth = 20)
    {
        _health -= losthealth;
        OnHealthChanged?.Invoke(_health);
        CheckDeath();
    }

    public void Heal(int addedHealth = 20)
    {
        _health = Mathf.Min(MaxHealth, _health + addedHealth);
        OnHealthChanged?.Invoke(_health);
    }

    [ContextMenu("Full Heal")]
    public void FullHeal()
    {
        Heal(1000);
    }

}
