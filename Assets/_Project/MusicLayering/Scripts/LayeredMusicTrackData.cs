using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Light & Shadow/Layered Music/Data")]
public class LayeredMusicTrackData : ScriptableObject
{
    [Serializable]
    public class MusicTrack
    {
        public string TrackPath;
        public List<bool> EnabledStages;
    }

    public List<MusicTrack> MusicTracks;
    public float BPM;
}
