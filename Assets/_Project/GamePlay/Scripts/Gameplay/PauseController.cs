using System.Collections;
using System.Collections.Generic;
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
        GameController.Instance.SetState(paused ? GameController.GameState.Paused : GameController.GameState.Playing);
    }
}
