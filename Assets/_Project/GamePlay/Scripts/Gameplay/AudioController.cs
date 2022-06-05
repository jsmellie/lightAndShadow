using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : SingletonBehaviour<AudioController>
{
    [SerializeField] private List<AudioSource> _layeredAudioSources = new List<AudioSource>();

    private LayeredMusicTrackData _trackData;
    private int _currentLayer = 0;
    private int _currentBeat = 0;

    protected override void Initialize()
    {

    }

    public void LoadMusic(LayeredMusicTrackData trackData)
    {
        foreach(AudioSource audioSource in _layeredAudioSources)
        {
            audioSource.clip = null;
        }

        _trackData = trackData;

        _currentLayer = -1;
    }

    public void PlayMusic(string levelName)
    {

    }
}
