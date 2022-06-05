using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressableSceneManager : SingletonBehaviour<AddressableSceneManager>
{
    Dictionary<AssetReference,AsyncOperationHandle<SceneInstance>> _loadedScenes = new Dictionary<AssetReference,AsyncOperationHandle<SceneInstance>>();

    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(AssetReference scene)
    {
       var handle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive);
        _loadedScenes.Add(scene,handle);
    }


    public void UnloadScene(AssetReference scene)
    {
        if(_loadedScenes.ContainsKey(scene))
        {
            Addressables.UnloadSceneAsync(_loadedScenes[scene]);
            _loadedScenes.Remove(scene);
        }
    }
}
