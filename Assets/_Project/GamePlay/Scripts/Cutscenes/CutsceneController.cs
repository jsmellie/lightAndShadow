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

    //assign to, don't +=
    public Action OnClipFinishedSingleAction;

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
            OnClipFinishedSingleAction = () =>
            {
                _videoPlayer.frame = 2;
            };

            PlayCutscene();
            SetVideoLooping(true);
        });
    }

    public void SetVideoLooping(bool isLooping)
    {
        _videoPlayer.isLooping = isLooping;
    }

    public void QueueCutscene1(Action onFinished)
    {
        LoadCutscene("Cutscene1End", () =>
            {
                OnClipFinishedSingleAction = () =>
                {
                    OnClipFinishedSingleAction = () =>
                    {
                        OnClipFinishedSingleAction = () =>
                        {
                            onFinished?.Invoke();
                        };

                        PlayCutscene();
                        SetVideoLooping(false);
                    };

                    PlayCutscene();
                    SetVideoLooping(false);

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

    public async void LoadCutscene(string key, Action onLoadComplete = null)
    {
        _nextClip = Addressables.LoadAssetAsync<VideoClip>(key);

        await _nextClip.Task;

        onLoadComplete?.Invoke();
    }

    private void LoopPointReached(VideoPlayer source)
    {
        OnClipFinishedSingleAction?.Invoke();
    }
}
