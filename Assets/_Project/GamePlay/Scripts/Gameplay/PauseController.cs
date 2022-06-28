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
        Time.timeScale = paused ? 0 : 1;
        AudioListener.pause = paused;
    }
}
