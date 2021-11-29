using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeredMusicTrackLoader
{
    private LayeredMusicTrackData _trackData;
    private AudioClip[] _loadedLayers = null;

    private Action<LayeredMusicTrackLoader, bool> _onLoadCompletedCallback;

    private uint _loadingRoutineID = 0;

    public AudioClip[] LoadedLayers
    {
        get { return _loadedLayers; }
    }

    public bool IsLoaded
    {
        get { return _loadedLayers != null && _loadedLayers.Length > 0; }
    }

    public LayeredMusicTrackData TrackData
    {
        get { return _trackData; }
    }

    public LayeredMusicTrackLoader(LayeredMusicTrackData trackData)
    {
        _trackData = trackData;
    }

    public void LoadLayers(Action<LayeredMusicTrackLoader, bool> onLoadCompleted)
    {
        Debug.Log($"Loaded Layers Count: {(_loadedLayers != null ? _loadedLayers.Length.ToString() : "NULL")} | Loading Routine ID: {_loadingRoutineID}");
        if (IsLoaded || _loadingRoutineID > 0)
        {
            Debug.LogError($"Attempted to load layered music track {_trackData.name} while it's already loading");
            if (onLoadCompleted != null)
            {
                onLoadCompleted(this, false);
            }
        }
        else
        {
            _onLoadCompletedCallback = onLoadCompleted;
            _loadingRoutineID = RoutineRunner.StartRoutine(LoadLayersRoutine());
        }
    }

    public void UnloadLayers(Action<LayeredMusicTrackLoader, bool> onUnloadCompleted)
    {
        if (_loadedLayers == null || _loadingRoutineID > 0)
        {
            Debug.LogError($"Attempted to unload layered music track {_trackData.name} before it was loaded.");

            if (onUnloadCompleted != null)
            {
                onUnloadCompleted(this,false);
            }
        }
        else
        {
            for(int i = 0; i < _loadedLayers.Length; ++i)
            {
                Resources.UnloadAsset(_loadedLayers[i]);
                _loadedLayers[i] = null;
            }

            _loadedLayers = null;

            if (onUnloadCompleted != null)
            {
                onUnloadCompleted(this,true);
            }
        }
    }

    private IEnumerator LoadLayersRoutine()
    {
        bool didSucceed = false;

        if (_trackData.LayerPath != null && _trackData.LayerPath.Length > 0)
        {
            ResourceRequest[] loadingOperations = new ResourceRequest[_trackData.LayerPath.Length];
            List<int> pendingRequests = new List<int>(loadingOperations.Length);

            for(int i = 0; i < _trackData.LayerPath.Length; ++i)
            {
                loadingOperations[i] = Resources.LoadAsync<AudioClip>(_trackData.LayerPath[i]);
                pendingRequests.Add(i);
            }

            ResourceRequest currentRequest = null;

            while(pendingRequests.Count > 0)
            {
                yield return null;

                for(int i = 0; i < pendingRequests.Count; ++i)
                {
                    currentRequest = loadingOperations[pendingRequests[i]];

                    if (currentRequest == null || currentRequest.isDone)
                    {
                        pendingRequests.RemoveAt(i);
                        i--;
                        break;   
                    }
                }
            }

            List<AudioClip> loadedClips = new List<AudioClip>(loadingOperations.Length);

            for(int i = 0; i < loadingOperations.Length; ++i)
            {
                currentRequest = loadingOperations[i];
                if (currentRequest.asset != null)
                {
                    loadedClips.Add((AudioClip)currentRequest.asset);
                }
            }

            Debug.Log("Adding clips to loaded layers.");
            _loadedLayers = loadedClips.ToArray();

            didSucceed = true;
        }

        _loadingRoutineID = 0;

        if (_onLoadCompletedCallback != null)
        {
            _onLoadCompletedCallback(this, didSucceed);
            _onLoadCompletedCallback = null;
        }
    }
}
