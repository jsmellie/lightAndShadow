using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static AudioDatabaseObject;

public class AudioDatabase : MonoBehaviour
{
    [SerializeField] private List<string> _audioDatabasePaths = new List<string>();

    private List<AudioDatabaseObject> _audioDatabases = new List<AudioDatabaseObject>();

    public void LoadAudioDatabases()
    {
        foreach(string path in _audioDatabasePaths)
        {
            _audioDatabases.Add(Resources.Load<AudioDatabaseObject>(path));
        }
    }

    public AudioDatabaseEntry GetClip(string key)
    {
        AudioDatabaseObject selectedDatabase = _audioDatabases.FirstOrDefault(x => x.Clips.Any(y => y.Key.Equals(key)));

        if (selectedDatabase != null)
        {
            return selectedDatabase.Clips.FirstOrDefault(x => x.Key.Equals(key));
        }

        return null;
    }
}
