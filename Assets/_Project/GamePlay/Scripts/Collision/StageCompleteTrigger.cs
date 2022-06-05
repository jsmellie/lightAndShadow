using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class StageCompleteTrigger : BaseTrigger
{
    [SerializeField] private AssetReference StageToLoad;


    public override void OnTriggerEnter(Collider collider)
    {
        AddressableSceneManager.Instance.LoadScene(StageToLoad);
        base.OnTriggerEnter(collider);
    }
}
