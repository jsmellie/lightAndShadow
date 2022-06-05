using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class UnloadStageTrigger : BaseTrigger
{
    [SerializeField] private AssetReference StageToUnload;


    public override void OnTriggerEnter(Collider collider)
    {        
        AddressableSceneManager.Instance.UnloadScene(StageToUnload);

        base.OnTriggerEnter(collider);
    }
}
