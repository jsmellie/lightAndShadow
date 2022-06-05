using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using static AudioDatabaseObject;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioDatabase : MonoBehaviour
{
    [SerializeField] private List<AssetReference> _audioDatabasePaths = new List<AssetReference>();

    private List<AudioDatabaseObject> _audioDatabases = new List<AudioDatabaseObject>();

    public void LoadAudioDatabases()
    {
        foreach(var path in _audioDatabasePaths)
        {
            Addressables.LoadAssetAsync<AudioDatabaseObject>(path).Completed += AddDatabase;
        }
    }

    private void AddDatabase(AsyncOperationHandle<AudioDatabaseObject> obj)
    {
        _audioDatabases.Add(obj.Result);
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
