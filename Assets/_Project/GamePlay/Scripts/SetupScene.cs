using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SetupScene : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "MainMenu";
    [SerializeField] private GameObject _menu;
    [SerializeField] private string _baseScene;

    private void Start()
    {
        LoadMainMenu();
    }

    private async Task LoadMainMenu()
    {
        await Task.Delay(100);
        var handle = AddressableSceneManager.Instance.LoadScene(_baseScene,LoadSceneMode.Single);

        await handle.Task;
        AddressableSceneManager.Instance.LoadScenesFromString(CheckpointManager.Instance.GetScenesForCurrentCheckpoint());
        Instantiate(_menu);
        
    }
}
