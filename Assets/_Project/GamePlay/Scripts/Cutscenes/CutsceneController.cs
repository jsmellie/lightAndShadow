using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class CutsceneController : SingletonBehaviour<CutsceneController>
{
    private VideoPlayer _videoPlayer;
    private Camera _videoCamera;

    private AsyncOperationHandle<VideoClip> _currentClip;
    private AsyncOperationHandle<VideoClip> _nextClip;

    private Action _OnClipFinishedAction;

    protected override void Initialize()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.audioOutputMode = VideoAudioOutputMode.Direct;
        _videoPlayer.aspectRatio = VideoAspectRatio.FitHorizontally;
        _videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        _videoPlayer.loopPointReached += LoopPointReached;
        _videoPlayer.playOnAwake = false;
    }

    public void LoopMainMenu()
    {
        LoadCutscene("Cutscene1Loop", () =>
        {
            _OnClipFinishedAction = () =>
            {
                _videoPlayer.frame = 2;
            };

            PlayCutscene();
            _videoPlayer.isLooping = true;
        });
    }

    public void QueueCutscene1(Action onFinished)
    {
        LoadCutscene("Cutscene1End", () =>
            {
                _OnClipFinishedAction = () =>
                {
                    _OnClipFinishedAction = () =>
                    {
                        _OnClipFinishedAction = () =>
                        {
                            onFinished?.Invoke();
                        };

                        PlayCutscene();
                        _videoPlayer.isLooping = false;
                    };

                    PlayCutscene();
                    _videoPlayer.isLooping = false;

                    LoadCutscene("Cutscene1", () =>
                    {

                    });
                };
            });
    }

    public void PlayCutscene()
    {
        if (_currentClip.IsValid())
        {
            Addressables.Release(_currentClip);
        }

        _currentClip = _nextClip;

        _videoCamera = CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID);
        _videoPlayer.targetCamera = _videoCamera;
        _videoPlayer.clip = _currentClip.Result;
        _videoPlayer.Play();
    }

    private async void LoadCutscene(string key, Action onLoadComplete)
    {
        _nextClip = Addressables.LoadAssetAsync<VideoClip>(key);

        await _nextClip.Task;

        onLoadComplete?.Invoke();
    }

    private void LoopPointReached(VideoPlayer source)
    {
        _OnClipFinishedAction?.Invoke();
    }
}
