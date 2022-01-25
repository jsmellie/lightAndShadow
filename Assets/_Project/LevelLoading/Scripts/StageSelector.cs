using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadStageOne()
    {
        StageLoader.Instance.OnStageLoaded += OnStageLoaded;
        StageLoader.Instance.LoadStage(Zones.URBAN, 1);
    }

    public void LoadStageTwo()
    {
        StageLoader.Instance.OnStageLoaded += OnStageLoaded;
        StageLoader.Instance.LoadStage(Zones.URBAN, 2);
    }

    public void UnloadStage()
    {
        StageLoader.Instance.OnStageUnloaded += OnStageUnloaded;
        StageLoader.Instance.UnloadStage(OnStageUnloaded);
    }

    private void OnStageLoaded(Zones zone, int stageIndex,  LevelLoadingErrorCodes errorCode)
    {
        if (errorCode != LevelLoadingErrorCodes.None)
        {
            Debug.LogError($"Error While Loading Stage: {errorCode}");
        }
        
        StageLoader.Instance.OnStageLoaded -= OnStageLoaded;
    }

    private void OnStageUnloaded(LevelLoadingErrorCodes errorCode)
    {
        if (errorCode != LevelLoadingErrorCodes.None)
        {
            Debug.LogError($"Error While Unloading Stage: {errorCode}");
        }
        
        StageLoader.Instance.OnStageUnloaded -= OnStageUnloaded;
    }
}
