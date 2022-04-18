using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : BaseTrigger
{
    [SerializeField] private LevelCompleteAnimationSequencer _levelCompleteSequencer = null;

    protected override void OnTriggerEnter(Collider collider)
    {
        base.OnTriggerEnter(collider);

        //PlayerController player = collider.GetComponent<PlayerController>();
        //player.IsControllerActive = false;

        PlayLevelCompleteSequence();
    }

    private void PlayLevelCompleteSequence()
    {
        FullScreenWipe.FadeIn(1, OnCompleteSequenceFinished);
    }

    private void OnCompleteSequenceFinished()
    {
        Zones currentZone = StageController.Instance.CurrentZone;
        int currentStageID = StageController.Instance.CurrentStageID;

        Zones nextZone;
        int nextStageID;
        StageProgressionTracker.GetNextStageInfo(currentZone, currentStageID, out nextZone, out nextStageID);

        if (nextStageID != StageProgressionTracker.InvalidStageID)
        {
            if (nextZone != currentZone)
            {
                //TODO jsmellie: Do the cinematic stuff here.
                LoadNextLevel();
            }
            else
            {
                LoadNextLevel();
            }

        }
    }

    private void LoadNextLevel()
    {
        Zones currentZone = StageController.Instance.CurrentZone;
        int currentStageID = StageController.Instance.CurrentStageID;

        Zones nextZone;
        int nextStageID;
        StageProgressionTracker.GetNextStageInfo(currentZone, currentStageID, out nextZone, out nextStageID);

        Debug.Log($"Level Completed.  Loading level {nextZone} stage {nextStageID}");

        StageController.Instance.LoadStage(nextZone, nextStageID);
    }
}
