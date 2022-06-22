using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
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
        var handle = AddressableSceneManager.Instance.LoadScene(_baseScene, LoadSceneMode.Single);

        await handle.Task;
        AddressableSceneManager.Instance.LoadScenesFromString(CheckpointManager.Instance.GetScenesForCurrentCheckpoint());
        Instantiate(_menu);
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
                break;
        }
    }

    private void EnterMenuState()
    {
        switch (CheckpointManager.Instance.CurrentCheckpoint)
        {
            case 0:
            case 6:
            case 18:
            case 24:
                CutsceneController.Instance.LoopMainMenu(CheckpointManager.Instance.CurrentCheckpoint);
                CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(false);
                break;

            default:
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
                        CheckpointManager.Instance.SaveCheckpoint(1);
                        AddressableSceneManager.Instance.LoadScene("URBAN_Stage1", LoadSceneMode.Additive);
                        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);
                    });
                });
                break;

            case 6:
                CutsceneController.Instance.QueueCutscene2(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        CheckpointManager.Instance.SaveCheckpoint(7);
                        AddressableSceneManager.Instance.LoadScene("COUNTRY_Stage1", LoadSceneMode.Additive);
                        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);
                    });
                });
                break;

            case 12:
                CutsceneController.Instance.QueueCutscene3(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        CheckpointManager.Instance.SaveCheckpoint(13);
                        AddressableSceneManager.Instance.LoadScene("FOREST_Stage1", LoadSceneMode.Additive);
                        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);
                    });
                });
                break;

            case 18:
                CutsceneController.Instance.QueueCutscene4(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        CheckpointManager.Instance.SaveCheckpoint(19);
                        AddressableSceneManager.Instance.LoadScene("FOOTHILLS_Stage1", LoadSceneMode.Additive);
                        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);
                    });
                });
                break;

            case 24:
                CutsceneController.Instance.QueueCutscene5(() =>
                {
                    FullScreenWipe.FadeIn(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        CheckpointManager.Instance.SaveCheckpoint(25);
                        AddressableSceneManager.Instance.LoadScene("MOUNTAIN_Stage1", LoadSceneMode.Additive);
                        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);
                    });
                });
                break;

            default:
                PlayerController.Instance.SetInteractable(true);
                PlayerController.Instance.SetAnimationState(PlayerAnimationController.AnimationState.Movement);
                PlayerController.Instance.DetectTriggers(true);
                break;
        }
    }
}
