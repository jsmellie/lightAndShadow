using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class CutsceneController : SingletonBehaviour<CutsceneController>
{
    private readonly Dictionary<int, string> CHECKPOINT_CUTSCENE_LOOP = new Dictionary<int, string>()
    {
        {0, "Cutscene1Loop"},
        {6, "Cutscene2Loop"},
        {12, "Cutscene3Loop"},
        {18, "Cutscene4Loop"},
        {24, "Cutscene5Loop"},
        {32, "Cutscene6Loop"}
    };

    private readonly Dictionary<int, string> CHECKPOINT_CUTSCENE = new Dictionary<int, string>()
    {
        {0, "Cutscene1"},
        {6, "Cutscene2"},
        {12, "Cutscene3"},
        {18, "Cutscene4"},
        {24, "Cutscene5"},
        {32, "Cutscene6"}
    };

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

    public void LoopMainMenu(int currentCheckpoint, Action onLoaded)
    {
        LoadCutscene(CHECKPOINT_CUTSCENE_LOOP[currentCheckpoint], () =>
        {
            OnClipFinishedSingleAction = () =>
            {
                _videoPlayer.frame = 2;
            };

            onLoaded?.Invoke();

            PlayCutscene();
            SetVideoLooping(true);
        });
    }

    public void PlayCutsceneForCheckpoint(int currentCheckpoint)
    {
        LoadCutscene(CHECKPOINT_CUTSCENE[currentCheckpoint], () =>
        {
            PlayCutscene();
            SetVideoLooping(false);
        });
    }

    public void SetVideoLooping(bool isLooping)
    {
        _videoPlayer.isLooping = isLooping;
    }

    public void QueueCutscene1(Action onFinished)
    {
        AudioController.Instance.SetupMusic();
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

    public void QueueCutscene2(Action onFinished)
    {
        AudioController.Instance.SetupMusic();

        LoadCutscene("Cutscene2End", () =>
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

                    LoadCutscene("Cutscene2", () =>
                    {

                    });
                };
            });
    }

    public void QueueCutscene3(Action onFinished)
    {
        AudioController.Instance.SetupMusic();

        LoadCutscene("Cutscene3End", () =>
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

                    LoadCutscene("Cutscene3", () =>
                    {

                    });
                };
            });
    }

    public void QueueCutscene4(Action onFinished)
    {
        AudioController.Instance.SetupMusic();

        LoadCutscene("Cutscene4End", () =>
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

                    LoadCutscene("Cutscene4", () =>
                    {

                    });
                };
            });
    }

    public void QueueCutscene5(Action onFinished)
    {
        AudioController.Instance.SetupMusic();

        LoadCutscene("Cutscene5End", () =>
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

                    LoadCutscene("Cutscene5", () =>
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

    public void LoadCutsceneForCheckpoint(int currentCheckpoint, Action onLoadComplete = null)
    {
        LoadCutscene(CHECKPOINT_CUTSCENE[currentCheckpoint], onLoadComplete);
    }

    private void LoopPointReached(VideoPlayer source)
    {
        OnClipFinishedSingleAction?.Invoke();
    }
}
