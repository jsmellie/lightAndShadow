using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;

public class StageLoader : SingletonBehaviour<StageLoader>
{
    
    public delegate void OnStageLoadingCompletedDelegate(Zones zone, int stageIndex, LevelLoadingErrorCodes loadingError);
    public delegate void OnStageUnloadingCompletedDelegate(LevelLoadingErrorCodes loadingErrorCodes);
    private const string _loadingScreenPath = "Prefabs/Popups/LoadingScreen";

    private const string _stageNameFormat = "{0}_Stage{1}";
    private const string _restartPopupName = "StageRestart";
    private const string _completionPopupName = "StageCompleted";
    private GameObject _loadingScreen = null;

    private Zones _currentZone;
    private int _currentStageIndex = StageProgressionTracker.InvalidStageID;

    private Coroutine _unloadingRoutine = null;
    private Coroutine _loadingRoutine = null;

    private OnStageLoadingCompletedDelegate _cachedCallback = null;

    public event OnStageLoadingCompletedDelegate OnStageLoaded;
    public event OnStageUnloadingCompletedDelegate OnStageUnloaded;

    private string CurrentStageName
    {
        get 
        {
            if (_currentStageIndex > 0)
            {
                return FormatStageSceneName(_currentZone, _currentStageIndex); 
            }
            else
            {
                return null;
            }
        }
    }

    public Zones CurrentZone
    {
        get { return _currentZone; }
    }

    public int CurrentStageID
    {
        get { return _currentStageIndex; }
    }

    public bool IsStageLoaded
    {
        get { return !string.IsNullOrEmpty(CurrentStageName); }
    }

    public bool IsLoadingHappening
    {
        get { return _unloadingRoutine != null || _loadingRoutine != null; }
    }

    public void SetStageInfo(Zones zone, int stageIndex)
    {
        _currentStageIndex = stageIndex;
        _currentZone = zone;
    }

    public static string FormatStageSceneName(Zones zone, int stageID)
    {
        return string.Format("{0}_Stage{1}", zone, stageID); 
    }

    protected override void Initialize()
    {
        _loadingScreen = Instantiate<GameObject>(Resources.Load<GameObject>(_loadingScreenPath));
        _loadingScreen.SetActive(false);
        DontDestroyOnLoad(_loadingScreen);
    }

    public void LoadStage(Zones zone, int stage)
    {
        string levelName = string.Format(_stageNameFormat, zone.ToString(), stage);

        Scene newLevelScene = SceneManager.GetSceneByName(levelName);

        if (newLevelScene != null)
        {
            //ToggleLoadingScreen();
            OnStageLoadingCompletedDelegate onStageLoadingCompleted = (Zones zone, int stageIndex, LevelLoadingErrorCodes error) => {
                //ToggleLoadingScreen();
                if (OnStageLoaded != null)
                {
                    OnStageLoaded(zone, stageIndex, error);
                }
            };
            OnStageUnloadingCompletedDelegate onStageUnloaded = (LevelLoadingErrorCodes error) => {  
                _currentStageIndex = stage;
                _currentZone = zone;
                _loadingRoutine = StartCoroutine(LoadStageRoutine(levelName, onStageLoadingCompleted));
            };
            // if (IsStageLoaded)
            // {
            //     Debug.Log(onStageUnloaded);
            //     _unloadingRoutine = StartCoroutine(UnloadStageRoutine(onStageUnloaded));
            // }
            // else
            // {
            //     onStageUnloaded(LevelLoadingErrorCodes.None);
            // }

            onStageUnloaded(LevelLoadingErrorCodes.None);
        }
        else
        {
            Debug.LogError($"No scene found for {levelName}.  Please make sure it exists in the build settings.");
        }
    }

    private IEnumerator LoadStageRoutine(string sceneName, OnStageLoadingCompletedDelegate onStageLoaded)
    {
        LevelLoadingErrorCodes errorCode = LevelLoadingErrorCodes.None;

        if (!string.IsNullOrEmpty(sceneName))
        {
            AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

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
            onStageLoaded(_currentZone, _currentStageIndex,  errorCode);
        }
        
        _loadingRoutine = null;
    }

    public void UnloadStage(OnStageUnloadingCompletedDelegate onStageUnloaded)
    {
        if (_unloadingRoutine == null)
        {
            _unloadingRoutine = StartCoroutine(UnloadStageRoutine(onStageUnloaded));
        }
    }

    private IEnumerator UnloadStageRoutine(OnStageUnloadingCompletedDelegate onStageUnloaded)
    {
        LevelLoadingErrorCodes errorCode = LevelLoadingErrorCodes.None;

        if (IsStageLoaded)
        {
            AsyncOperation sceneUnloadOperation = SceneManager.UnloadSceneAsync(CurrentStageName);

            if (sceneUnloadOperation == null)
            {
                Debug.LogError($"Error while trying to unload scene {CurrentStageName}");
            }

            while (!sceneUnloadOperation.isDone)
            {
                Debug.Log($"Unloading Scene {CurrentStageName} - {(sceneUnloadOperation.progress * 100)}%");
                yield return null;
            }
        }
        else
        {
            errorCode = LevelLoadingErrorCodes.FailedToLoad;
        }

        _currentStageIndex = StageProgressionTracker.InvalidStageID;

        if (onStageUnloaded != null)
        {
            Debug.Log($"Unloading Scene Completed.");
            onStageUnloaded(errorCode);
        }

        if (OnStageUnloaded != null)
        {
            OnStageUnloaded(errorCode);
        }
        
        _unloadingRoutine = null;
    }

    public void ReloadStage()
    {
        if (!IsLoadingHappening)
        {
            ToggleLoadingScreen();
            UnloadStage(null);
            _loadingRoutine = StartCoroutine(LoadStageRoutine(CurrentStageName, null));
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
