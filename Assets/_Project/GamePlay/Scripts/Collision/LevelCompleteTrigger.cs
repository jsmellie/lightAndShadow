using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : BaseTrigger
{
    [SerializeField] private string _cutsceneAddress;
    [SerializeField] private string _oldScenes;
    [SerializeField] private string _nextLevel;

    [SerializeField] private int _nextLevelFirstCheckpoint;

    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        PlayLevelCompleteSequence();
    }

    private void PlayLevelCompleteSequence()
    {
        PlayerHealthController.Instance.SetHealthDrainPaused(true);
        FullScreenWipe.FadeIn(1, OnCompleteSequenceFinished);
    }

    private void OnCompleteSequenceFinished()
    {
        //play cutscene
        CutsceneController.Instance.LoadCutscene(_cutsceneAddress, () => {
        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(true);
        FullScreenWipe.FadeOut(0.5f);
        CutsceneController.Instance.PlayCutscene();
        CutsceneController.Instance.SetVideoLooping(false);
        CutsceneController.Instance.OnClipFinishedSingleAction = () => {
            FullScreenWipe.FadeIn(0);
            CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
            AddressableSceneManager.Instance.UnloadScenes(_oldScenes);
            CheckpointManager.Instance.SaveCheckpoint(_nextLevelFirstCheckpoint);
            AddressableSceneManager.Instance.LoadScene(_nextLevel);
            FullScreenWipe.FadeOut(1f, () => {PlayerHealthController.Instance.SetHealthDrainPaused(false);}); 
        };
        });
        
    }

    private void LoadNextLevel()
    {

    }
}
