using System.Collections;
using System.Collections.Generic;
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
        PlayerHealthController.Instance.SetHealthDrainPaused(true);
        CheckpointManager.Instance.SaveCheckpoint(CheckpointManager.Instance.CurrentCheckpoint + 1);
        FullScreenWipe.FadeIn(1, OnCompleteSequenceFinished);
    }

    private void OnCompleteSequenceFinished()
    {
        GameController.Instance.SetState(GameController.GameState.Cutscene);        
    }

    private void LoadNextLevel()
    {

    }
}
