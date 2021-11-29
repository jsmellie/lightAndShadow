using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGameLevelCaller : MonoBehaviour
{
    void Awake()
    {
        VirtualCameraManager.Instance.OnStageLoaded();
    }
}
