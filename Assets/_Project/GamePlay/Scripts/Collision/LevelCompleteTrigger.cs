using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelCompleteTrigger : BaseTrigger
{
    public override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);
        PlayLevelCompleteSequence();
    }

    private void PlayLevelCompleteSequence()
    {
        PlayerController.Instance.SetInteractable(false);
        PlayerHealthController.Instance.SetHealthDrainPaused(true);
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
