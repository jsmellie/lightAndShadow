using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using static AudioDatabaseObject;
using System.Threading;
using static LayeredMusicTrackData;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioController : SingletonBehaviour<AudioController>
{
    [SerializeField] private AudioDatabase _audioDatabase;
    [SerializeField] private LayeredMusicController _layeredMusicController;
    [SerializeField] private List<AudioSource> _layeredAudioSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _soundEffectAudioSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _musicAudioSources = new List<AudioSource>();
    [SerializeField] private AudioSource _oneShotAudioSource;

    private Dictionary<int, AudioClip> _loadedLayeredMusic = new Dictionary<int, AudioClip>();
    private Dictionary<int, AudioClip> _previousLoadedLayeredMusic = new Dictionary<int, AudioClip>();

    private int _currentMusicAudioSource = 0;
    private int _currentBeat = 0;
    private float _loadedMusicBPM = 0;

    private CancellationTokenSource _tempoCancellationToken;

    public Action<int> OnBeat;

    protected override void Initialize()
    {
        _audioDatabase.LoadAudioDatabases();
    }

    private async Task MusicTempoLoop(float bpm, CancellationToken cancellationToken)
    {
        int msPerBeat = (int)(60000 / bpm);

        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            

            await Task.Delay(msPerBeat);

            if(PauseController.IsPaused)
            {
                continue;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            _currentBeat++;

            if (_currentBeat > 3)
            {
                _currentBeat = 0;
            }

            OnBeat?.Invoke(_currentBeat);
        }
    }

    private void Update()
    {
        UpdateLayeredAudioVolumes();
    }

    public void SetupMusic()
    {
        string currentTrack = GetTrackNameForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint);

        Addressables.LoadAssetAsync<LayeredMusicTrackData>(currentTrack).Completed += (x) => {
            LoadLayeredMusic(x.Result);
        };
    }

    private void UpdateLayeredAudioVolumes()
    {
        List<float> layerVolumes = _layeredMusicController.GetLayerVolumes();

        for (int i = 0; i < _layeredAudioSources.Count; i++)
        {
            if (layerVolumes.Count > i)
            {
                _layeredAudioSources[i].volume = layerVolumes[i];
            }
            else
            {
                _layeredAudioSources[i].volume = 0;
            }
        }
    }

    public void PlaySoundEffect(string clipName, bool isOneShot = false)
    {
        AudioDatabaseEntry clip = _audioDatabase.GetClip(clipName);
        AudioSource source = _oneShotAudioSource;

        bool shouldPlayOneShot = isOneShot || !clip.InterruptSameLayer || _soundEffectAudioSources.Count <= clip.Layer;

        if (shouldPlayOneShot)
        {
            source.PlayOneShot(clip.AudioClip);
        }
        else
        {
            source = _soundEffectAudioSources[clip.Layer];
            source.Stop();
            source.clip = clip.AudioClip;
            source.Play();
        }
    }

    public void PlayMusic(string clipName, bool crossfade)
    {
        AudioDatabaseEntry clip = _audioDatabase.GetClip(clipName);

        int nextAudioSource = 0;

        if (_currentMusicAudioSource == 0)
        {
            _musicAudioSources[1].clip = clip.AudioClip;
            nextAudioSource = 1;
        }
        else
        {
            _musicAudioSources[0].clip = clip.AudioClip;
        }

        if (crossfade)
        {
            _musicAudioSources[_currentMusicAudioSource].DOFade(0, 1);

            _musicAudioSources[nextAudioSource].volume = 0;
            _musicAudioSources[nextAudioSource].DOFade(1, 1);
        }
        else
        {
            _musicAudioSources[_currentMusicAudioSource].Stop();

            _musicAudioSources[nextAudioSource].volume = 1;
        }

        _musicAudioSources[nextAudioSource].Play();

        _currentMusicAudioSource = nextAudioSource;
    }

    public async void LoadLayeredMusic(LayeredMusicTrackData trackData)
    {
        _loadedMusicBPM = trackData.BPM;
        _previousLoadedLayeredMusic = new Dictionary<int, AudioClip>(_loadedLayeredMusic);
        _loadedLayeredMusic.Clear();

        foreach(AudioSource audioSource in _layeredAudioSources)
        {
            audioSource.clip = null;
        }

        var handles = new List<AsyncOperationHandle<AudioClip>>();
        for (int i = 0; i < trackData.MusicTracks.Count;i++)
        {
            int index = i;
            
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(trackData.MusicTracks[i].TrackPath);
            handle.Completed += (x) => 
                {
                    _loadedLayeredMusic.Add(index ,x.Result);
                };
                handles.Add(handle);
        }

        List<Task> tasks = new List<Task>();
        foreach (var task in handles)
        {
            tasks.Add(task.Task);
        }
        
        await Task.WhenAll(tasks);

        _layeredMusicController.SetNextTrackData(trackData);
        InitializeLayeredMusic();
    }

    public void InitializeLayeredMusic()
    {
        _layeredMusicController.InitializeTrack(() =>
        {
            UnloadPreviousLayeredMusic();
            InitializeLayeredAudioSources();
        });
    }

    private void InitializeLayeredAudioSources()
    {
        if (_tempoCancellationToken != null)
        {
            _tempoCancellationToken.Cancel();
        }

        _tempoCancellationToken = new CancellationTokenSource();
        for (int i = 0; i < _layeredAudioSources.Count; i++)
        {
            _layeredAudioSources[i].Stop();

            if (_loadedLayeredMusic.Count > i)
            {
                _layeredAudioSources[i].clip = _loadedLayeredMusic[i];
                _layeredAudioSources[i].Play();
            }
            else
            {
                _layeredAudioSources[i].clip = null;
            }
        }

        _currentBeat = 0;
        _ = MusicTempoLoop(_loadedMusicBPM, _tempoCancellationToken.Token);
    }

    private string GetTrackNameForCheckpoint(int checkpoint)
    {
        if(checkpoint < 6)
        {
            return "LayeredMusic1";
        }
        else if(checkpoint < 12)
        {
            return "LayeredMusic1"; //todo change when music exists
        }
        else if(checkpoint < 18)
        {
            return "LayeredMusic1"; //todo change when music exists
        }
        else if(checkpoint < 24)
        {
            return "LayeredMusic1"; //todo change when music exists
        }
        else
        {
            return "LayeredMusic1"; //todo change when music exists
        }
    }

    private void UnloadPreviousLayeredMusic()
    {
        _previousLoadedLayeredMusic.Clear();        
    }
}