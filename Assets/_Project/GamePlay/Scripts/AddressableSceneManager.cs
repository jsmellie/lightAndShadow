using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class AddressableSceneManager : SingletonBehaviour<AddressableSceneManager>
{
    Dictionary<string,AsyncOperationHandle<SceneInstance>> _loadedScenes = new Dictionary<string,AsyncOperationHandle<SceneInstance>>();

    protected override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(AssetReference scene, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        if(!_loadedScenes.ContainsKey(scene.ToString()))
        {
            var handle = Addressables.LoadSceneAsync(scene, mode);
            _loadedScenes.Add(scene.ToString(),handle);
        }
        
    }


    public void UnloadScene(AssetReference scene)
    {
        if(_loadedScenes.ContainsKey(scene.ToString()))
        {
            Addressables.UnloadSceneAsync(_loadedScenes[scene.ToString()], true);
            _loadedScenes.Remove(scene.ToString());
        }
      
    }
}
