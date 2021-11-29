using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;

public class StageLoader : SingletonBehaviour<StageLoader>
{
    
    public delegate void OnStageLoadingCompleted(LevelLoadingErrorCodes loadingError);
    private const string _loadingScreenPath = "Prefabs/Popups/LoadingScreen";

    private const string _stageNameFormat = "{0}_Stage{1}";
    private const string _restartPopupName = "StageRestart";
    private const string _completionPopupName = "StageCompleted";
    private GameObject _loadingScreen = null;

    private string _currentStage = string.Empty;

    private Coroutine _unloadingRoutine = null;
    private Coroutine _loadingRoutine = null;

    private OnStageLoadingCompleted _cachedCallback = null;

    public bool IsStageLoaded
    {
        get { return !string.IsNullOrEmpty(_currentStage); }
    }

    public bool IsLoadingHappening
    {
        get { return _unloadingRoutine != null || _loadingRoutine != null; }
    }

    protected override void Initialize()
    {
        _loadingScreen = Instantiate<GameObject>(Resources.Load<GameObject>(_loadingScreenPath));
        _loadingScreen.SetActive(false);
        DontDestroyOnLoad(_loadingScreen);
    }

    public void LoadStage(Zones zone, int stage, OnStageLoadingCompleted onStageLoaded)
    {
        string levelName = string.Format(_stageNameFormat, zone.ToString(), stage);

        Scene newLevelScene = SceneManager.GetSceneByName(levelName);

        if (newLevelScene != null)
        {
            _cachedCallback = onStageLoaded;
            ToggleLoadingScreen();
            OnStageLoadingCompleted onStageLoadingCompleted = (LevelLoadingErrorCodes error) => {
                ToggleLoadingScreen();
                if (_cachedCallback != null)
                {
                    _cachedCallback(error);
                    _cachedCallback = null;
                }
            };
            OnStageLoadingCompleted onStageUnloaded = (LevelLoadingErrorCodes error) => {  
                _currentStage = levelName;
                _loadingRoutine = StartCoroutine(LoadStageRoutine(levelName, onStageLoadingCompleted));
            };
            if (IsStageLoaded)
            {
                _unloadingRoutine = StartCoroutine(UnloadStageRoutine(onStageUnloaded));
            }
            else
            {
                onStageUnloaded(LevelLoadingErrorCodes.None);
            }
        }
        else
        {
            Debug.LogError($"No scene found for {levelName}.  Please make sure it exists in the build settings.");
        }
    }

    private IEnumerator LoadStageRoutine(string sceneName, OnStageLoadingCompleted onStageLoaded)
    {
        LevelLoadingErrorCodes errorCode = LevelLoadingErrorCodes.None;

        if (!string.IsNullOrEmpty(sceneName))
        {
            AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (!sceneLoadOperation.isDone)
            {
                Debug.Log($"Loading Scene {sceneName} - {(sceneLoadOperation.progress * 100)}%");
                yield return null;
            }
        }
        else
        {
            errorCode = LevelLoadingErrorCodes.FailedToLoad;
        }

        if (onStageLoaded != null)
        {
            Debug.Log("Scene Loading Completed.");
            onStageLoaded(errorCode);
        }
        
        _loadingRoutine = null;
    }

    public void UnloadStage(OnStageLoadingCompleted onStageUnloaded)
    {
        if (_unloadingRoutine == null)
        {
            _unloadingRoutine = StartCoroutine(UnloadStageRoutine(onStageUnloaded));
        }
    }

    private IEnumerator UnloadStageRoutine(OnStageLoadingCompleted onStageUnloaded)
    {
        LevelLoadingErrorCodes errorCode = LevelLoadingErrorCodes.None;

        if (IsStageLoaded)
        {
            AsyncOperation sceneUnloadOperation = SceneManager.UnloadSceneAsync(_currentStage);

            while (!sceneUnloadOperation.isDone)
            {
                Debug.Log($"Unloading Scene {_currentStage} - {(sceneUnloadOperation.progress * 100)}%");
                yield return null;
            }
        }
        else
        {
            errorCode = LevelLoadingErrorCodes.FailedToLoad;
        }

        _currentStage = string.Empty;

        if (onStageUnloaded != null)
        {
            Debug.Log($"Unloading Scene Completed.");
            onStageUnloaded(errorCode);
        }
        
        _unloadingRoutine = null;
    }

    public void ReloadStage()
    {
        if (!IsLoadingHappening)
        {
            ToggleLoadingScreen();
            UnloadStage(null);
            _loadingRoutine = StartCoroutine(LoadStageRoutine(_currentStage, null));
        }
    }

    private void ToggleLoadingScreen()
    {
        if (_loadingScreen != null)
        {
            _loadingScreen.SetActive(!_loadingScreen.activeSelf);
        }
    }

    private void OnLoadingCompleted()
    {
        if(!IsLoadingHappening)
        {
            ToggleLoadingScreen();

        }
    }
}
