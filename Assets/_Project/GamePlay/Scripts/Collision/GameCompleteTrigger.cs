using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameCompleteTrigger : BaseTrigger
{
    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        gameObject.SetActive(false);
        PlayLevelCompleteSequence();
    }

    private void PlayLevelCompleteSequence()
    {
        PlayerHealthController.Instance.SetHealthDrainPaused(true);
#if !DISABLESTEAMWORKS
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.SetAchievement("TheEnd");
            Steamworks.SteamUserStats.StoreStats();
        }
#endif
        AudioController.Instance.PlayMusic("", false);
        CheckpointManager.Instance.SaveCheckpoint(CheckpointManager.Instance.CurrentCheckpoint + 1);
        FullScreenWipe.FadeToBlack(1, OnCompleteSequenceFinished);
    }

    private void OnCompleteSequenceFinished()
    {
        GameController.Instance.SetState(GameController.GameState.Cutscene).ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);        
    }

    private void LoadNextLevel()
    {

    }
}
