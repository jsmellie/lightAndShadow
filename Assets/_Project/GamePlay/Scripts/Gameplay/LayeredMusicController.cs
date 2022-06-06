using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class LayeredMusicController : MonoBehaviour
{
    private const float FADE_DURATION = 1f;

    private LayeredMusicTrackData _currentTrackData;

    private LayeredMusicTrackData _nextTrackData;

    private int _currentStage = -1;
    private int _totalLayerCount = -1;
    private int _totalStageCount = -1;

    private List<float> _layerVolumes = new List<float>();

    public void SetNextTrackData(LayeredMusicTrackData trackData)
    {
        _nextTrackData = trackData;
    }

    public List<float> GetLayerVolumes()
    {
        return _layerVolumes;
    }

    public void InitializeTrack(Action onComplete)
    {
        if (_currentTrackData != null)
        {
            Sequence fadeSequence = DOTween.Sequence();

            fadeSequence
                .Append(
                    DOTween.To(
                        () => _layerVolumes[0],
                        x => _layerVolumes[0] = x,
                        0,
                        FADE_DURATION));

            for (int i = 1; i < _layerVolumes.Count;i++)
            {
                fadeSequence
                    .Join(
                        DOTween.To(
                            () => _layerVolumes[i],
                            x => _layerVolumes[i] = x,
                            0,
                            FADE_DURATION));
            }

            fadeSequence.OnComplete(() => {
                InitializeNextTrack(onComplete);
            });
        }
        else
        {
            InitializeNextTrack(onComplete);
        }
    }

    private void InitializeNextTrack(Action onComplete)
    {
        _currentTrackData = _nextTrackData;
        _currentStage = 0;
        _totalLayerCount = _currentTrackData.MusicTracks.Count;
        _totalStageCount = _currentTrackData.MusicTracks.First().EnabledStages.Count;
        
        _layerVolumes.Clear();
        _layerVolumes = new List<float>(new float[_totalLayerCount]);

        onComplete?.Invoke();

        UpdateVolumes();
    }

    public void SetStage(int layer)
    {
        if(layer >= _totalStageCount)
        {
            _currentStage = _totalStageCount;
        }
        else
        {
            _currentStage = Math.Max(layer, 0);
        }
        UpdateVolumes();
    }

    public void IncrementLayer()
    {
        if(_currentTrackData == null)
        {
            return;
        }

        if (_currentStage < _totalStageCount - 1)
        {
            _currentStage++;

            UpdateVolumes();
        }
    }

    public void DecrementLayer()
    {
        if(_currentTrackData == null)
        {
            return;
        }

        if (_currentStage > 0)
        {
            _currentStage--;

            UpdateVolumes();
        }
    }

    private void UpdateVolumes()
    {
        for (int i = 0; i < _totalLayerCount; i++)
        {
            int index = i;
            if (_currentTrackData.MusicTracks[i].EnabledStages[_currentStage])
            {
                if (_layerVolumes[i] < 0.5f)
                {
                    DOVirtual.Float(_layerVolumes[index], 1, FADE_DURATION, (x) =>
                    {
                        _layerVolumes[index] = x;
                    });
                }
            }
            else
            {
                if (_layerVolumes[i] > 0.5f)
                {
                    DOVirtual.Float(_layerVolumes[index], 0, FADE_DURATION, (x) =>
                    {
                         _layerVolumes[index] = x;
                    });
                }
            }
        }
    }
}
