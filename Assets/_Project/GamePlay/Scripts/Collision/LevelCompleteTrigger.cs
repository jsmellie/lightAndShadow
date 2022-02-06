using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : BaseTrigger
{
    [SerializeField] private LevelCompleteAnimationSequencer _levelCompleteSequencer = null;

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);

        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.IsControllerActive = false;

        PlayLevelCompleteSequence();
    }

    private void PlayLevelCompleteSequence()
    {
        FullScreenWipe.FadeIn(1, OnCompleteSequenceFinished);
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
        Zones currentZone = StageLoader.Instance.CurrentZone;
        int currentStageID = StageLoader.Instance.CurrentStageID;

        Zones nextZone;
        int nextStageID;
        StageProgressionTracker.GetNextStageInfo(currentZone, currentStageID, out nextZone, out nextStageID);

        Debug.Log($"Level Completed.  Loading level {nextZone} stage {nextStageID}");

        StageLoader.Instance.LoadStage(nextZone, nextStageID);
    }
}
