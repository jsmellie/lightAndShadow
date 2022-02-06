using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class LevelCompleteAnimationSequencer
{
    public LevelCompleteAnimationSequencer()
    {
        IsPlaying = false;
    }
    public bool IsPlaying
    {
        protected set;
        get;
    }
    public abstract void PlaySequence();
}
