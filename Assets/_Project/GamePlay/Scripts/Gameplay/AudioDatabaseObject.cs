using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Light & Shadow/Audio/AudioDatabaseObject")]

[Serializable]
public class AudioDatabaseObject : ScriptableObject
{
    [Serializable]
    public class AudioDatabaseEntry
    {
        public string Key;
        public int Layer;
        public AudioClip AudioClip;
    }

    public List<AudioDatabaseEntry> Clips;
}
