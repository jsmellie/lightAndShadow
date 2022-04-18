using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonBehaviour<GameController>
{
    [SerializeField] private GameObject _playerPrefab;
    
    protected override void Initialize()
    {
        
    }
}
