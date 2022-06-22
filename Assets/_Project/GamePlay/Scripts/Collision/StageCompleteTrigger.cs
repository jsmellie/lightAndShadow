using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class StageCompleteTrigger : BaseTrigger
{
    public override void OnTriggerEnter(Collider collider)
    {
        AddressableSceneManager.Instance.LoadScenesFromString(CheckpointManager.Instance.GetScenesForCurrentCheckpoint());
    }
}
