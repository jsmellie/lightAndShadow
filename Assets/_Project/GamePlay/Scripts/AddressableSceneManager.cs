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

    public AsyncOperationHandle<SceneInstance> LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Additive)
    {
        if(!_loadedScenes.ContainsKey(scene))
        {
            var handle = Addressables.LoadSceneAsync(scene, mode);
            _loadedScenes.Add(scene,handle);
            return handle;
        }
        return default;
    }

    public void LoadScenesFromString(string scenes)
    {
        string[] sceneList = scenes.Split(',');

        foreach (string scene in sceneList)
        {
            if(!_loadedScenes.ContainsKey(scene))
            {
                var handle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive);
                _loadedScenes.Add(scene,handle);
            }
        }
    }

    public void UnloadScene(string scene)
    {
        if(_loadedScenes.ContainsKey(scene))
        {
            Addressables.UnloadSceneAsync(_loadedScenes[scene], true);
            _loadedScenes.Remove(scene);
        }
    }
    
    public void UnloadScenes(string scenes)
    {
        string[] sceneList = scenes.Split(',');

        foreach (string scene in sceneList)
        {
            UnloadScene(scene);
        }
    }
}
