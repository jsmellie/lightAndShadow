using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : SingletonBehaviour<PlayerHealthController>
{
    
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private bool LoseHealth = false;
    private int BEATS_PER_DAMAGE = 1;
    private int MAX_SAFE_BEATS = 2;
    private int _health = 100;
    private int _beatCounter = 0;
    private int _safeBeats = 0;
    private bool _isDead = false;
    private bool _isPaused = false;

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
        if (_health <= 0)
        {
            _isDead = true;
            OnDeath?.Invoke(); //more death stuff here, probably a death audio trill?
        } 
            
    }

    public void SetHealthDrainPaused (bool paused)
    {
        _isPaused = paused;
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
                    if(!_isDead)
                        Damage(5);
                }

                _beatCounter -= BEATS_PER_DAMAGE;
            }
        }
    }

    public void Damage(int losthealth = 20)
    {
        if(!_isPaused)
        {
#if UNITY_EDITOR
            if(!LoseHealth)
                return;
#endif
            _health -= losthealth;
            OnHealthChanged?.Invoke(_health);
            CheckDeath();
        }
    }

    public void Heal(int addedHealth = 20)
    {
        _health = Mathf.Min(MaxHealth, _health + addedHealth);
        OnHealthChanged?.Invoke(_health);
    }

    public void Respawn(int setHealth = 40)
    {
        _health = setHealth;
        _isDead = false;
        OnHealthChanged?.Invoke(_health);
    }

    [ContextMenu("Full Heal")]
    public void FullHeal()
    {
        _isDead = false;//this is for testing
        Heal(1000);
    }

}
