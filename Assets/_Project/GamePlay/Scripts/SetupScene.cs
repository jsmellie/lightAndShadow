using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupScene : MonoBehaviour
{
    private const string MAIN_MENU_SCENE = "MainMenu";

    private void Start()
    {
        LoadMainMenu();
    }

    private async Task LoadMainMenu()
    {
        await Task.Delay(1000);

        SceneManager.LoadScene(MAIN_MENU_SCENE, LoadSceneMode.Single);
    }
}
