using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class GameController : SingletonBehaviour<GameController>
{
    public enum GameState
    {
        Menu,
        Loading,
        Cutscene,
        Playing,
        Paused
    }

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private string _baseScene;
    [SerializeField] private GameObject _menu;
    
    private GameState _currentGameState = GameState.Menu;

    protected override void Initialize()
    {
        
    }

    public async Task LoadMenu()
    {
        await Task.Delay(100);
        var handle = AddressableSceneManager.Instance.LoadScene(_baseScene, LoadSceneMode.Single, true);

        await handle.Task;
        AddressableSceneManager.Instance.LoadScenesFromString(CheckpointManager.Instance.GetScenesForCurrentCheckpoint());
        Instantiate(_menu);

        await Task.WhenAll(AddressableSceneManager.Instance.GetTasks());
        
        SetState(GameState.Menu);
    }

    public void SetState(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Menu:
                EnterMenuState();
                break;

            case GameState.Playing:
                EnterPlayState();
                break;

            case GameState.Loading:
                break;

            case GameState.Paused:
                break;

            case GameState.Cutscene:
                EnterCutsceneState();
                break;
        }
    }

    private void EnterCutsceneState()
    {       
        //play cutscene
        CutsceneController.Instance.LoadCutsceneForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint, () =>
        {
            CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(true);
            FullScreenWipe.FadeOut(0.5f);
            CutsceneController.Instance.PlayCutscene();
            CutsceneController.Instance.SetVideoLooping(false);
            CutsceneController.Instance.OnClipFinishedSingleAction = () =>
            {
                FullScreenWipe.FadeIn(1f, () => 
                {
                    CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                    LoadNextScene();
                });
            };
        });
    }

    private async void LoadNextScene()
    {
        AddressableSceneManager.Instance.UnloadScenes(CheckpointManager.Instance.GetScenesForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint - 1));

        CheckpointManager.Instance.SaveCheckpoint(CheckpointManager.Instance.CurrentCheckpoint + 1);

        await AddressableSceneManager.Instance.LoadScene(CheckpointManager.Instance.GetSceneForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint)).Task;

        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);

        FullScreenWipe.FadeOut(1f, () =>
        {
            SetState(GameState.Playing);
        });
    }

    private void EnterMenuState()
    {
        switch (CheckpointManager.Instance.CurrentCheckpoint)
        {
            case 0:
            case 6:
            case 12:
            case 18:
            case 24:
                CutsceneController.Instance.LoopMainMenu(CheckpointManager.Instance.CurrentCheckpoint);
                CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(false);
                CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(true);
                break;

            default:
                CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);
                CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                break;
        }
    }

    private void EnterPlayState()
    {
        switch (CheckpointManager.Instance.CurrentCheckpoint)
        {
            case 0:
                CutsceneController.Instance.QueueCutscene1(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        LoadNextScene();                        
                    });
                });
                break;

            case 6:
                CutsceneController.Instance.QueueCutscene2(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        LoadNextScene();
                    });
                });
                break;

            case 12:
                CutsceneController.Instance.QueueCutscene3(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        LoadNextScene();
                    });
                });
                break;

            case 18:
                CutsceneController.Instance.QueueCutscene4(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        LoadNextScene();
                    });
                });
                break;

            case 24:
                CutsceneController.Instance.QueueCutscene5(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        LoadNextScene();
                    });
                });
                break;

            default:
                PlayerController.Instance.SetInteractable(true);
                PlayerController.Instance.SetAnimationState(PlayerAnimationController.AnimationState.Movement);
                PlayerController.Instance.DetectTriggers(true);
                PlayerHealthController.Instance.SetHealthDrainPaused(false);
                break;
        }
    }
}
