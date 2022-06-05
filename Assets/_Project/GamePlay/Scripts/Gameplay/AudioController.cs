using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using static AudioDatabaseObject;
using static LayeredMusicTrackData;

public class AudioController : SingletonBehaviour<AudioController>
{
    [SerializeField] private AudioDatabase _audioDatabase;
    [SerializeField] private LayeredMusicController _layeredMusicController;
    [SerializeField] private List<AudioSource> _layeredAudioSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _soundEffectAudioSources = new List<AudioSource>();
    [SerializeField] private List<AudioSource> _musicAudioSources = new List<AudioSource>();
    [SerializeField] private AudioSource _oneShotAudioSource;

    private List<AudioClip> _loadedLayeredMusic = new List<AudioClip>();
    private List<AudioClip> _previousLoadedLayeredMusic = new List<AudioClip>();

    private List<int> _inUseSoundEffectSources = new List<int>();

    private int _currentMusicAudioSource = 0;
    private int _currentBeat = 0;

    protected override void Initialize()
    {
        _audioDatabase.LoadAudioDatabases();
    }

    private void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            LoadLayeredMusic(Resources.Load<LayeredMusicTrackData>("Audio/Music/LayeredTrack1/LayeredTrack1"));
            InitializeLayeredMusic();
        }

        if (Input.GetKeyDown("r"))
        {
            PlaySoundEffect("Test1", true);
        }

        if (Input.GetKeyDown("1"))
        {
            _layeredMusicController.DecrementLayer();
        }

        if (Input.GetKeyDown("2"))
        {
            _layeredMusicController.IncrementLayer();
        }

        UpdateLayeredAudioVolumes();
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

    public void LoadLayeredMusic(LayeredMusicTrackData trackData)
    {
        _previousLoadedLayeredMusic = _loadedLayeredMusic;
        _loadedLayeredMusic.Clear();

        List<AudioClip> audioClips = new List<AudioClip>();

        foreach(AudioSource audioSource in _layeredAudioSources)
        {
            audioSource.clip = null;
        }

        for (int i = 0; i < trackData.MusicTracks.Count;i++)
        {
            audioClips.Add(Resources.Load<AudioClip>(trackData.MusicTracks[i].TrackPath));
        }

        _loadedLayeredMusic = audioClips;

        _layeredMusicController.SetNextTrackData(trackData);
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
    }

    private void UnloadPreviousLayeredMusic()
    {
        _previousLoadedLayeredMusic.Clear();        
    }
}