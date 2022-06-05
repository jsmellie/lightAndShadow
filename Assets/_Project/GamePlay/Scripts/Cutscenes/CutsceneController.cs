using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : SingletonBehaviour<CutsceneController>
{
    private VideoPlayer _videoPlayer;
    private Camera _videoCamera;

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
            PlayCutscene("Cutscene1Loop");
        }
    }

    public void PlayCutscene(string key)
    {
        _videoCamera = CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID);
        _videoPlayer.targetCamera = _videoCamera;
        _videoPlayer.clip = Resources.Load<VideoClip>(key);
        _videoPlayer.Play();
    }

    private void LoopPointReached(VideoPlayer source)
    {
        _videoPlayer.frame = 2;
    }
}
