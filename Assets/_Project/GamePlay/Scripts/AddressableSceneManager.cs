using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    public AsyncOperationHandle<SceneInstance> LoadScene(string scene, LoadSceneMode mode = LoadSceneMode.Additive, bool alwaysLoaded = false)
    {
        if(!_loadedScenes.ContainsKey(scene))
        {
            var handle = Addressables.LoadSceneAsync(scene, mode);
            if(!alwaysLoaded)
                _loadedScenes.Add(scene,handle);
            return handle;
        }
        return default;
    }

    public void LoadScenesFromString(string scenes)
    {
        List<string> sceneList = new List<string>(scenes.Split(','));

        foreach (string scene in sceneList)
        {
            if(!_loadedScenes.ContainsKey(scene))
            {
                var handle = Addressables.LoadSceneAsync(scene, LoadSceneMode.Additive);
                _loadedScenes.Add(scene,handle);
            }
        }

        List<string> toRemove = new List<string>();
        foreach (string loadedScene in _loadedScenes.Keys)
        {
            if(!sceneList.Contains(loadedScene))
            {
                toRemove.Add(loadedScene);
            }
        }
        
        foreach (string unloadScene in toRemove)
        {
            UnloadScene(unloadScene);
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

    public List<Task> GetTasks()
    {
        List<Task> taskList = new List<Task>();

        foreach(string loadedScene in _loadedScenes.Keys)
        {
            taskList.Add(_loadedScenes[loadedScene].Task);
        }

        return taskList;
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
