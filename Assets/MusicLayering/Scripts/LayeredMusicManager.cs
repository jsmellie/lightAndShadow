using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class LayeredMusicManager : MonoBehaviour
{
    [Header("DEUBG")]
    [SerializeField] private LayeredMusicTrackData _testingTrack;

    private AudioSource[] _audioSources = null;
    private LayeredMusicTrackLoader _currentTrack;
    private int _lastLayerIndex = -1;
    private int _totalLayerCount = -1;

    private Action<LayeredMusicManager, bool> _onLoadCompletedCallback;
    private Action<LayeredMusicManager, bool> _onUnloadCompletedCallback;

    public void Initialize()
    {
        if (_audioSources == null)
        {
            _audioSources = this.GetComponents<AudioSource>();
        }

        if (_audioSources == null)
        {
            Debug.LogError($"No AudioSources found on {this.name}.  At least one AudioSource is needed.");
        }
    }

    public void LoadTrack(LayeredMusicTrackData trackData, Action<LayeredMusicManager, bool> onLoadCompleted)
    {
        LayeredMusicTrackLoader loader = null;
        if (trackData != null)
        {
            loader = new LayeredMusicTrackLoader(trackData);
        }

        if (loader != null)
        {
            if (_currentTrack != null)
            {
                //TODO jsmellie: Unload the current track;
                _currentTrack = null;
            }

            if (loader.IsLoaded)
            {
                OnNewTrackLoaded(loader, true);
            }
            else
            {
                loader.LoadLayers(OnNewTrackLoaded);
            }
        }
    }

    [ContextMenu("Load debug track")]
    public void CONTEXT_LoadDebugTrack()
    {
        Initialize();
        LoadTrack(_testingTrack, null);
    }

    [ContextMenu("Check AudioSource Status")]
    public void CONTEXT_CheckAudioSourceStatus()
    {
        if (_audioSources != null)
        {
            for(int i = 0; i < _audioSources.Length; ++i)
            {
                Debug.Log($"Checking AudioSource {i} status - Is Playing: {_audioSources[i].isPlaying}");
            }
        }
        else
        {
            Debug.LogError("Attempting to check AudioSource status when no AudioSources found.");
        }
    }

    [ContextMenu("Add Layer")]
    public void AddLayer()
    {
        if (_currentTrack == null)
        {
            Debug.LogError($"No track loaded.");
            return;
        }
        int nextLayer = _lastLayerIndex += 1;

        if (nextLayer < _totalLayerCount)
        {
            _lastLayerIndex = nextLayer;
            AudioSource layerSource = _audioSources[nextLayer];
            layerSource.DOFade(1.0f, 1.0f);
        }
        else
        {
            Debug.LogError($"All layers already playing.");
        }
    }

    [ContextMenu("Remove layer")]
    public void RemoveLayer()
    {

        if (_currentTrack == null)
        {
            Debug.LogError($"No track loaded.");
            return;
        }
        if (_lastLayerIndex >= 0)
        {
            AudioSource layerSource = _audioSources[_lastLayerIndex];
            layerSource.DOFade(0.0f, 1.0f);
            _lastLayerIndex -= 1;
        }
        else
        {
            Debug.LogError($"All layers already muted.");
        }
    }

    private void OnNewTrackLoaded(LayeredMusicTrackLoader trackLoader, bool wasSuccessful)
    {
        Debug.Log($"On New Track Loaded - trackData {(trackLoader != null ? trackLoader.TrackData.name : "NULL")} | was successful: {wasSuccessful}");
        if (trackLoader != null && wasSuccessful)
        {
            _currentTrack = trackLoader;

            SetupAudioSources();
            StartAudioSources();
        }
        else
        {
            if (_onLoadCompletedCallback != null)
            {
                _onLoadCompletedCallback(this, false);
                _onLoadCompletedCallback = null;
            }
        }
    }

    private void SetupAudioSources()
    {
        if (_currentTrack.LoadedLayers.Length > _audioSources.Length)
        {
            Debug.LogError($"Not enough AudioSources found for all the layers of {_currentTrack.TrackData.name}");
        }

        _totalLayerCount = Mathf.Min(_currentTrack.LoadedLayers.Length, _audioSources.Length);

        for(int i = 0; i < _totalLayerCount; ++i)
        {
            _audioSources[i].clip = _currentTrack.LoadedLayers[i];
            Debug.Log($"Setting {_currentTrack.LoadedLayers[i].name} to AudioSource");
            _audioSources[i].volume = 0f;
            _audioSources[i].loop = true;
        }
    }

    private void StartAudioSources()
    {
        for(int i = 0; i < _totalLayerCount; ++i)
        {
            Debug.Log($"Starting AudioSource for {_audioSources[i].clip.name}.");
            _audioSources[i].Play();
        }
    }
}
