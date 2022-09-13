using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
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
        Paused,
        Respawning
    }

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private string _baseScene;
    [SerializeField] private GameObject _menu;
    
    private GameState _currentGameState = GameState.Menu;

    private MainMenuController _mainMenuController;
    private BugsController _bugsController;

    public GameState CurrentState
    {
        get { return _currentGameState; }
    }

    public bool CanPause()
    {
        return _currentGameState == GameState.Playing;
    }

    protected override void Initialize()
    {
        
    }

    public void PlayerDied()
    {
        SetState(GameState.Respawning).ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }

    public async Task SpawnPlayer()
    {
        await PlayerSpawnHandler.Instance.Spawn(CheckpointManager.Instance.GetCurrentCheckpoint().SpawnAnchor);

        CollectableManager.Instance.Spawn();        
    }

    public async Task LoadMenu()
    {
        await Task.Delay(100);
        var handle = AddressableSceneManager.Instance.LoadScene(_baseScene, LoadSceneMode.Single, true);

        await handle.Task;
        AddressableSceneManager.Instance.LoadScenesFromString(CheckpointManager.Instance.GetScenesForCurrentCheckpoint());
        Instantiate(_menu);

        await Task.WhenAll(AddressableSceneManager.Instance.GetTasks());
        
        await SetState(GameState.Menu);

        _mainMenuController.SceneLoaded();
    }

    public void SetMainMenuController(MainMenuController mainMenuController)
    {
        _mainMenuController = mainMenuController;
    }

    public void SetBugsController(BugsController bugsController)
    {
        _bugsController = bugsController;
    }

    public async Task SetState(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Menu:
                await EnterMenuState();
                break;

            case GameState.Playing:
                if (_currentGameState == GameState.Paused)
                {
                    _currentGameState = GameState.Playing;
                }
                else
                {
                    await EnterPlayState();
                }
                break;

            case GameState.Loading:
                break;

            case GameState.Paused:
                _currentGameState = GameState.Paused;
                break;

            case GameState.Cutscene:
                EnterCutsceneState();
                break;

            case GameState.Respawning:
                await EnterRespawnState();
                break;
        }
    }

    private async Task EnterRespawnState()
    {
        PlayerController.Instance.SetInteractable(false);
        PlayerController.Instance.DetectTriggers(false);
        PlayerHealthController.Instance.SetHealthDrainPaused(true);

        PlayerController.Instance.GetComponent<PlayerAnimationController>().PlayDeathAnimation();

        _bugsController.OverrideHealth = true;

        DOVirtual.Float(0.95f, 1.1f, 1f, (x) =>
        {
            _bugsController.SetRadius(x);
        }).SetEase(Ease.InQuad).SetDelay(2f);

        await Task.Delay(3333);

        AudioController.Instance.SetLayeredVolume(0, 1f);

        await Task.Delay(1000);

        await Respawn();

        await Task.CompletedTask;
    }

    private async Task Respawn()
    {
        PlayerHealthController.Instance.Respawn(70);

        await SpawnPlayer();

        await Task.Delay(100);

        AudioController.Instance.PlayStageMusic();
        AudioController.Instance.SetLayeredVolume(1, 1f);

        AudioController.Instance.ResyncLayeredAudio();

        await Task.Delay(500);

        await FinishedRespawning();
    }

    private async Task FinishedRespawning()
    {
        PlayerController.Instance.GetComponent<PlayerAnimationController>().PlayRespawnAnimation();

        DOVirtual.Float(1.0f, 0f, 2f, (x) =>
        {
            _bugsController.SetRadius(x);
        }).SetEase(Ease.InOutQuad);
        
        await Task.Delay(5100);

        _bugsController.OverrideHealth = false;

        await SetState(GameState.Playing);
    }

    private void EnterCutsceneState()
    {
        AudioController.Instance.SetLayeredVolume(0, 1f);

        PlayerController.Instance.SetInteractable(false);
        PlayerController.Instance.DetectTriggers(false);
        PlayerHealthController.Instance.SetHealthDrainPaused(true);

        //play cutscene
        CutsceneController.Instance.LoadCutsceneForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint, () =>
        {
            CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(true);
            FullScreenWipe.FadeOut(0.5f);
            CutsceneController.Instance.PlayCutscene();
            CutsceneController.Instance.SetVideoLooping(false);
            if(CheckpointManager.Instance.CurrentCheckpoint <= 30)
            {

            CutsceneController.Instance.OnClipFinishedSingleAction = () =>
            {
                FullScreenWipe.FadeToBlack(1f, () => 
                {
                    CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                    LoadNextScene().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
                });
            };
            }
            else
            {
                CutsceneController.Instance.OnClipFinishedSingleAction = () =>
                FullScreenWipe.FadeToBlack(1f, () => 
                {
                    CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                    CheckpointManager.Instance.ResetProgress();
                    LoadMenu().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
                });
            }
        });
    }

    private bool _startedFromMenu = false;
    private bool _startNewStageMusic = true;

    private async Task LoadNextScene()
    {
        AddressableSceneManager.Instance.UnloadScenes(CheckpointManager.Instance.GetScenesForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint - 1));

        CheckpointManager.Instance.SaveCheckpoint(CheckpointManager.Instance.CurrentCheckpoint + 1);

        await AddressableSceneManager.Instance.LoadScene(CheckpointManager.Instance.GetSceneForCheckpoint(CheckpointManager.Instance.CurrentCheckpoint)).Task;

        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);

        await Task.Delay(1);

        await SpawnPlayer();
        PlayerHealthController.Instance.Respawn(70);

        await AudioController.Instance.SetupMusic();
        _startNewStageMusic = true;

        FullScreenWipe.FadeOut(1f, () =>
        {
            SetState(GameState.Playing).ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
        });
    }

    private bool _waitingForCutscene = false;

    public void CutsceneLoaded()
    {
        _waitingForCutscene = false;
    }

    private async Task EnterMenuState()
    {
        _currentGameState = GameState.Menu;

        switch (CheckpointManager.Instance.CurrentCheckpoint)
        {
            case 0:
            case 6:
            case 12:
            case 18:
            case 24:
                _waitingForCutscene = true;

                CutsceneController.Instance.LoopMainMenu(CheckpointManager.Instance.CurrentCheckpoint, CutsceneLoaded);
                CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(false);
                CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(true);

                while (_waitingForCutscene)
                {
                    await Task.Delay(10);
                }

                break;

            default:
                CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(true);
                CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);

                while (CheckpointManager.Instance.GetCurrentCheckpoint() == null)
                {
                    await Task.Delay(10);
                }

                await AudioController.Instance.SetupMusic();

                await SpawnPlayer();
                PlayerHealthController.Instance.Respawn(70);

                PlayerController.Instance.GetComponent<PlayerAnimationController>().PlayStartLoop();

                _startedFromMenu = true;
                break;
        }
    }

    private async Task EnterPlayState()
    {
        switch (CheckpointManager.Instance.CurrentCheckpoint)
        {
            case 0:
                CutsceneController.Instance.QueueCutscene1(() =>
                {
                    FullScreenWipe.FadeToBlack(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        _currentGameState = GameState.Loading;
                        LoadNextScene().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);                        
                    });
                });
                break;

            case 6:
                CutsceneController.Instance.QueueCutscene2(() =>
                {
                    FullScreenWipe.FadeToBlack(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        _currentGameState = GameState.Loading;
                        LoadNextScene().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
                    });
                });
                break;

            case 12:
                CutsceneController.Instance.QueueCutscene3(() =>
                {
                    FullScreenWipe.FadeToBlack(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        _currentGameState = GameState.Loading;
                        LoadNextScene().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
                    });
                });
                break;

            case 18:
                CutsceneController.Instance.QueueCutscene4(() =>
                {
                    FullScreenWipe.FadeToBlack(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        _currentGameState = GameState.Loading;
                        LoadNextScene().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
                    });
                });
                break;

            case 24:
                CutsceneController.Instance.QueueCutscene5(() =>
                {
                    FullScreenWipe.FadeToBlack(1, () =>
                    {
                        CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                        _currentGameState = GameState.Loading;
                        LoadNextScene().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
                    });
                });
                break;

            default:
                AudioController.Instance.SetLayeredVolume(1, 0);

                if (_startedFromMenu)
                {
                    _startedFromMenu = false;
                    PlayerController.Instance.GetComponent<PlayerAnimationController>().PlayStartAnimation();

                    if (_startNewStageMusic)
                    {
                        _startNewStageMusic = false;
                        AudioController.Instance.PlayNewStageMusic();
                    }
                    else
                    {
                        AudioController.Instance.PlayStageMusic();
                    }

                    await Task.Delay(2133);
                }

                if (_startNewStageMusic)
                {
                    _startNewStageMusic = false;
                    AudioController.Instance.PlayNewStageMusic();
                }

                AudioController.Instance.ResyncLayeredAudio();
                
                _currentGameState = GameState.Playing;
                PlayerController.Instance.SetInteractable(true);
                PlayerController.Instance.SetAnimationState(PlayerAnimationController.AnimationState.Movement);
                PlayerController.Instance.DetectTriggers(true);
                PlayerHealthController.Instance.SetHealthDrainPaused(false);

                PlayerHealthController.Instance.OnDeath = GameController.Instance.PlayerDied;
                break;
        }
    }
}
