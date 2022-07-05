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
using UnityEngine.Audio;

public class AudioController : SingletonBehaviour<AudioController>
{
    private const string MASTER_VOLUME = "MASTER_VOLUME";

    [SerializeField] private AudioDatabase _audioDatabase;
    [SerializeField] private LayeredMusicController _layeredMusicController;
    [SerializeField] private List<AudioSource> _layeredAudioSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _soundEffectAudioSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _musicAudioSources = new List<AudioSource>();
    [SerializeField] private AudioSource _oneShotAudioSource;

    [SerializeField] private AudioMixer _audioMixer;

    private Dictionary<int, AudioClip> _loadedLayeredMusic = new Dictionary<int, AudioClip>();
    private Dictionary<int, AudioClip> _previousLoadedLayeredMusic = new Dictionary<int, AudioClip>();

    private int _currentMusicAudioSource = 0;
    private int _currentBeat = 0;
    private float _loadedMusicBPM = 0;
    private float _layeredVolume = 1;

    private int _lastLoadedCheckpoint = -1;

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

            if (!PauseController.IsPaused)
            {
                OnBeat?.Invoke(_currentBeat);
            }
        }
    }

    private void Update()
    {
        UpdateLayeredAudioVolumes();
    }

    public async Task SetupMusic()
    {
        if (_lastLoadedCheckpoint == CheckpointManager.Instance.CurrentCheckpoint)
        {
            InitializeLayeredAudioSources();
        }
        else
        {
            string currentTrack = GetTrackNameForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint);

            AsyncOperationHandle<LayeredMusicTrackData> handle = Addressables.LoadAssetAsync<LayeredMusicTrackData>(currentTrack);

            await handle.Task;
            await LoadLayeredMusic(handle.Result);
        }

        await Task.CompletedTask;
    }

    private void UpdateLayeredAudioVolumes()
    {
        List<float> layerVolumes = _layeredMusicController.GetLayerVolumes();

        for (int i = 0; i < _layeredAudioSources.Count; i++)
        {
            if (layerVolumes.Count > i)
            {
                _layeredAudioSources[i].volume = layerVolumes[i] * _layeredVolume;
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

    public async Task LoadLayeredMusic(LayeredMusicTrackData trackData)
    {
        _lastLoadedCheckpoint = CheckpointManager.Instance.CurrentCheckpoint;
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
                    if (_loadedLayeredMusic.ContainsKey(index))
                    {
                        _loadedLayeredMusic[index] = x.Result;
                    }
                    else
                    {
                        _loadedLayeredMusic.Add(index, x.Result);
                    }
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
    }

    public void PlayNewStageMusic()
    {
        InitializeLayeredMusic();
    }

    public void PlayStageMusic()
    {
        InitializeLayeredAudioSources();
    }

    public void SetLayeredVolume(float volume, float time)
    {
        DOVirtual.Float(_layeredVolume, volume, time, (x) =>
        {
            _layeredVolume = x;
        });
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

    public void ResyncLayeredAudio()
    {
        var timeSamples = _layeredAudioSources[0].timeSamples;
        
        for (int i = 1; i < _layeredAudioSources.Count;i++)
        {
            _layeredAudioSources[i].timeSamples = timeSamples;
        }
    }

    private void OnDestroy()
    {
        if (_tempoCancellationToken != null)
        {
            _tempoCancellationToken.Cancel();
        }
    }

    private string GetTrackNameForCheckpoint(int checkpoint)
    {
        if(checkpoint < 6)
        {
            return "CityLayeredTrackData";
        }
        else if(checkpoint < 12)
        {
            return "CountryLayeredTrackData"; 
        }
        else if(checkpoint < 18)
        {
            return "ForestLayeredTrackData"; 
        }
        else if(checkpoint < 24)
        {
            return "CountryLayeredTrackData"; //todo change when music exists
        }
        else
        {
            return "CountryLayeredTrackData"; //todo change when music exists
        }
    }

    private void UnloadPreviousLayeredMusic()
    {
        _previousLoadedLayeredMusic.Clear();        
    }

    public void SetPaused(bool paused)
    {
        _audioMixer.SetFloat(MASTER_VOLUME, paused ? -10f : 0f);
    }
}