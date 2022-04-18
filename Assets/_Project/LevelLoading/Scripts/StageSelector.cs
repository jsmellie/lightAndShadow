using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadStageOne()
    {
        StageController.Instance.OnStageLoaded += OnStageLoaded;
        StageController.Instance.LoadStage(Zones.URBAN, 1);
    }

    public void LoadStageTwo()
    {
        StageController.Instance.OnStageLoaded += OnStageLoaded;
        StageController.Instance.LoadStage(Zones.URBAN, 2);
    }

    public void UnloadStage()
    {
        StageController.Instance.OnStageUnloaded += OnStageUnloaded;
        StageController.Instance.UnloadStage(OnStageUnloaded);
    }

    private void OnStageLoaded(Zones zone, int stageIndex,  LevelLoadingErrorCodes errorCode)
    {
        if (errorCode != LevelLoadingErrorCodes.None)
        {
            Debug.LogError($"Error While Loading Stage: {errorCode}");
        }

        StageController.Instance.OnStageLoaded -= OnStageLoaded;
    }

    private void OnStageUnloaded(LevelLoadingErrorCodes errorCode)
    {
        if (errorCode != LevelLoadingErrorCodes.None)
        {
            Debug.LogError($"Error While Unloading Stage: {errorCode}");
        }

        StageController.Instance.OnStageUnloaded -= OnStageUnloaded;
    }
}
