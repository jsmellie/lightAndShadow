using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.AddressableAssets;

public class CutsceneController : SingletonBehaviour<CutsceneController>
{
    private VideoPlayer _videoPlayer;
    private Camera _videoCamera;

    private VideoClip _currentClip;

    protected override void Initialize()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.None;
        _videoPlayer.aspectRatio = VideoAspectRatio.FitHorizontally;
        _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        _videoPlayer.loopPointReached += LoopPointReached;
        _videoPlayer.playOnAwake = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            LoadCutscene("Cutscene1Loop");
        }
    }

    public void PlayCutscene()
    {
        _videoCamera = CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID);
        _videoPlayer.targetCamera = _videoCamera;
        _videoPlayer.clip = _currentClip;
        _videoPlayer.Play();
    }

    private async void LoadCutscene(string key)
    {
        var handle = Addressables.LoadAssetAsync<VideoClip>(key);
        handle.Completed += (x) => 
        {
            _currentClip = x.Result;
        };
        await handle.Task;

        PlayCutscene();
        
    }

    private void LoopPointReached(VideoPlayer source)
    {
        _videoPlayer.frame = 2;
    }
}
