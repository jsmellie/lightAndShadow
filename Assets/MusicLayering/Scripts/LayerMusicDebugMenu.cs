using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMusicDebugMenu : MonoBehaviour
{
    [SerializeField] LayeredMusicManager _musicManager;

    public void LoadTrack()
    {
        _musicManager.CONTEXT_LoadDebugTrack();
    }

    public void AddLayer()
    {
        _musicManager.AddLayer();
    }

    public void RemoveLayer()
    {
        _musicManager.RemoveLayer();
    }
}
