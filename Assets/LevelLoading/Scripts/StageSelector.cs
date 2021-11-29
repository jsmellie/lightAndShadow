using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadStageOne()
    {
        StageLoader.Instance.LoadStage(Zones.URBAN, 1, OnStageLoaded);
    }

    public void LoadStageTwo()
    {
        StageLoader.Instance.LoadStage(Zones.URBAN, 2, OnStageLoaded);
    }

    public void UnloadStage()
    {
        StageLoader.Instance.UnloadStage(OnStageUnloaded);
    }

    private void OnStageLoaded(LevelLoadingErrorCodes errorCode)
    {
        if (errorCode != LevelLoadingErrorCodes.None)
        {
            Debug.LogError($"Error While Loading Stage: {errorCode}");
        }
    }

    private void OnStageUnloaded(LevelLoadingErrorCodes errorCode)
    {
        if (errorCode != LevelLoadingErrorCodes.None)
        {
            Debug.LogError($"Error While Unloading Stage: {errorCode}");
        }
    }
}
