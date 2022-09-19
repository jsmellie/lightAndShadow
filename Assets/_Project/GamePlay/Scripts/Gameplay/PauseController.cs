using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class PauseController
{
    private static bool _isPaused = false;
    public static bool IsPaused 
    {
        get { return _isPaused; }
    }
    public static void SetPause(bool paused)
    {
        _isPaused = paused;
        AudioController.Instance.SetPaused(paused);
        GameController.Instance.SetState(paused ? GameController.GameState.Paused : GameController.GameState.Playing).ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
        CutsceneController.Instance.Pause(paused);
    }
}
