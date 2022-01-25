using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : BaseTrigger
{
    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        PlayLevelCompleteSequence();
    }

    private void PlayLevelCompleteSequence()
    {
        //TODO jsmellie: What ever we want (fade, w.e)
        OnCompleteSequenceFinished();

    }

    private void OnCompleteSequenceFinished()
    {
        Zones currentZone = StageLoader.Instance.CurrentZone;
        int currentStageID = StageLoader.Instance.CurrentStageID;

        Zones nextZone;
        int nextStageID;
        StageProgressionTracker.GetNextStageInfo(currentZone, currentStageID, out nextZone, out nextStageID);

        if (nextStageID != StageProgressionTracker.InvalidStageID)
        {
            if (nextZone != currentZone)
            {
                //TODO jsmellie: Play the cinematic
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
        Zones currentZone = StageLoader.Instance.CurrentZone;
        int currentStageID = StageLoader.Instance.CurrentStageID;

        Zones nextZone;
        int nextStageID;
        StageProgressionTracker.GetNextStageInfo(currentZone, currentStageID, out nextZone, out nextStageID);

        Debug.Log($"Level Completed.  Loading level {nextZone} stage {nextStageID}");

        StageLoader.Instance.LoadStage(nextZone, nextStageID);
    }
}
